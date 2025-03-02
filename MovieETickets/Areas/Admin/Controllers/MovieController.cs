using Microsoft.AspNetCore.Mvc;
using Movie_Point.Repository.IRepository;
using MovieETickets.Models;
using MovieETickets.Repositories.IRepositories;

namespace MovieETickets.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public MovieController(IMovieRepository movieRepository, ICategoryRepository categoryRepository,
            ICinemaRepository cinemaRepository, IActorRepository actorRepository, IActorMovieRepository actorMovieRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
            this.actorRepository = actorRepository;
            this.actorMovieRepository = actorMovieRepository;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.Get(
                filter: e => e.CategoryId == e.Category.Id && e.CinemaId == e.Cinema.Id, // Filtering condition
                includes:
                [
                    e => e.Cinema,
                    e => e.Category
                ]
            );
            return View(movies.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var category = categoryRepository.Get();
            ViewBag.category = category.ToList();
            var cinema = cinemaRepository.Get();
            ViewBag.cinema = cinema.ToList();
            var actor = actorRepository.Get();
            ViewBag.actor = actor.ToList();
            //ViewBag.MovieStatus = new SelectList(Enum.GetValues(typeof(MovieStatus)));
            return View(new Movie());
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile file, int[] actorsId)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                #region Save img into wwwroot
                if (file != null && file.Length > 0)
                {
                    // File Name, File Path
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    // Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save img into db
                    movie.ImgUrl = fileName;
                }
                #endregion
                movieRepository.Create(movie);
                movieRepository.Commit();

                foreach (var actorId in actorsId)
                {
                    actorMovieRepository.Create(new ActorMovie { MovieId = movie.Id, ActorId = actorId });
                }
                actorMovieRepository.Commit();
                //TempData["notification"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }

            var category = categoryRepository.Get();
            ViewBag.category = category.ToList();
            var cinema = cinemaRepository.Get();
            ViewBag.cinema = cinema.ToList();
            var actor = actorRepository.Get();
            ViewBag.actor = actor.ToList();
            return View(movie);


        }

        [HttpGet]
        public IActionResult Edit(int movieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == movieId);
            if (movie == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            // Get Selected Actors for the Movie
            var selectedActors = actorMovieRepository.Get(e => e.MovieId == movieId)
                                                     .Select(a => a.ActorId)
                                                     .ToList();

            ViewBag.SelectedActors = selectedActors;
            ViewBag.Category = categoryRepository.Get().ToList();
            ViewBag.Cinema = cinemaRepository.Get().ToList();
            ViewBag.Actor = actorRepository.Get().ToList();

            return View(movie);
        }


        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile file, int[] actorsId)
        {
            #region Save img into wwwroot
            var movieImg = movieRepository.GetOne(e => e.Id == movie.Id);

            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movieImg.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                movie.ImgUrl = fileName;
            }
            else
            {
                movie.ImgUrl = movieImg.ImgUrl;
            }
            #endregion

            if (movie != null)
            {
                // Save movie changes first
                movieRepository.Edit(movie);
                movieRepository.Commit();

                // Get current actor-movie records
                var checkEditMovie = actorMovieRepository.Get(e => e.MovieId == movie.Id).ToList();

                // Remove actors that are NOT in the new list
                foreach (var actorMovie in checkEditMovie)
                {
                    if (!actorsId.Contains(actorMovie.ActorId))
                    {
                        actorMovieRepository.Delete(actorMovie);
                    }
                }

                // Add new actors that are not already in the database
                foreach (var actorId in actorsId)
                {
                    if (!checkEditMovie.Any(a => a.ActorId == actorId))
                    {
                        actorMovieRepository.Create(new ActorMovie { MovieId = movie.Id, ActorId = actorId });
                    }
                }

                actorMovieRepository.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }


        public ActionResult Delete(int movieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == movieId);

            if (movie != null)
            {
                if (!string.IsNullOrEmpty(movie.ImgUrl))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies", movie.ImgUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                movieRepository.Delete(movie);
                movieRepository.Commit();
                //TempData["notifation"] = "Delete Movie successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MovieETickets.Models;
using MovieETickets.Repositories;

namespace MovieETickets.Controllers
{
    public class MovieController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly MovieRepository movieRepository = new MovieRepository();
        private readonly CategoryRepository categoryRepository = new CategoryRepository();
        private readonly CinemaRepository cinemaRepository = new CinemaRepository();

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

            //ViewBag.MovieStatus = new SelectList(Enum.GetValues(typeof(MovieStatus)));
            return View(new Movie());
        }


        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile file)
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
                //TempData["notification"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }
            var category = categoryRepository.Get();
            ViewBag.category = category.ToList();
            var cinema = cinemaRepository.Get();
            ViewBag.cinema = cinema.ToList();
            return View(movie);


        }



        [HttpGet]
        public IActionResult Edit(int movieId)
        {
            var category = categoryRepository.Get();
            ViewBag.category = category.ToList();
            var cinema = cinemaRepository.Get();
            ViewBag.cinema = cinema.ToList();
            var movie = movieRepository.GetOne(e => e.Id == movieId);
            if (movie != null)
            {
                return View(movie);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Edit(Movie movie, IFormFile file)
        {
            #region Save img into wwwroot
            var movieImg = movieRepository.GetOne(e => e.Id == movie.Id);

            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movieImg.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Save img into db
                movie.ImgUrl = fileName;
            }
            else
            {
                movie.ImgUrl = movieImg.ImgUrl;
            }
            #endregion

            if (movie != null)
            {
                movieRepository.Edit(movie);
                movieRepository.Commit();
                //TempData["notifation"] = "Update Movie successfuly";
                var category = categoryRepository.Get();
                ViewBag.category = category.ToList();
                var cinema = cinemaRepository.Get();
                ViewBag.cinema = cinema.ToList();
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

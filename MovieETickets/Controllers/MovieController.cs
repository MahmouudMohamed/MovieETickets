using Microsoft.AspNetCore.Mvc;
using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories;

namespace MovieETickets.Controllers
{
    public class MovieController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        MovieRepository movieRepository = new MovieRepository();
        CategoryRepository catRepository = new CategoryRepository();
        CinemaRepository cinemaRepository = new CinemaRepository();

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
            var category = dbContext.Categories;
            ViewBag.category = category.ToList();
            var cinema = dbContext.Cinemas;
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
                dbContext.Add(movie);

                dbContext.SaveChanges();
                TempData["notification"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }
            var category = dbContext.Categories;
            ViewBag.category = category.ToList();
            var cinema = dbContext.Cinemas;
            ViewBag.cinema = cinema.ToList();
            return View(movie);


        }
    }
}

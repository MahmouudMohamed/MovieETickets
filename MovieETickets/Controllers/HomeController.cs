using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories;
using System.Diagnostics;

namespace MovieETickets.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly MovieRepository movieRepository = new MovieRepository();

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult Details(int movieId)
        {
            var movie = dbContext.Movies.Include(e => e.Category)
                .Include(e => e.Cinema).Include(e => e.actorsMovies).ThenInclude(e => e.Actor).FirstOrDefault(e => e.Id == movieId);
            return View(movie);
        }

        public IActionResult ShowAllMoviesInSameCategory(string categoryName)
        {
            var movies = movieRepository.Get(
                filter: e => e.Category.Name == categoryName, // Filtering condition
                includes:
                [
            e => e.Cinema,
            e => e.Category
                ]
            );
            return View(movies.ToList());
        }

        public IActionResult ShowAllMoviesInSameCinema(string cinemaName)
        {
            var movies = movieRepository.Get(
                filter: e => e.Cinema.Name == cinemaName,// Filtering condition
                includes:
                [
            e => e.Cinema,
            e => e.Category
                ]
            );
            return View(movies);
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

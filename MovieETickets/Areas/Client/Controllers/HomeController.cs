using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories.IRepositories;
using System.Diagnostics;

namespace MovieETickets.Areas.Client.Controllers
{
    [Area("Client")]

    public class HomeController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(IMovieRepository movieRepository, ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            this.movieRepository = movieRepository;
            _logger = logger;
            this.dbContext = dbContext;
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
            //var movie = movieRepository.Get(filter: e => e.Id == movieId, includes: [e => e.Category, e => e.Cinema, e => e.actorsMovies]);
            var movie = dbContext.Movies
                 .Include(e => e.Category)
                 .Include(e => e.Cinema)
                 .Include(e => e.actorsMovies).ThenInclude(e => e.Actor)
                 .FirstOrDefault(e => e.Id == movieId);
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

        public IActionResult ActorDetails(int actorId)
        {
            var actor = dbContext.actors
                .Include(a => a.actorsMovies) // Include join table
                .ThenInclude(am => am.Movie) // Include movies through the join table
                .FirstOrDefault(a => a.Id == actorId);
            return View(actor);
        }

        public IActionResult SearchForMovie(string Name)
        {
            var movies = dbContext.Movies
                            .Include(e => e.Cinema)
                            .Include(e => e.Category)
                            .Where(e => e.Name.Contains(Name))
                            .ToList();
            if (movies.Count != 0)
                return View(movies);
            return RedirectToAction("NotFoundPage", "Home");
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

using Microsoft.AspNetCore.Mvc;
using Movie_Point.Repository;
using MovieETickets.Repositories;

namespace MovieETickets.Area.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieRepository movieRepository = new MovieRepository();
        private readonly CategoryRepository categoryRepository = new CategoryRepository();
        private readonly CinemaRepository cinemaRepository = new CinemaRepository();
        private readonly ActorRepository actorRepository = new ActorRepository();
        private readonly ActorMovieRepository actorMovieRepository = new ActorMovieRepository();

        [Area("Admin")]
        public IActionResult Index()
        {
            var category = categoryRepository.Get();
            ViewBag.category = category.ToList().Count();
            var cinema = cinemaRepository.Get();
            ViewBag.cinema = cinema.ToList().Count();
            var actor = actorRepository.Get();
            ViewBag.actor = actor.ToList().Count();
            var movie = movieRepository.Get();
            ViewBag.moive = movie.ToList().Count();


            return View();
        }
    }
}

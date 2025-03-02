using Microsoft.AspNetCore.Mvc;
using Movie_Point.Repository.IRepository;
using MovieETickets.Repositories.IRepositories;

namespace MovieETickets.Area.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public HomeController(IMovieRepository movieRepository, ICategoryRepository categoryRepository,
            ICinemaRepository cinemaRepository, IActorRepository actorRepository, IActorMovieRepository actorMovieRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
            this.actorRepository = actorRepository;
            this.actorMovieRepository = actorMovieRepository;
        }

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

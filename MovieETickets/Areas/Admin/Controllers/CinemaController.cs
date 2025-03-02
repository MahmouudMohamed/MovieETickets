using Microsoft.AspNetCore.Mvc;
using MovieETickets.Models;
using MovieETickets.Repositories;

namespace MovieETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        CinemaRepository cinemaRepository = new CinemaRepository();


        public IActionResult Index()
        {
            var cinema = cinemaRepository.Get();
            return View(cinema.ToList());
        }

        public IActionResult Create()
        {
            return View(new Cinema());
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile file)
        {
            ModelState.Remove("Movies");
            ModelState.Remove("CinemaLogo"); // Prevents automatic validation failure

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("CinemaLogo", "Cinema logo is required.");
            }

            if (ModelState.IsValid)
            {
                #region Save Image to wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                cinema.CinemaLogo = fileName;
                #endregion

                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();

                TempData["notification"] = "Cinema added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(cinema);
        }


        [HttpGet]
        public IActionResult Edit(int cinemaId)
        {
            var cinema = cinemaRepository.GetOne(e => e.Id == cinemaId);
            if (cinema != null)
            {
                return View(cinema);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile file)
        {
            #region Save img into wwwroot
            var cinema1 = cinemaRepository.GetOne(e => e.Id == cinema.Id);

            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", cinema1.CinemaLogo);
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
                cinema.CinemaLogo = fileName;
            }
            else
            {
                cinema.CinemaLogo = cinema1.CinemaLogo;
            }
            #endregion

            if (cinema != null)
            {
                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();
                //dbContext.Cinemas.Update(cinema);
                //dbContext.SaveChanges();
                TempData["notifation"] = "Update Cinema successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        //public IActionResult Edit(Cinema cinema)
        //{
        //    ModelState.Remove("CinemaLogo");
        //    ModelState.Remove("Movies");
        //    if (ModelState.IsValid)
        //    {
        //        cinemaRepository.Edit(cinema);
        //        cinemaRepository.Commit();


        //        TempData["notifation"] = "Update cinema successfuly";

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return RedirectToAction("NotFoundPage", "Home");
        //}

        public ActionResult Delete(int cinemaId)
        {
            var cinema = cinemaRepository.GetOne(e => e.Id == cinemaId);

            if (cinema != null)
            {
                if (!string.IsNullOrEmpty(cinema.CinemaLogo))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cinema", cinema.CinemaLogo);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                cinemaRepository.Delete(cinema);
                cinemaRepository.Commit();
                TempData["notifation"] = "Delete Cinema successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult ShowAllMovies(string cinemaName)
        {
            return RedirectToAction("ShowAllMoviesInSameCinema", "Home");
        }

    }
}

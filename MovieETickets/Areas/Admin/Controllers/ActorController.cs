using Microsoft.AspNetCore.Mvc;
using Movie_Point.Repository.IRepository;
using MovieETickets.Models;

namespace MovieETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        IActorRepository actorRepository;

        public ActorController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }
        public IActionResult Index()
        {
            var actors = actorRepository.Get();

            return View(actors);
        }

        public IActionResult Create()
        {
            return View(new Actor());
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile ProfilePicture)
        {
            ModelState.Remove("file");
            //ModelState.Remove("ProfilePicture");
            ModelState.Remove("movies");
            ModelState.Remove("actorsMovies");
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    #region Save img into wwwroot
                    // File Name, File Path
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                    // Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ProfilePicture.CopyTo(stream);
                    }
                    // Save img into db
                    actor.ProfilePicture = fileName;
                    #endregion
                }
                actorRepository.Create(actor);
                actorRepository.Commit();
                TempData["notifation"] = "Add Actor successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(actor);

        }


        [HttpGet]
        public IActionResult Edit(int actorId)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorId);
            if (actor == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(actor);
        }

        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile ProfilePicture)
        {
            #region Save img into wwwroot
            var actorImg = actorRepository.GetOne(e => e.Id == actor.Id);

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actorImg.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    ProfilePicture.CopyTo(stream);
                }

                actor.ProfilePicture = fileName;
            }
            else
            {
                actor.ProfilePicture = actorImg.ProfilePicture;
            }
            #endregion

            if (actor != null)
            {
                actorRepository.Edit(actor);
                actorRepository.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
















        public ActionResult Delete(int actorId)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorId);

            if (actor != null)
            {
                if (!string.IsNullOrEmpty(actor.ProfilePicture))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cast", actor.ProfilePicture);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                actorRepository.Delete(actor);
                actorRepository.Commit();


                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}




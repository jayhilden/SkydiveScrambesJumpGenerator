using System.Collections.Generic;
using System.Web.Mvc;
using Scrambles.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    [Authorize]
    public class ScoresController : Controller
    {
        private readonly ScoresWebService _scoresWebService;

        public ScoresController(ScoresWebService scoresWebService)
        {
            _scoresWebService = scoresWebService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var vm = _scoresWebService.GetScoreListVM();
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var row = _scoresWebService.GetEditModel(id);
            return View(row);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScoresEditModel editModel)
        {
            var dupCheck = new HashSet<int> {editModel.DownJumper1};
            if (dupCheck.Contains(editModel.DownJumper2))
            {
                ModelState.AddModelError(nameof(editModel.DownJumper2), "Jumper cannot be listed twice.");
            }
            dupCheck.Add(editModel.DownJumper2);
            if (dupCheck.Contains(editModel.UpJumper1))
            {
                ModelState.AddModelError(nameof(editModel.UpJumper1), "Jumper cannot be listed twice.");
            }
            dupCheck.Add(editModel.UpJumper1);
            if (dupCheck.Contains(editModel.UpJumper2))
            {
                ModelState.AddModelError(nameof(editModel.UpJumper2), "Jumper cannot be listed twice.");
            }
            dupCheck.Add(editModel.UpJumper2);
            if (ModelState.IsValid)
            {
                _scoresWebService.Save(editModel);
                return RedirectToAction("Index");
            }

            _scoresWebService.PopulateLists(editModel);
            return View(editModel);
        }

        [AllowAnonymous]
        public ActionResult Results()
        {
            var data = _scoresWebService.GetResultsViewModel();
            return View(data);
        }
    }
}
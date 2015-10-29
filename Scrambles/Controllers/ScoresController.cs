using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Sql.Models;
using Scrambles.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    public class ScoresController : Controller
    {
        private readonly ScoresWebService _scoresWebService;

        public ScoresController(ScoresWebService scoresWebService)
        {
            _scoresWebService = scoresWebService;
        }

        // GET: Scores
        public ActionResult Index()
        {
            var list = _scoresWebService.GetList();
            return View(list);
        }

        public ActionResult Edit(int id)
        {
            var row = _scoresWebService.GetEditModel(id);
            return View(row);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScoresEditModel roundData)
        {
            if (ModelState.IsValid)
            {
                _scoresWebService.Save(roundData);
                return RedirectToAction("Index");
            }
            return View(roundData);
        }

        public ActionResult Results()
        {
            var data = _scoresWebService.GetResultsViewModel();
            return View(data);
        }
    }
}
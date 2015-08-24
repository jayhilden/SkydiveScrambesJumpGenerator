﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}
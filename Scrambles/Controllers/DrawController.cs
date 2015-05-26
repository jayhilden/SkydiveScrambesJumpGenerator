using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Sql;
using Data.Sql.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    public class DrawController : Controller
    {
        private readonly DrawWebService _drawWebService;

        public DrawController(DrawWebService drawWebService)
        {
            _drawWebService = drawWebService;
        }


        // GET: Draw
        public ActionResult Index()
        {
            var left = _drawWebService.GetDraw(JumpGroupFlag.Left);
            var right = _drawWebService.GetDraw(JumpGroupFlag.Right);
            var all = left.ToList();
            all.AddRange(right);
            return View(all);
        }
    }
}
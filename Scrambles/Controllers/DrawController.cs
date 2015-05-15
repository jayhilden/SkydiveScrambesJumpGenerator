using System;
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
        private readonly PiiaDb _db = new PiiaDb();
        private readonly DrawWebService _drawWebService;

        public DrawController()
        {
            _drawWebService = new DrawWebService(_db);
        }

        // GET: Draw
        public ActionResult Index()
        {
            var left = _drawWebService.GetDraw(JumpGroupFlag.Left);
            return View(left);
        }
    }
}
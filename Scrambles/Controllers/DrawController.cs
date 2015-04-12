using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Sql;

namespace Scrambles.Controllers
{
    public class DrawController : Controller
    {
        private readonly PiiaDb _db = new PiiaDb();

        // GET: Draw
        public ActionResult Index()
        {
            return View(_db);
        }
    }
}
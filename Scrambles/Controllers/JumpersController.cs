using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Sql;
using Data.Sql.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    public class JumpersController : Controller
    {
        private readonly PiiaDb _db;
        private readonly RandomizationWebService _randomizationWebService;

        public JumpersController(PiiaDb db, RandomizationWebService randomizationWebService)
        {
            _db = db;
            _randomizationWebService = randomizationWebService;
        }


        // GET: Jumpers
        public ActionResult Index()
        {
            return View(_db.Jumpers.ToList());
        }

        // GET: Jumpers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jumper jumper = _db.Jumpers.Find(id);
            if (jumper == null)
            {
                return HttpNotFound();
            }
            return View(jumper);
        }

        // GET: Jumpers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jumpers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JumperID,FirstName,LastName,NumberOfJumps,Organizer,Paid,Comment,RandomizedUpDown,RandomizedLetter")] Jumper jumper)
        {
            if (ModelState.IsValid)
            {
                _db.Jumpers.Add(jumper);
                _db.SaveChanges();
                _randomizationWebService.RemoveRandomization();
                return RedirectToAction("Index");
            }

            return View(jumper);
        }

        // GET: Jumpers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jumper jumper = _db.Jumpers.Find(id);
            if (jumper == null)
            {
                return HttpNotFound();
            }
            return View(jumper);
        }

        // POST: Jumpers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Jumper jumper)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(jumper).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jumper);
        }

        // GET: Jumpers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jumper jumper = _db.Jumpers.Find(id);
            if (jumper == null)
            {
                return HttpNotFound();
            }
            return View(jumper);
        }

        // POST: Jumpers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _randomizationWebService.RemoveRandomization();
            Jumper jumper = _db.Jumpers.Find(id);
            _db.Jumpers.Remove(jumper);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Randomize()
        {
            try
            {
                _randomizationWebService.Randomize();
            }
            catch (Exception e)
            {
                return View("RandomizationError", e);
                //return RedirectToAction("RandomizationError", e);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RandomizationError(Exception e)
        {
            return View(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

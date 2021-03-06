﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Data.Sql;
using Data.Sql.Models;
using Scrambles.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    [Authorize]
    public class RoundsController : Controller
    {
        private readonly PiiaDb _db;

        public RoundsController(PiiaDb db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var vm = new RoundListVM
            {
                Rounds = _db.Rounds.OrderBy(x=>x.RoundNumber).ToList(),
                IsAdmin = UserService.IsAdmin()
            };
            return View(vm);
        }

        // GET: Rounds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var round = _db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // GET: Rounds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoundID,RoundNumber,Formations")] Round round)
        {
            if (ModelState.IsValid)
            {
                _db.Rounds.Add(round);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(round);
        }

        // GET: Rounds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = _db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // POST: Rounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoundID,RoundNumber,Formations")] Round round)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(round).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(round);
        }

        // GET: Rounds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = _db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // POST: Rounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Round round = _db.Rounds.Find(id);
            _db.Rounds.Remove(round);
            _db.SaveChanges();
            return RedirectToAction("Index");
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

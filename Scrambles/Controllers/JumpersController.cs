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

namespace Scrambles.Controllers
{
    public class JumpersController : Controller
    {
        private PiiaDb db = new PiiaDb();

        // GET: Jumpers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Jumpers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jumper jumper = db.Users.Find(id);
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
                db.Users.Add(jumper);
                db.SaveChanges();
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
            Jumper jumper = db.Users.Find(id);
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
        public ActionResult Edit([Bind(Include = "JumperID,FirstName,LastName,NumberOfJumps,Organizer,Paid,Comment,RandomizedUpDown,RandomizedLetter")] Jumper jumper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jumper).State = EntityState.Modified;
                db.SaveChanges();
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
            Jumper jumper = db.Users.Find(id);
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
            Jumper jumper = db.Users.Find(id);
            db.Users.Remove(jumper);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

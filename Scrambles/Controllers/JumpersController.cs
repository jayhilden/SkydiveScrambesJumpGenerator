using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Data.Sql;
using Data.Sql.Models;
using Scrambles.Models;
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
            var model = new JumperListViewModel
            {
                Jumpers = _db.Jumpers.ToList(),
                RandomizationLocked = _randomizationWebService.RandomizationLocked()
            };
            return View(model);
        }


        // GET: Jumpers/Create
        public ActionResult Create()
        {
            var model = new Jumper();
            return View("Edit", model);
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
            var debug = ModelState.GetErrors();
            if (ModelState.IsValid)
            {
                if (jumper.JumperID == 0)
                {
                    _db.Jumpers.Add(jumper);
                }
                else
                {
                    _db.Entry(jumper).State = EntityState.Modified;
                }
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
            var jumper = _db.Jumpers.Find(id);
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

        public ActionResult LockUnlockRandomization(bool locked)
        {
            _randomizationWebService.LockUnlockRandomization(locked);
            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Rewarding.Models;

namespace Rewarding.Controllers
{
    public class RewardsController : Controller
    {
        private PersonContext db = new PersonContext();

        // GET: Rewards
        public ActionResult Index()
        {
            return View(db.Rewards.ToList());
        }

        // GET: Rewards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // GET: Rewards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rewards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Rewards.Add(reward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reward);
        }

        // GET: Rewards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Rewards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reward reward = db.Rewards.Find(id);
            if (reward.Persons.Any())
            {
                ViewBag.Message = "You can't delete the reward. It's used in another table";
                return View("DeleteFailure");
            }
            else
            {
                if (reward == null)
                {
                    return HttpNotFound();
                }
                return View(reward);
            }
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reward reward = db.Rewards.Find(id);
            db.Rewards.Remove(reward);
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

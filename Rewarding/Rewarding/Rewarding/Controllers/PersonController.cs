using System.Linq;
using System.Web.Mvc;
using Rewarding.Models;
using System.Net;
using System.Data.Entity;

namespace Rewarding.Controllers
{
    public class PersonController : Controller
    {
        PersonContext db = new PersonContext();
        // GET: Person
        public ActionResult Index()
        {
            var persons = db.Persons.Include(i => i.Rewards);
            return View(persons);
        }
        
        //GET
        public ActionResult Create()
        {
            ViewBag.Rewards = db.Rewards.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Birthdate,Age,Rewards")] Person person, int[] selectedRewards)
        {
            if (selectedRewards != null)
            {
                //получаем выбранные rewards
                foreach (var c in db.Rewards.Where(co => selectedRewards.Contains(co.Id)))
                {
                    person.Rewards.Add(c);
                }
            }

            db.Persons.Add(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Persons/Details/1 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: /Persons/Edit/1 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rewards = db.Rewards.ToList();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Birthdate,Age")] Person person, int[] selectedRewards)
        {
            Person newPerson = db.Persons.Find(person.Id);
            newPerson.Name = person.Name;
            newPerson.Birthdate = person.Birthdate;
            newPerson.Age = person.Age;

            newPerson.Rewards.Clear();
            if (selectedRewards != null)
            {
                //получаем выбранные rewards
                foreach (var c in db.Rewards.Where(co => selectedRewards.Contains(co.Id)))
                {
                    newPerson.Rewards.Add(c);
                }
            }

            db.Entry(newPerson).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Persons/Delete/1
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
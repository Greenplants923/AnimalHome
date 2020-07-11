using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWeiboProject202005061120.Models;

namespace MyWeiboProject202005061120.Controllers
{
    public class UsersController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: Users
        public ActionResult Index()
        {
            var usersSet = db.UsersSet.Include(u => u.Animals);
            return View(usersSet.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.AnimalsId = new SelectList(db.AnimalsSet, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Email,UserName,Sex,PhoneNumber,Location,AboutMe,Photo,AnimalsId")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.UsersSet.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalsId = new SelectList(db.AnimalsSet, "Id", "Name", users.AnimalsId);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalsId = new SelectList(db.AnimalsSet, "Id", "Name", users.AnimalsId);
            return View(users);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Email,UserName,Sex,PhoneNumber,Location,AboutMe,Photo,AnimalsId")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnimalsId = new SelectList(db.AnimalsSet, "Id", "Name", users.AnimalsId);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.UsersSet.Find(id);
            db.UsersSet.Remove(users);
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

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
    public class SpecialFollowersController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: SpecialFollowers
        public ActionResult Index()
        {
            var specialFollowerSet = db.SpecialFollowerSet.Include(s => s.Users);
            return View(specialFollowerSet.ToList());
        }

        // GET: SpecialFollowers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollower specialFollower = db.SpecialFollowerSet.Find(id);
            if (specialFollower == null)
            {
                return HttpNotFound();
            }
            return View(specialFollower);
        }

        // GET: SpecialFollowers/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // POST: SpecialFollowers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,UsersId")] SpecialFollower specialFollower)
        {
            if (ModelState.IsValid)
            {
                db.SpecialFollowerSet.Add(specialFollower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollower.UsersId);
            return View(specialFollower);
        }

        // GET: SpecialFollowers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollower specialFollower = db.SpecialFollowerSet.Find(id);
            if (specialFollower == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollower.UsersId);
            return View(specialFollower);
        }

        // POST: SpecialFollowers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,UsersId")] SpecialFollower specialFollower)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialFollower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollower.UsersId);
            return View(specialFollower);
        }

        // GET: SpecialFollowers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollower specialFollower = db.SpecialFollowerSet.Find(id);
            if (specialFollower == null)
            {
                return HttpNotFound();
            }
            return View(specialFollower);
        }

        // POST: SpecialFollowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecialFollower specialFollower = db.SpecialFollowerSet.Find(id);
            db.SpecialFollowerSet.Remove(specialFollower);
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

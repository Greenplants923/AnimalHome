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
    public class FollowersController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: Followers
        public ActionResult Index()
        {
            var followerSet = db.FollowerSet.Include(f => f.Users);
            return View(followerSet.ToList());
        }

        // GET: Followers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.FollowerSet.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            return View(follower);
        }

        // GET: Followers/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // POST: Followers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,UsersId")] Follower follower)
        {
            if (ModelState.IsValid)
            {
                db.FollowerSet.Add(follower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", follower.UsersId);
            return View(follower);
        }

        // GET: Followers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.FollowerSet.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", follower.UsersId);
            return View(follower);
        }

        // POST: Followers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,UsersId")] Follower follower)
        {
            if (ModelState.IsValid)
            {
                db.Entry(follower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", follower.UsersId);
            return View(follower);
        }

        // GET: Followers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.FollowerSet.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            return View(follower);
        }

        // POST: Followers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Follower follower = db.FollowerSet.Find(id);
            db.FollowerSet.Remove(follower);
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

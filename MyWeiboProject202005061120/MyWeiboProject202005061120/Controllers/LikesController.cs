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
    public class LikesController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: Likes
        public ActionResult Index()
        {
            var likesSet = db.LikesSet.Include(l => l.Posts).Include(l => l.Users);
            return View(likesSet.ToList());
        }

        // GET: Likes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Likes likes = db.LikesSet.Find(id);
            if (likes == null)
            {
                return HttpNotFound();
            }
            return View(likes);
        }

        // GET: Likes/Create
        public ActionResult Create()
        {
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // POST: Likes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,PostsId,UsersId")] Likes likes)
        {
            if (ModelState.IsValid)
            {
                db.LikesSet.Add(likes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", likes.PostsId);
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", likes.UsersId);
            return View(likes);
        }

        // GET: Likes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Likes likes = db.LikesSet.Find(id);
            if (likes == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", likes.PostsId);
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", likes.UsersId);
            return View(likes);
        }

        // POST: Likes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,PostsId,UsersId")] Likes likes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(likes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", likes.PostsId);
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", likes.UsersId);
            return View(likes);
        }

        // GET: Likes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Likes likes = db.LikesSet.Find(id);
            if (likes == null)
            {
                return HttpNotFound();
            }
            return View(likes);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Likes likes = db.LikesSet.Find(id);
            db.LikesSet.Remove(likes);
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

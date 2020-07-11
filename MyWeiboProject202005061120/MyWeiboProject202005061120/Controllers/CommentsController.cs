using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeiboProject202005061120.Models;

namespace MyWeiboProject202005061120.Controllers
{
    public class CommentsController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: Comments
        public ActionResult Index(int? id)
        {
            //var userId = User.Identity.GetUserId();
            //Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            //var userid = userI.Id;
            //var commentsSet = db.CommentsSet.Include(c => c.Users).Include(c => c.Posts);
            var commentsSet = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";");
            if (commentsSet == null)
            {
                return HttpNotFound();
            }
            return View(commentsSet.ToList());
        }



        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.CommentsSet.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return View();
        }

        // POST: Comments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Body,PostsId")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
                var userid = userI.Id;
                //Id,TimeStamp,UsersId,PostsId
                comments.TimeStamp = DateTime.Now;
                comments.UsersId = userid;
                //comments.PostsId =;
                db.CommentsSet.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", comments.UsersId);
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", comments.PostsId);
            return View(comments);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.CommentsSet.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", comments.UsersId);
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", comments.PostsId);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,UsersId,PostsId")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", comments.UsersId);
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", comments.PostsId);
            return View(comments);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.CommentsSet.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.CommentsSet.Find(id);
            db.CommentsSet.Remove(comments);
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

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
    public class FollowedsController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: Followeds
        public ActionResult Index()
        {
            var followedSet = db.FollowedSet.Include(f => f.Users);
            return View(followedSet.ToList());
        }

        // GET: Followeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followed followed = db.FollowedSet.Find(id);
            if (followed == null)
            {
                return HttpNotFound();
            }
            return View(followed);
        }

        // GET: Followeds/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // 登录用户关注其他用户
        public ActionResult CreateFollowed(int followedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();

            // 登录用户“关注的人”增加
            Followed followed = new Followed();
            followed.UsersId = userI.Id;
            followed.FollowedsId = followedid;
            followed.TimeStamp = DateTime.Now;
            db.FollowedSet.Add(followed);

            // 被关注人的“粉丝”增加
            Follower follower = new Follower();
            follower.UsersId = followedid;
            follower.FollowersId = userI.Id;
            follower.TimeStamp = DateTime.Now;
            db.FollowerSet.Add(follower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("Manage/ShowBasicInformation", new { id = followedid });
        }

        // POST: Followeds/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,UsersId")] Followed followed)
        {
            if (ModelState.IsValid)
            {
                db.FollowedSet.Add(followed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", followed.UsersId);
            return View(followed);
        }

        // GET: Followeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followed followed = db.FollowedSet.Find(id);
            if (followed == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", followed.UsersId);
            return View(followed);
        }

        // POST: Followeds/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,UsersId")] Followed followed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(followed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", followed.UsersId);
            return View(followed);
        }

        // GET: Followeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followed followed = db.FollowedSet.Find(id);
            if (followed == null)
            {
                return HttpNotFound();
            }
            return View(followed);
        }

        // POST: Followeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Followed followed = db.FollowedSet.Find(id);
            db.FollowedSet.Remove(followed);
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

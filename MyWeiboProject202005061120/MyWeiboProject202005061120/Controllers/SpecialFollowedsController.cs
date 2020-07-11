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
    public class SpecialFollowedsController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // GET: SpecialFolloweds
        public ActionResult Index()
        {
            var specialFollowedSet = db.SpecialFollowedSet.Include(s => s.Users);
            return View(specialFollowedSet.ToList());
        }

        // GET: SpecialFolloweds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollowed specialFollowed = db.SpecialFollowedSet.Find(id);
            if (specialFollowed == null)
            {
                return HttpNotFound();
            }
            return View(specialFollowed);
        }

        // GET: SpecialFolloweds/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // POST: SpecialFolloweds/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,UsersId")] SpecialFollowed specialFollowed)
        {
            if (ModelState.IsValid)
            {
                db.SpecialFollowedSet.Add(specialFollowed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollowed.UsersId);
            return View(specialFollowed);
        }

        // GET: SpecialFolloweds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollowed specialFollowed = db.SpecialFollowedSet.Find(id);
            if (specialFollowed == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollowed.UsersId);
            return View(specialFollowed);
        }

        // POST: SpecialFolloweds/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,UsersId")] SpecialFollowed specialFollowed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialFollowed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", specialFollowed.UsersId);
            return View(specialFollowed);
        }

        // GET: SpecialFolloweds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialFollowed specialFollowed = db.SpecialFollowedSet.Find(id);
            if (specialFollowed == null)
            {
                return HttpNotFound();
            }
            return View(specialFollowed);
        }

        // POST: SpecialFolloweds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecialFollowed specialFollowed = db.SpecialFollowedSet.Find(id);
            db.SpecialFollowedSet.Remove(specialFollowed);
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

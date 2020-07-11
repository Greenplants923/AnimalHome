using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeiboProject202005061120.Models;
using PagedList;

namespace MyWeiboProject202005061120.Controllers
{
    public class PostsController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();

        // 返回上一页
        public ActionResult ReturnByAjax(HttpPostedFileBase uploadPhoto)
        {
            string photo = "";
            uploadPhoto = Request.Files[0];

            if (uploadPhoto == null)
            {
                photo = "uploadPhoto is null";
                return Content(photo);
            }

            if (uploadPhoto.ContentLength <= 0)
            {
                photo = "context.Request.Files.Count <= 0";
                return Content(photo);
            }

            // 上传文件代码 记得要先新建一下Upload文件夹
            var fileName = Path.Combine(Request.MapPath("~/myUploads/"), Path.GetFileName(uploadPhoto.FileName));
            try
            {
                uploadPhoto.SaveAs(fileName);
                return Content(uploadPhoto.FileName);
            }
            catch
            {
                photo = "上传失败！";
                return Content(photo);
            }
        }


        // GET: Posts
        public ActionResult Index(int page = 1)
        {
            int pageSize = 6;  // 每页显示10条数据
            var allpostsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(allpostsSet);
        }

        // GET: 热门动态
        public ActionResult HotPosts(int page=1)
        {
            int pageSize = 6;  // 每页显示10条数据
            var postsSet = db.PostsSet.SqlQuery("SELECT top 20 * FROM PostsSet ORDER BY Score DESC, TimeStamp DESC;").ToPagedList(page, pageSize);            
            //将分页处理后的列表传给View
            return View(postsSet);
            //return View(postsSet.ToList());
        }

        // GET: 关注的人的动态
        public ActionResult FollowedPosts(int page = 1)
        {
            int pageSize = 6;  // 每页显示10条数据
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            int userid = userI.Id;
            var postsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet WHERE EXISTS( SELECT FollowedsId FROM FollowedSet WHERE UsersId = " + userid + "AND FollowedSet.FollowedsId= PostsSet.UsersId) ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(postsSet);
        }

        // GET: 特别关注的人的动态
        public ActionResult SPFollowedPosts(int page = 1)
        {
            int pageSize = 6;  // 每页显示10条数据
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            int userid = userI.Id;
            var postsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet WHERE EXISTS( SELECT SPFollowedsId FROM SpecialFollowedSet WHERE UsersId = " + userid + "AND SpecialFollowedSet.SPFollowedsId= PostsSet.UsersId) ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(postsSet);
        }

        private static String mySearchValue = "";
        // POST: 搜索结果页面
        public ActionResult SearchResult(string searchValue, int page = 1)
        {
            int pageSize = 6;  // 每页显示10条数据
            mySearchValue = searchValue;
            var result = from m in db.PostsSet select m;
            var postsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet where Body like '%" + mySearchValue + "%' or exists(select Id from UsersSet where UserName like '%" + mySearchValue + "%' and UsersSet.Id= PostsSet.UsersId) ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(postsSet);
        }

        // GET: 搜索页面
        public ActionResult Search(int page = 1)
        {
            int pageSize = 6;  // 每页显示10条数据
            var postsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(postsSet);
        }



        // 展示用户的全部动态
        public ActionResult ShowUsersPosts(string id, int page=1)
        {
            int pageSize = 6;  // 每页显示10条数据
            Users userI = db.UsersSet.Where(useri => useri.UserId == id).AsNoTracking().FirstOrDefault();
            var userid = userI.Id;
            var postsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet WHERE UsersId=" + userid + " ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(postsSet);
        }

        // 展示动态的全部评论
        // GET: Comments
        public ActionResult ShowPostComments(int id,int page=1)
        {
            int pageSize = 10;  // 每页显示10条数据
            var commentsSet = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + "ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            if (commentsSet == null)
            {
                return HttpNotFound();
            }
            Session["PostsId"] = id;
            return View(commentsSet);
        }

        // 写评论
        // GET: Posts/CreateComment
        public ActionResult CreateComment(int id)
        {
            Session["PostsId"] = id;
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return View();
        }

        // 写评论
        // POST: Posts/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        //int id,
        public ActionResult CreateComment([Bind(Include = "Body")] Comments comment)
        {
            int PostId = Convert.ToInt32(Session["PostsId"]); 
            //Comments comment = new Comments();
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
                comment.TimeStamp = DateTime.Now;
                //comment.Body = GetCommentBody();
                comment.PostsId = PostId;
                comment.UsersId = userI.Id;
                db.CommentsSet.Add(comment);

                // 动态的评论数加1  // 更改动态的评分
                Posts post = db.PostsSet.Where(posti => posti.Id == PostId).AsNoTracking().FirstOrDefault();
                post.CommentsNumber = post.CommentsNumber + 1;
                long liken = post.LikesNumber;
                long commentn = post.CommentsNumber;
                double score = liken + commentn;
                post.Score = score;
                db.Entry(post).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("ShowPostComments", new { id = comment.PostsId });
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", comment.UsersId);
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body", comment.PostsId);
            return View(comment);
        }

        // 点赞动态
        // GET: Posts/CreateLike
        public ActionResult CreateLike(int id)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            int PostId = id;
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();

            Likes like = new Likes();
            like.UsersId = userI.Id;
            like.PostsId = PostId;
            like.TimeStamp = DateTime.Now;
            db.LikesSet.Add(like);

            // 动态的点赞数加1   // 更改动态的评分
            Posts post = db.PostsSet.Where(posti => posti.Id == PostId).AsNoTracking().FirstOrDefault();
            post.LikesNumber = post.LikesNumber + 1;
            long liken = post.LikesNumber;
            long commentn = post.CommentsNumber;
            double score = liken + commentn;
            post.Score = score;
            db.Entry(post).State = EntityState.Modified;

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            //Response.Redirect("Home/Index");
            //Response.Redirect(Request.ServerVariables("HTTP_REFERER"));
            //Response.Write("<script language=javascript>history.go(-3);</script>");
            return RedirectToAction("Details", new { id = post.Id });
        }

        // 普通用户发布动态
        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // 普通用户发布动态
        // POST: Posts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Body")] Posts posts)
        {
            //Id,Body,Photo,TimeStamp,LikesNumber,CommentsNumber,Score,UsersId
            //if (ModelState.IsValid)
            //{
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).FirstOrDefault();
                posts.Photo = "";
                posts.TimeStamp = DateTime.Now;
                posts.LikesNumber = 0;
                posts.CommentsNumber = 0;
                posts.Score = 0.0;
                posts.UsersId = userI.Id;
                db.PostsSet.Add(posts);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("ShowUsersPosts", new { id = posts.Users.UserId });
            //}

            //ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            //return View(posts);
        }

        // Vip用户发布动态
        // GET: Posts/CreateVip
        public ActionResult CreateVip()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // Vip用户发布动态
        // POST: Posts/CreateVip
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVip([Bind(Include = "Body,Photo")] Posts posts)
        {
            //Id,TimeStamp,LikesNumber,CommentsNumber,Score,UsersId
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).FirstOrDefault();
                //if((posts.Body == "" || posts.Body == null) && posts.Photo != null)
                //{
                //    posts.Body = "分享图片";
                //}
                posts.TimeStamp = DateTime.Now;
                posts.LikesNumber = 0;
                posts.CommentsNumber = 0;
                posts.Score = 0.0;
                posts.UsersId = userI.Id;
                db.PostsSet.Add(posts);
                db.SaveChanges();
                return RedirectToAction("ShowUsersPosts", new { id = userI.UserId });
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // 广告商发布广告
        // GET: Posts/CreateVip
        public ActionResult CreateAdv()
        {
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email");
            return View();
        }

        // Vip用户发布动态
        // POST: Posts/CreateVip
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdv([Bind(Include = "Body,Photo")] Posts posts)
        {
            //Id,TimeStamp,LikesNumber,CommentsNumber,Score,UsersId
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).FirstOrDefault();
                Random ran = new Random();
                int randLikes = ran.Next(100, 999);
                int randComments = ran.Next(100, 999);
                //posts.Photo = "";
                posts.TimeStamp = DateTime.Now;
                posts.LikesNumber = randLikes;
                posts.CommentsNumber = randComments;
                posts.Score = posts.LikesNumber + posts.CommentsNumber;
                posts.UsersId = userI.Id;
                db.PostsSet.Add(posts);
                db.SaveChanges();
                return RedirectToAction("ShowUsersPosts", new { id = userI.UserId });
            }

            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // 展示动态详情
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Posts postcomments = new Posts(2);
            Posts posts = db.PostsSet.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            Session["PostsId"] = posts.Id;
            return View(posts);
        }

        // Vip用户编辑动态
        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            Session["PostId"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.PostsSet.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // Vip用户编辑动态
        // POST: Posts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Body,Photo")] Posts posts)
        {
            //Id,TimeStamp,LikesNumber,CommentsNumber,Score,UsersId
            if (ModelState.IsValid)
            {
                int PostId = Convert.ToInt32(Session["PostId"]);
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
                var userid = userI.Id;
                Posts postI = db.PostsSet.Where(posti => posti.Id == PostId).AsNoTracking().FirstOrDefault(); //posti.UsersId == userid && 
                if (posts.Photo == null || posts.Photo == "")
                    posts.Photo = postI.Photo;
                posts.Id = postI.Id;
                posts.TimeStamp = postI.TimeStamp;
                posts.LikesNumber = postI.LikesNumber;
                posts.CommentsNumber = postI.CommentsNumber;
                posts.Score = postI.Score;
                posts.UsersId = postI.UsersId;
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowUsersPosts", new { id = userId });
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // 删除动态
        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.PostsSet.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // 删除动态
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.PostsSet.Find(id);
            var userid = posts.Users.UserId;

            // 删除动态时，同时删除动态的评论和点赞
            Comments comments;
            comments = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";").FirstOrDefault();
            if (comments != null)
            {
                do
                {
                    db.CommentsSet.Remove(comments);
                    db.SaveChanges();
                    comments = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";").FirstOrDefault();
                } while (comments != null);
            }
            Likes likes;
            likes = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE PostsId=" + id + ";").FirstOrDefault();
            if (likes != null)
            {
                do
                {
                    db.LikesSet.Remove(likes);
                    db.SaveChanges();
                    likes = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE PostsId=" + id + ";").FirstOrDefault();
                } while (likes != null);
            }

            db.PostsSet.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("ShowUsersPosts", new { id = userid });
        }

        // 管理员删除动态
        // GET: Posts/Delete/5
        public ActionResult DeleteAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.PostsSet.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // 管理员删除动态
        // POST: Posts/Delete/5
        [HttpPost, ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAdminConfirmed(int id)
        {
            Posts posts = db.PostsSet.Find(id);
            var userid = posts.Users.UserId;

            // 删除动态时，同时删除动态的评论和点赞
            Comments comments;
            comments = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";").FirstOrDefault();
            if (comments != null)
            {
                do
                {
                    db.CommentsSet.Remove(comments);
                    db.SaveChanges();
                    comments = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";").FirstOrDefault();
                } while (comments != null);
            }
            Likes likes;
            likes = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE PostsId=" + id + ";").FirstOrDefault();
            if (likes != null)
            {
                do
                {
                    db.LikesSet.Remove(likes);
                    db.SaveChanges();
                    likes = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE PostsId=" + id + ";").FirstOrDefault();
                } while (likes != null);
            }

            db.PostsSet.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // 删除某条动态的某条评论
        // GET: Posts/DeleteComment/5
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = db.CommentsSet.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // 删除某条动态的某条评论
        // POST: Posts/DeleteComment/5
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            Comments comment = db.CommentsSet.Find(id);
            db.CommentsSet.Remove(comment);

            int PostId = Convert.ToInt32(Session["PostsId"]);
            // 动态的评论数减1  // 更新动态的评分
            Posts post = db.PostsSet.Where(posti => posti.Id == PostId).AsNoTracking().FirstOrDefault();
            post.CommentsNumber = post.CommentsNumber - 1;
            long liken = post.LikesNumber;
            long commentn = post.CommentsNumber;
            double score = liken + commentn;
            post.Score = score;

            db.Entry(post).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("ShowPostComments", new { id = comment.PostsId });
        }

        // 取消动态点赞
        // GET: Posts/DeleteLike
        public ActionResult DeleteLike(int id)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            int PostId = id;
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();

            var like = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE UsersId=" + userI.Id + "AND PostsId=" + PostId + ";").FirstOrDefault();
            db.LikesSet.Remove(like);

            // 动态的点赞数减1   // 更改动态的评分
            Posts post = db.PostsSet.Where(posti => posti.Id == PostId).AsNoTracking().FirstOrDefault();
            post.LikesNumber = post.LikesNumber - 1;
            long liken = post.LikesNumber;
            long commentn = post.CommentsNumber;
            double score = liken + commentn;
            post.Score = score;
            db.Entry(post).State = EntityState.Modified;

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("Details", new { id = PostId });
        }

        public ActionResult UploadImgsByAjax(HttpPostedFileBase uploadPhoto)
        {
            string photo = "";
            uploadPhoto = Request.Files[0];

            if (uploadPhoto == null)
            {
                photo = "uploadPhoto is null";
                return Content(photo);
            }

            if (uploadPhoto.ContentLength <= 0)
            {
                photo = "context.Request.Files.Count <= 0";
                return Content(photo);
            }

            // 上传文件代码 记得要先新建一下Upload文件夹
            var fileName = Path.Combine(Request.MapPath("~/myUploads/"), Path.GetFileName(uploadPhoto.FileName));
            try
            {
                uploadPhoto.SaveAs(fileName);
                return Content(uploadPhoto.FileName);
            }
            catch
            {
                photo = "上传失败！";
                return Content(photo);
            }
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

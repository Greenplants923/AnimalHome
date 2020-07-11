using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeiboProject202005061120.Models;
using PagedList;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {

        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();



        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            // 设置用户初始角色（User）
            var userid = User.Identity.GetUserId();
            string uid = "e1d60597-3a41-4eb2-9b47-f775cbcf03cf";
            // 连接数据库
            SqlConnection conn = new SqlConnection();
            string connectionString = "server=127.0.0.1.;database=myWeiboDB202005061120;uid=sa; pwd=xxq300446";
            conn.ConnectionString = connectionString;
            conn.Open();
            // 查询数据库
            string sqlStr1 = "SELECT * FROM AspNetUserRoles WHERE UserId='" + userid + "' ;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStr1;
            SqlDataReader dr = cmd.ExecuteReader();//执行SQL语句

            //DataTable dt = new DataTable();
            //SqlDataAdapter msda;
            //msda = new SqlDataAdapter(cmd);
            //msda.Fill(dt);
            if (dr.HasRows == false && userid != null)
            {
                dr.Close();//关闭执行
                // 更新数据库
                string sqlStr2 = "INSERT INTO AspNetUserRoles (UserId,RoleId) VALUES('" + userid + "' ,'" + uid + "');";
                cmd.CommandText = sqlStr2;
                int iud2 = 0;
                iud2 = cmd.ExecuteNonQuery();
            }
            else
            {
                dr.Close();//关闭执行
            }
            // 关闭数据库连接
            conn.Close();

            int pageSize = 6;  // 每页显示10条数据
            var allpostsSet = db.PostsSet.SqlQuery("SELECT * FROM PostsSet ORDER BY TimeStamp DESC;").ToPagedList(page, pageSize);
            return View(allpostsSet);
            //return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //
        //GET: Home/ShowBasicInformation
        public ActionResult ShowBasicInformation(string id)
        {
            //var userId = User.Identity.GetUserId();
            Users userInfo = db.UsersSet.FirstOrDefault(useri => String.Equals(useri.UserId, id));
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // POST: Home/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Body,Photo")] Posts posts)
        {
            //Id,TimeStamp,LikesNumber,CommentsNumber,Score,UsersId
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
                var userid = userI.Id;
                Posts postI = db.PostsSet.Where(posti => posti.UsersId == userid).AsNoTracking().FirstOrDefault();
                posts.Id = postI.Id;
                posts.TimeStamp = postI.TimeStamp;
                posts.LikesNumber = postI.LikesNumber;
                posts.CommentsNumber = postI.CommentsNumber;
                posts.Score = postI.Score;
                posts.UsersId = postI.UsersId;
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowMyPosts");
            }
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "Email", posts.UsersId);
            return View(posts);
        }

        // GET: Home/Details/5
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


        // GET: Home/Delete/5
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

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.PostsSet.Find(id);
            db.PostsSet.Remove(posts);

            // 删除动态时，同时删除动态的点赞和评论
            Likes likes;
            do
            {
                likes = db.LikesSet.SqlQuery("SELECT * FROM LikesSet WHERE PostsId=" + id + ";").FirstOrDefault();
                db.LikesSet.Remove(likes);
            } while (likes != null);
            Comments comments;
            do
            {
                comments = db.CommentsSet.SqlQuery("SELECT * FROM CommentsSet WHERE PostsId=" + id + ";").FirstOrDefault();
                db.CommentsSet.Remove(comments);
            } while (comments != null);

            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}

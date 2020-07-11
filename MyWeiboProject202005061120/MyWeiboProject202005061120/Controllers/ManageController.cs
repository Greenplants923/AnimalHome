using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyWeiboProject202005061120.Models;
using System.Data.Entity;
using System.Net;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using PagedList;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private myWeiboDB202005061120Entities db = new myWeiboDB202005061120Entities();
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // 充值成为会员
        public ActionResult ToBeVip(string userid)
        {
            //Boolean paysuccess = false;
            string vipid = "07a9ffb2-0ecb-44db-9f70-c8692c1d3bbf";
            //string uid = "e1d60597-3a41-4eb2-9b47-f775cbcf03cf";
            // 连接数据库
            SqlConnection conn = new SqlConnection();
            string connectionString = "server=127.0.0.1.;database=myWeiboDB202005061120;uid=sa; pwd=xxq300446";
            conn.ConnectionString = connectionString;
            conn.Open();
            // 更新数据库
            string sqlStr = "UPDATE AspNetUserRoles SET RoleId = '" + vipid + "' WHERE UserId = '" + userid + "';";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStr;
            int iud = 0;
            iud = cmd.ExecuteNonQuery();
            // 关闭数据库连接
            conn.Close();
            Users userI = db.UsersSet.Where(ui => ui.UserId == userid).FirstOrDefault();
            userI.Vip = true;
            db.Entry(userI).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ShowBasicInformation", new { id = userid });
        }


        // 列出用户所有粉丝
        // GET: /Manage/ShowMyFollowers
        public ActionResult ShowFollowers(string id, int page = 1)
        {
            int pageSize = 10;  // 每页显示10条数据
            Users userI = db.UsersSet.Where(useri => useri.UserId == id).AsNoTracking().FirstOrDefault();
            var userid = userI.Id;
            var followersSet = db.FollowerSet.SqlQuery("SELECT * FROM FollowerSet WHERE UsersId=" + userid + ";").ToPagedList(page, pageSize);
            return View(followersSet);
        }

        // 列出用户关注的所有人
        // GET: /Manage/ShowMyFollowers
        public ActionResult ShowFolloweds(string id, int page = 1)
        {
            int pageSize = 10;  // 每页显示10条数据
            Users userI = db.UsersSet.Where(useri => useri.UserId == id).AsNoTracking().FirstOrDefault();
            var userid = userI.Id;
            var followedsSet = db.FollowedSet.SqlQuery("SELECT * FROM FollowedSet WHERE UsersId=" + userid + ";").ToPagedList(page, pageSize);
            return View(followedsSet);
        }

        // 登录用户在“我的关注”页面取消关注
        public ActionResult DeleteMyFolloweds(int followedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == followedid).AsNoTracking().FirstOrDefault();

            // 登录用户“关注的人”减少
            Followed followed = db.FollowedSet.Where(followedi => followedi.FollowedsId == followedid && followedi.UsersId == userI.Id).FirstOrDefault();
            db.FollowedSet.Remove(followed);

            // 被关注人的“粉丝”减少
            Follower follower = db.FollowerSet.Where(followeri => followeri.UsersId == followedid && followeri.FollowersId == userI.Id).FirstOrDefault();
            db.FollowerSet.Remove(follower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowFolloweds", new { id = User.Identity.GetUserId() });
        }

        // Vip用户在“我的关注”页面进行特别关注
        public ActionResult CreateMySPFolloweds(int spfollowedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == spfollowedid).AsNoTracking().FirstOrDefault();

            // 登录用户“关注的人”增加
            SpecialFollowed spfollowed = new SpecialFollowed();
            spfollowed.UsersId = userI.Id;
            spfollowed.SPFollowedsId = spfollowedid;
            spfollowed.TimeStamp = DateTime.Now;
            //followed.Users = userI;
            db.SpecialFollowedSet.Add(spfollowed);
            db.SaveChanges();

            // 被关注人的“粉丝”增加
            SpecialFollower spfollower = new SpecialFollower();
            spfollower.UsersId = spfollowedid;
            spfollower.SPFollowersId = userI.Id;
            spfollower.TimeStamp = DateTime.Now;
            //follower.Users = userII;
            db.SpecialFollowerSet.Add(spfollower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowFolloweds", new { id = User.Identity.GetUserId() });
        }


        // Vip用户在“我的关注”页面取消特别关注
        // GET: /Manage/DeleteSPFolloweds
        public ActionResult DeleteMySPFolloweds(int spfollowedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == spfollowedid).AsNoTracking().FirstOrDefault();

            // 登录用户“特别关注的人”减少
            SpecialFollowed spfollowed = db.SpecialFollowedSet.Where(followedi => followedi.SPFollowedsId == spfollowedid && followedi.UsersId == userI.Id).FirstOrDefault();
            db.SpecialFollowedSet.Remove(spfollowed);

            // 被特别关注人的“粉丝”减少
            SpecialFollower spfollower = db.SpecialFollowerSet.Where(followeri => followeri.UsersId == spfollowedid && followeri.SPFollowersId == userI.Id).FirstOrDefault();
            db.SpecialFollowerSet.Remove(spfollower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowFolloweds", new { id = User.Identity.GetUserId() });
        }


        // 登录用户在“我的粉丝”界面移除粉丝
        public ActionResult DeleteMyFollowers(int followerid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == followerid).AsNoTracking().FirstOrDefault();

            // 登录用户“粉丝”减少
            Follower follower = db.FollowerSet.Where(followeri => followeri.UsersId == userI.Id && followeri.FollowersId == followerid).FirstOrDefault();
            db.FollowerSet.Remove(follower);

            // 关注人的“关注”减少
            Followed followed = db.FollowedSet.Where(followedi => followedi.FollowedsId == userI.Id && followedi.UsersId == followerid).FirstOrDefault();
            db.FollowedSet.Remove(followed);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowFollowers", new { id = User.Identity.GetUserId() });
        }

        //列出用户的基本信息
        //GET: /Account/ShowBasicInformation
        public ActionResult ShowBasicInformation(string id)
        {
            Users userInfo = db.UsersSet.SqlQuery("SELECT * FROM UsersSet WHERE UserId='" + id + "';").FirstOrDefault();
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // 上传图片
        public ActionResult UploadImgsByAjax(HttpPostedFileBase uploadPhoto)
        {
            string photo = "";
            uploadPhoto = Request.Files[0];

            if (uploadPhoto == null)
            {
                photo = "uploadPhoto is null";
                return Content(photo);
            }

            if(uploadPhoto.ContentLength <= 0)
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

        //修改用户基本信息
        // GET: Users/Edit/5
        public ActionResult EditBasicInformation(int? id)
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

        //修改用户基本信息
        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBasicInformation([Bind(Include = "Photo,Email,UserName,AnimalsId,Sex,Location,AboutMe")] Users users)
        {
            //if (ModelState.IsValid)
            //{
                var userId = User.Identity.GetUserId();
                Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault(); //增加一个不再追踪
                Animals animal = db.AnimalsSet.Where(ai => ai.Id == users.AnimalsId).FirstOrDefault();
                if((users.Photo == "" || users.Photo == null) && users.Photo != animal.DefaultPhoto)
                {
                    users.Photo = animal.DefaultPhoto;
                }
                users.UserId = userI.UserId;
                users.TimeStamp = userI.TimeStamp;
                users.PhoneNumber = userI.PhoneNumber;
                users.Vip = userI.Vip;
                users.Id = userI.Id;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowBasicInformation", new { id = userI.UserId });
            //}
            //ViewBag.AnimalsId = new SelectList(db.AnimalsSet, "Id", "Name", users.AnimalsId);
            //return View(users);
        }

        // 登录用户关注其他用户
        public ActionResult CreateFollowed(int followedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == followedid).AsNoTracking().FirstOrDefault();

            // 登录用户“关注的人”增加
            Followed followed = new Followed();
            //followed.Id = 1;
            followed.UsersId = userI.Id;
            followed.FollowedsId = followedid;
            followed.TimeStamp = DateTime.Now;
            //followed.Users = userI;
            db.FollowedSet.Add(followed);
            db.SaveChanges();

            // 被关注人的“粉丝”增加
            Follower follower = new Follower();
            //follower.Id = 1;
            follower.UsersId = followedid;
            follower.FollowersId = userI.Id;
            follower.TimeStamp = DateTime.Now;
            //follower.Users = userII;
            db.FollowerSet.Add(follower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowBasicInformation", new { id = userII.UserId });
        }

        // 登录用户取关其他用户
        public ActionResult DeleteFollowed(int followedid)
        {
            //int PostId = Convert.ToInt32(Session["PostsId"]);
            var userId = User.Identity.GetUserId();
            Users userI = db.UsersSet.Where(useri => useri.UserId == userId).AsNoTracking().FirstOrDefault();
            Users userII = db.UsersSet.Where(useri => useri.Id == followedid).AsNoTracking().FirstOrDefault();

            // 登录用户“关注的人”减少
            Followed followed = db.FollowedSet.Where(followedi => followedi.FollowedsId == followedid && followedi.UsersId == userI.Id).FirstOrDefault();
            db.FollowedSet.Remove(followed);

            // 被关注人的“粉丝”减少
            Follower follower = db.FollowerSet.Where(followeri => followeri.UsersId == followedid && followeri.FollowersId == userI.Id).FirstOrDefault();
            db.FollowerSet.Remove(follower);

            db.SaveChanges();
            ViewBag.UsersId = new SelectList(db.UsersSet, "Id", "UserName");
            ViewBag.PostsId = new SelectList(db.PostsSet, "Id", "Body");
            return RedirectToAction("ShowBasicInformation", new { id = userII.UserId });
        }



        //
        // GET: /Account/Index
        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "密码修改成功"
                : message == ManageMessageId.SetPasswordSuccess ? "密码设置成功"
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two factor provider has been set."
                : message == ManageMessageId.Error ? "发生未知错误"
                : message == ManageMessageId.AddPhoneSuccess ? "手机号绑定成功"
                : message == ManageMessageId.RemovePhoneSuccess ? "手机号解绑成功"
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // GET: /Account/RemoveLogin
        [HttpGet]
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var userId = User.Identity.GetUserId();
            var result = await UserManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Account/AddPhoneNumber
        [HttpGet]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Account/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "安全代码: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/RememberBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RememberBrowser()
        {
            var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/ForgetBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTFA()
        {
            var userId = User.Identity.GetUserId();
            await UserManager.SetTwoFactorEnabledAsync(userId, true);
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTFA()
        {
            var userId = User.Identity.GetUserId();
            await UserManager.SetTwoFactorEnabledAsync(userId, false);
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Account/VerifyPhoneNumber
        [HttpGet]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            ViewBag.Status = "“【宠屋】验证码：【" + code + "】，您正在办理绑定手机号业务，请勿向任何人提供您收到的短信校验码”";
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Account/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var result = await UserManager.ChangePhoneNumberAsync(userId, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "手机号验证失败");
            return View(model);
        }

        //
        // GET: /Account/RemovePhoneNumber
        [HttpGet]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var userId = User.Identity.GetUserId();
            var result = await UserManager.SetPhoneNumberAsync(userId, null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var result = await UserManager.AddPasswordAsync(userId, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "删除了外部登录"
                : message == ManageMessageId.Error ? "发生未知错误"
                : "";
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(userId);
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var userId = User.Identity.GetUserId();
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, userId);
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(userId, loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}
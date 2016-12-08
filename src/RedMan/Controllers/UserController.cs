using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.DataAccess.Repository;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Services.EntitiesServices;

namespace RedMan.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;
        private readonly IdentityService _identityService;


        public UserController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._replyRepo = new Repository<Reply>(context);
            this._identityService = new IdentityService(new IdentityRepository<User>(context));
        }

        /// <summary>
        /// 用户主页
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int id)
        {
            var user = await _userRepo.FindAsync(p => p.UserId == id);
            if(user == null)
                throw new Exception("用户找不到，或者已被删除");
            //获取用户发布过的主题
            var topic_Pub = await _topicRepo.FindAllDelayAsync(p => p.Author_Id == user.UserId);
            //获取用户发布过的回复
            var reply_Pub = await _replyRepo.FindAllDelayAsync(p => p.Author_Id == user.UserId);
            var topic_Reply = GetTopicByReply(reply_Pub);
            UserViewModel userViewModel;
            if(topic_Pub!=null && topic_Reply != null)
            {
                userViewModel = new UserViewModel()
                {
                    User = user,
                    Topic_Published = topic_Pub.Distinct(),
                    Topic_Join = topic_Reply.Distinct()
                };
            }
            else
            {
                userViewModel = new UserViewModel()
                {
                    User = user
                };
            }

            return View(userViewModel);
        }

        /// <summary>
        /// 用户设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Settings()
        {
            var user = await _userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if(user == null)
                throw new Exception("用户找不到，或已被删除");
            var userSettingViewModel = new UserSettingsViewModel()
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
                Signature = user.Signature
            };
            ViewData["Error1"] = false;
            return View(userSettingViewModel);
        }

        /// <summary>
        /// 用户设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Settings(UserSettingsViewModel model)
        {
            ViewData["Error1"] = true;
            if(!ModelState.IsValid)
                return View(model);
            var loginUserName = User.Identity.Name;
            if(loginUserName == null)
                return RedirectToAction("Login","Account",new { ReturnUrl = "/User/Setting" });
           
            var user = await _userRepo.FindAsync(p => p.UserId == model.UserId);
            if(user==null)
                throw new Exception("用户找不到，或已被删除");
            if(user.Name != loginUserName)
                return RedirectToAction("Index","Home");

            var nameIsExist = await _userRepo.IsExistAsync(p => p.Name == model.Name);
            if(nameIsExist && (model.Name != user.Name))
            {
                ModelState.AddModelError("","此名称已存在");
                return View(model);
            }
            var emailIsExist = await _userRepo.IsExistAsync(p => p.Email == model.Email);
            if(emailIsExist && model.Email != user.Email)
            {
                ModelState.AddModelError("","此邮箱地址已存在");
                return View(model);
            }

            user.Name = model.Name;
            user.Email = model.Email;
            if(!string.IsNullOrEmpty(model.Avatar))
                user.Avatar = model.Avatar;
            user.Signature = model.Signature;
            var success = await _userRepo.UpdateAsync(user);
            if(success)
                return new RedirectResult(Url.Content("/User/Settings/"));
            else
            {
                ModelState.AddModelError("","保存失败，请稍后重试");
                return View(model);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["Error2"] = true;
            var user = await _userRepo.FindAsync(p => p.UserId == model.UserId);
            if(user == null)
            {
                ModelState.AddModelError("","用户不存在，或已被删除");
                return PartialView("_PartialChangePassword",model);
            }
            if(!ModelState.IsValid)
                return PartialView("_PartialChangePassword",model);
            if(user.Password != model.OldPassword)
            {
                ModelState.AddModelError("","密码不正确");
                return PartialView("_PartialChangePassword",model);
            }
            user.Password = model.Password;
            var success = await _userRepo.UpdateAsync(user);
            if(success)
            {
                ViewData["Error2"] = false;
                return Content("<script>alert('修改成功!');location.href='/Account/Login'</script>");
            }
            else
            {
                ModelState.AddModelError("","修改失败");
                return PartialView("_PartialChangePassword",new ChangePasswordViewModel());
            }
        }

        /// <summary>
        /// 获取积分排行前100名用户
        /// </summary>
        /// <returns></returns>
        [Route("/user/top100")]
        public IActionResult Top100()
        {
            return View();
        }

        /// <summary>
        /// 获取用户头像链接
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<FileContentResult> GetUserAvatarUrl(int id)
        {
            var user = await _userRepo.FindAsync(p => p.UserId == id);
            if(user == null)
                return null;
            var path = $"{Directory.GetCurrentDirectory()}/wwwroot/{user.Avatar}";
            try
            {
                using(FileStream fs = new FileStream(path,FileMode.Open))
                {
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes,0,bytes.Length);
                    return File(bytes,"application/x-png");
                }
            }
            catch(Exception)
            {
                return null;
            }
            
        }
        #region 辅助方法

        /// <summary>
        /// 根据回复列表，获取对应的话题
        /// </summary>
        /// <param name="replies">回复列表</param>
        /// <returns></returns>
        public IEnumerable<Topic> GetTopicByReply(IEnumerable<Reply> replies)
        {
            foreach(var item in replies)
            {
                yield return _topicRepo.Find(p => p.TopicId == item.Topic_Id);
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Model.Context;
using RedMan.DataAccess.IRepository;
using Web.Model.Entities;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RedMan.Controllers
{
    [Authorize]
    public class TopicController :Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Reply> _replyRepo;
        private readonly IRepository<TopicCollect> _topicCollectRepo;

        public TopicController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._replyRepo = new Repository<Reply>(context);
            this._topicCollectRepo = new Repository<TopicCollect>(context);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int id)
        {
            var topic = await _topicRepo.FindAsync(p => p.TopicId == id);
            if(topic == null)
                throw new Exception("找不到此话题");
            topic.Visit_Count += 1;
            await _topicRepo.UpdateAsync(topic);
            var author = await _userRepo.FindAsync(p => p.UserId == topic.Author_Id);
            if(author == null)
                throw new Exception("作者未找到");

            var topicViewModel = new TopicViewModel()
            {
                Tab = topic.Type,
                Title = topic.Title,
                Content = topic.Content,

                Topic = topic,
                Author = author
            };

            if(User.Claims.Any(p => p.Type == ClaimTypes.Name))
            {
                var loginUser = await _userRepo.FindAsync(p => p.Name == User.Identity.Name);
                topicViewModel.LoginUser = loginUser;
                topicViewModel.Collected = await _topicCollectRepo.IsExistAsync(p => p.TopicId == topic.TopicId && p.UserId == loginUser.UserId);
            }

            return View(topicViewModel);
        }

        /// <summary>
        /// 发布话题
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            ViewData["Error"] = false;
            ViewData["Action"] = "Add";
            return View(new TopicViewModel());
        }

        /// <summary>
        /// 发布话题
        /// </summary>
        /// <param name="model">话题</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicViewModel model)
        {
            model.Title = model.Title.Trim();
            ViewData["Error"] = true;
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var loginUserName = User.Identity.Name;
            var loginUser = await _userRepo.FindAsync(p => p.Name == loginUserName);
            if(loginUser == null)
            {
                ModelState.AddModelError("","用户找不到");
                return View(model);
            }
            var topic = new Topic()
            {
                Title = model.Title,
                Content = model.Content,
                Author_Id = loginUser.UserId,
                CreateDateTime = DateTime.Now,
                Type = model.Tab
            };
            var result = await _topicRepo.AddAsync(topic,false);
            loginUser.Topic_Count += 1;
            result = await _userRepo.UpdateAsync(loginUser);
            if(result)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("","出现未知错误，发布失败，请稍后再试");
                return View(model);
            }
        }

        /// <summary>
        /// 编辑话题
        /// </summary>
        /// <param name="topidId">话题编号</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Error"] = false;
            var topic = await _topicRepo.FindAsync(p => p.TopicId == id);
            if(topic == null)
                throw new Exception("此话题未找到，或已被删除");
            var loginUserName = User.Identity.Name;
            var loginUser = await _userRepo.FindAsync(p => p.Name == loginUserName);

            var topicViewModel = new TopicViewModel()
            {
                Author = loginUser,
                Content = topic.Content,
                Title = topic.Title,
                Tab = topic.Type,
                LoginUser = loginUser,
                Topic = topic,
                TopicId=topic.TopicId
            };
            return View("Add",topicViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TopicViewModel model)
        {
            model.Title = model.Title.Trim();
            ViewData["Error"] = true;
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var loginUserName = User.Identity.Name;
            var loginUser = await _userRepo.FindAsync(p => p.Name == loginUserName);
            if(loginUser == null)
            {
                ModelState.AddModelError("","用户找不到");
                return View(model);
            }
            var topicOrign = await _topicRepo.FindAsync(p => p.TopicId == model.TopicId);
            if(topicOrign == null)
                throw new Exception("找不到话题，或已被删除");
            topicOrign.Title = model.Title;
            topicOrign.Content = model.Content;
            topicOrign.Type = model.Tab;
            topicOrign.UpdateDateTime = DateTime.Now;

            var result = await _topicRepo.UpdateAsync(topicOrign);
            if(result)
            {
                return new RedirectResult(Url.Content($"/Topic/Index/{topicOrign.TopicId}"));
            }
            else
            {
                ModelState.AddModelError("","出现未知错误，编辑失败，请稍后再试");
                return View(model);
            }
        }

        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="id">话题ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var topic = await _topicRepo.FindAsync(p => p.TopicId == id);
            if(topic == null)
                return Json(new { success = false,message = "找不到此话题" });
            else
            {
                var success = await _topicRepo.DeleteAsync(topic,false);
                var user = await _userRepo.FindAsync(p => p.UserId == topic.Author_Id);
                if(user!=null)
                {
                    user.Topic_Count -= 1;
                    success = await _userRepo.UpdateAsync(user);
                }
                if(success)
                    return Json(new { success = true,message = "删除成功" });
                else
                    return Json(new { success = false,message = "删除失败" });                
            }
        }
    }
}

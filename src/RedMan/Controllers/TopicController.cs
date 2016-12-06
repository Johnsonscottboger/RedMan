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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RedMan.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Reply> _replyRepo;

        public TopicController(MyContext context) 
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._replyRepo = new Repository<Reply>(context);
        }

        [Route("[controller]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int id)
        {
            var topic = await _topicRepo.FindAsync(p => p.TopicId == id);
            if(topic == null)
                throw new Exception("找不到此话题");

            return View(topic);
        }

        public IActionResult Add() 
        {
            ViewData["Error"] = false;
            ViewData["Action"] = "Add";
            return View(new TopicViewModel());
        }

        /// <summary>
        /// 发布话题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicViewModel model) 
        {
            model.Title = model.Title.Trim();
            ViewData["Error"] = true;
            if(!ModelState.IsValid) {
                return View(model);
            }
            var loginUserName = User.Identity.Name;
            var loginUser = await _userRepo.FindAsync(p => p.Name == loginUserName);
            if(loginUser == null) {
                ModelState.AddModelError("","用户找不到");
                return View(model);
            }
            var topic = new Topic() {
                Title = model.Title,
                Content = model.Content,
                Author_Id = loginUser.UserId,
                CreateDateTime = DateTime.Now,
                Type = model.Tab
            };
            var result = await _topicRepo.AddAsync(topic);
            if(result) {
                return RedirectToAction("Index","Home");
            }else {
                ModelState.AddModelError("","出现未知错误，发布失败，请稍后再试");
                return View(model);
            }
        }
    }
}

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
using Web.Model.Context;
using Web.Model.Entities;

namespace RedMan.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;

        public UserController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._replyRepo = new Repository<Reply>(context);
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

        public async Task<IActionResult> Settings()
        {
            //TODO:用户资料设置
            return null;
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

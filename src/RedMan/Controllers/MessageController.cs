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

namespace RedMan.Controllers
{
    /// <summary>
    /// 消息管理控制器
    /// </summary>
    [Authorize]
    public class MessageController : Controller
    {
        #region - Private -
        private readonly ModelContext _context;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Message> _msgRepo;
        #endregion

        /// <summary>
        /// 初始化消息管理控制器<see cref="MessageController"/>实例
        /// </summary>
        /// <param name="context">指定的模型上下文<see cref="ModelContext"/>实例</param>
        public MessageController(ModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
            this._msgRepo = new Repository<Message>(context);
        }

        /// <summary>
        /// 消息管理首页
        /// </summary>
        /// <returns>消息管理首页</returns>
        public async Task<IActionResult> Index()
        {
            var loginUser = await this._userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if (loginUser == null)
                throw new Exception("找不到用户，或已被删除");
            var unreadMsg = await this._msgRepo.FindAllDelayAsync(p => p.ToUserId == loginUser.UserId && !p.Has_Read);
            return View(unreadMsg);
        }

        /// <summary>
        /// 阅读消息
        /// </summary>
        /// <returns>阅读消息页面</returns>
        [HttpPost]
        public async Task<IActionResult> ReadMessage()
        {
            var loginUser = await this._userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if (loginUser == null)
                throw new Exception("找不到用户，或已被删除");
            var unreadMsg = await this._msgRepo.FindAllDelayAsync(p => p.ToUserId == loginUser.UserId && p.Has_Read);
            return PartialView("_PartialReadMessage", unreadMsg);
        }

        /// <summary>
        /// 将消息标记为已读
        /// </summary>
        /// <param name="id">指定标记的消息ID</param>
        /// <returns>消息发出的话题首页</returns>
        public async Task<IActionResult> MarkRead(int id)
        {
            var msg = await this._msgRepo.FindAsync(p => p.MessageId == id);
            if (msg != null)
            {
                msg.Has_Read = true;
                await this._msgRepo.UpdateAsync(msg, false);
                var user = await this._userRepo.FindAsync(p => p.UserId == msg.ToUserId);
                if (user != null)
                {
                    user.UnreadMsg_Count -= 1;
                    await this._userRepo.UpdateAsync(user);
                }
                return new RedirectResult($"/Topic/Index/{msg.Topic_Id}#{msg.FromReplyId}");
            }
            throw new Exception("消息找不到，或已被删除");
        }
    }
}

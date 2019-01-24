using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Model.Context;
using RedMan.DataAccess.IRepository;
using Web.Model.Entities;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using Microsoft.AspNetCore.Authorization;


namespace RedMan.Controllers
{
    /// <summary>
    /// 回复管理控制器
    /// </summary>
    [Authorize]
    public class ReplyController : Controller
    {
        #region - Private -
        private readonly ModelContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Message> _msgRepo;
        #endregion

        /// <summary>
        /// 初始化回复管理控制器<see cref="ReplyController"/>实例
        /// </summary>
        /// <param name="context">指定的模型上下文<see cref="ModelContext"/>实例</param>
        public ReplyController(ModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._replyRepo = new Repository<Reply>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
            this._msgRepo = new Repository<Message>(context);
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="id">指定的话题 ID</param>
        /// <param name="replyContent">指定的回复内容</param>
        /// <returns>话题首页</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, string replyContent)
        {
            replyContent = replyContent?.Trim();
            var topic = await this._topicRepo.FindAsync(p => p.TopicId == id);
            if (topic == null)
                throw new Exception("话题找不到，或者已被删除");
            if (string.IsNullOrEmpty(replyContent))
                return new RedirectResult(Url.Content($"/Topic/Index/{id}"));
            var loginUser = await this._userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if (loginUser == null)
                return new RedirectResult(Url.Action("Login", "Account"));
            var reply = new Reply()
            {
                Content = replyContent,
                Author_Id = loginUser.UserId,
                Author_Name = loginUser.Name,
                Topic_Id = id,
                CreateDateTime = DateTime.Now
            };
            var replySuccess = await this._replyRepo.AddAsync(reply);
            topic.ReplyCount += 1;
            topic.Last_Reply_Id = reply.ReplyId;
            topic.Last_ReplyDateTime = reply.CreateDateTime;
            topic.LastReplyUserId = loginUser.UserId;
            var topicSuccess = await this._topicRepo.UpdateAsync(topic, false);
            loginUser.Reply_Count += 1;
            loginUser.Score += 1;
            topicSuccess = await this._userRepo.UpdateAsync(loginUser);
            var topicAuthor = await this._userRepo.FindAsync(p => p.UserId == topic.AuthorId);
            if (topicAuthor != null && topicAuthor.UserId != loginUser.UserId)
            {
                topicAuthor.UnreadMsg_Count += 1;
                await this._userRepo.UpdateAsync(topicAuthor);
                await AddMessage(topicAuthor, loginUser, topic, reply);
            }
            if (replySuccess && topicSuccess)
            {
                return new RedirectResult(Url.Content($"/Topic/Index/{id}#{reply.ReplyId}"));
            }
            else
            {
                ModelState.AddModelError("", "回复发布失败，请稍后重试");
                return new RedirectResult(Url.Content($"/Topic/Index/{id}"));
            }
        }

        /// <summary>
        /// 向回复添加回复
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <param name="r_content">回复内容</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToReply(int id, string r_content)
        {
            var reply = await this._replyRepo.FindAsync(p => p.ReplyId == id);
            if (reply == null)
                throw new Exception("回复找不到，或已被删除");
            var topicId = reply.Topic_Id;
            if (string.IsNullOrEmpty(r_content))
                return new RedirectResult(Url.Content($"/Topic/Index/{topicId}"));
            var topic = await this._topicRepo.FindAsync(p => p.TopicId == topicId);
            if (topic == null)
            {
                ModelState.AddModelError("", "话题找不到，或者已被删除");
                return RedirectToAction("Index", "Home");
            }
            var loginUser = await this._userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if (loginUser == null)
            {
                return new RedirectResult(Url.Action("Login", "Account"));
            }
            var replyToReply = new Reply()
            {
                Content = r_content,
                Author_Id = loginUser.UserId,
                Author_Name = loginUser.Name,
                Topic_Id = topicId,
                Reply_Id = reply.ReplyId,
                CreateDateTime = DateTime.Now,
            };
            var replySuccess = await this._replyRepo.AddAsync(replyToReply);
            topic.Last_ReplyDateTime = replyToReply.CreateDateTime;
            topic.Last_Reply_Id = replyToReply.ReplyId;
            topic.LastReplyUserId = replyToReply.Author_Id;
            topic.ReplyCount += 1;
            var topicSuccess = await this._topicRepo.UpdateAsync(topic, false);
            loginUser.Reply_Count += 1;
            loginUser.Score += 1;
            topicSuccess = await this._userRepo.UpdateAsync(loginUser);
            var replyAuthor = await this._userRepo.FindAsync(p => p.UserId == reply.Author_Id);
            if (replyAuthor != null && replyAuthor.UserId != loginUser.UserId)
            {
                replyAuthor.UnreadMsg_Count += 1;
                await this._userRepo.UpdateAsync(replyAuthor);
                await AddMessage(replyAuthor, loginUser, topic, replyToReply);
            }
            if (replySuccess && topicSuccess)
            {
                return new RedirectResult(Url.Content($"/Topic/Index/{topicId}#{replyToReply.ReplyId}"));
            }
            else
            {
                ModelState.AddModelError("", "回复失败，请稍后重试");
                return new RedirectResult(Url.Content($"/Topic/Index/{topicId}"));
            }
        }

        /// <summary>
        /// 编辑回复
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Error"] = false;
            var reply = await this._replyRepo.FindAsync(p => p.ReplyId == id);
            if (reply == null)
                throw new Exception("回复找不到，或已被删除");
            var replyViewModel = new ReplyEditViewModel()
            {
                ReplyId = reply.ReplyId,
                Content = reply.Content
            };
            return View(replyViewModel);
        }

        /// <summary>
        /// 编辑回复
        /// </summary>
        /// <param name="model">回复</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReplyEditViewModel model)
        {
            model.Content = model.Content.Trim();
            ViewData["Error"] = true;
            if (model != null)
                model.Content = model.Content.Trim();
            if (!ModelState.IsValid)
                return View(model);
            var reply = await this._replyRepo.FindAsync(p => p.ReplyId == model.ReplyId);
            if (reply == null)
                throw new Exception("回复找不到，或已被删除");
            reply.Content = model.Content;
            reply.UpdateDateTime = DateTime.Now;
            var success = await this._replyRepo.UpdateAsync(reply);
            if (success)
            {
                ViewData["Error"] = false;
                return Redirect($"/Topic/Index/{reply.Topic_Id}#{reply.ReplyId}");
            }
            else
            {
                ModelState.AddModelError("", "保存失败，请稍后重试");
                return View(model);
            }
        }

        /// <summary>
        /// 删除回复ID
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var reply = await this._replyRepo.FindAsync(p => p.ReplyId == id);
            if (reply == null)
                return Json(new { status = "fail" });
            reply.Deleted = true;
            await this._replyRepo.UpdateAsync(reply, false);
            var topic = await this._topicRepo.FindAsync(p => p.TopicId == reply.Topic_Id);
            topic.ReplyCount -= 1;
            var topicSuccess = await this._topicRepo.UpdateAsync(topic);
            if (topicSuccess)
                return Json(new { status = "success" });
            else
                return Json(new { status = "fail" });
        }

        #region - Private Method -
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="to_userid">接收用户ID</param>
        /// <param name="from_userid">发送用户ID</param>
        /// <param name="fromReply">来源回复</param>
        /// <param name="topic">目标话题</param>
        /// <param name="reply">目标回复</param>
        /// <returns></returns>
        private async Task<bool> AddMessage(User to_user, User from_user, Topic fromTopic, Reply newReply)
        {
            var message = new Message()
            {
                ToUserId = to_user.UserId,
                FromUserId = from_user.UserId,
                FromUserName = from_user.Name,
                Topic_Id = fromTopic.TopicId,
                Tilte = fromTopic.Title,

                FromReplyId = newReply.ReplyId,
                Content = "回复了",
                CreateDateTime = DateTime.Now
            };

            return await this._msgRepo.AddAsync(message);
        }
        #endregion
    }
}

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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RedMan.Controllers
{
    [Authorize]
    public class ReplyController :Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;
        private readonly IRepository<User> _userRepo;
        public ReplyController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._replyRepo = new Repository<Reply>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
        }

        
        public async Task<IActionResult> Index(Int64 topicId)
        {
            return View();
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="id">话题ID</param>
        /// <param name="r_content">回复内容</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id,string r_content)
        {
            var topic = await _topicRepo.FindAsync(p => p.TopicId == id);
            if(topic == null)
                throw new Exception("话题找不到，或者已被删除");
            if(string.IsNullOrEmpty(r_content))
            {
                ModelState.AddModelError("","请输入回复内容");
                return new RedirectResult(Url.Content($"/Topic/Index/{id}"));
            }
            var loginUser = await _userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if(loginUser == null)
            {
                return new RedirectResult(Url.Action("Login","Account"));
            }
            var reply = new Reply()
            {
                Content = r_content,
                Author_Id = loginUser.UserId,
                Author_Name = loginUser.Name,
                Topic_Id = id,
                CreateDateTime = DateTime.Now
            };
            var replySuccess = await _replyRepo.AddAsync(reply);
            topic.Reply_Count += 1;
            topic.Last_Reply_Id = reply.ReplyId;
            topic.Last_ReplyDateTime = reply.CreateDateTime;
            topic.Last_Reply_UserId = loginUser.UserId;
            var topicSuccess = await _topicRepo.UpdateAsync(topic,false);
            loginUser.Reply_Count += 1;
            topicSuccess = await _userRepo.UpdateAsync(loginUser);
            if(replySuccess&&topicSuccess)
            {
                return new RedirectResult(Url.Content($"/Topic/Index/{id}"));
            }
            else
            {
                ModelState.AddModelError("","回复发布失败，请稍后重试");
                return new RedirectResult(Url.Content($"/Topic/Index/{id}"));
            }
        }

        /// <summary>
        /// 向回复添加回复
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <param name="r_content">回复内容</param>
        /// <returns></returns>
        public async Task<IActionResult> AddToReply(int id,string r_content)
        {
            var loginUser = await _userRepo.FindAsync(p => p.Name == User.Identity.Name);
            if(loginUser == null)
                return RedirectToAction("Login","Account");
            var reply = await _replyRepo.FindAsync(p => p.ReplyId == id);
            if(reply == null)
                throw new Exception("回复找不到，或已被删除");
            var topicId = reply.Topic_Id;
            var topic = await _topicRepo.FindAsync(p => p.TopicId == topicId);
            if(topic==null)
            {
                ModelState.AddModelError("","话题找不到，或者已被删除");
                return RedirectToAction("Index","Home");
            }
            var replyToReply = new Reply()
            {
                Content = r_content,
                Author_Id = loginUser.UserId,
                Author_Name = loginUser.Name,
                Topic_Id = topicId,
                Reply_Id=reply.ReplyId,
                CreateDateTime = DateTime.Now,
            };
            var replySuccess = await _replyRepo.AddAsync(replyToReply);
            topic.Last_ReplyDateTime = replyToReply.CreateDateTime;
            topic.Last_Reply_Id = replyToReply.ReplyId;
            topic.Last_Reply_UserId = replyToReply.Author_Id;
            topic.Reply_Count += 1;
            var topicSuccess = await _topicRepo.UpdateAsync(topic,false);
            loginUser.Reply_Count += 1;
            topicSuccess = await _userRepo.UpdateAsync(loginUser);
            if(replySuccess && topicSuccess)
            {
                return new RedirectResult(Url.Content($"/Topic/Index/{topicId}#{replyToReply.ReplyId}"));
            }
            else
            {
                ModelState.AddModelError("","回复失败，请稍后重试");
                return new RedirectResult(Url.Content($"/Topic/Index/{topicId}"));
            }
        }

        /// <summary>
        /// 删除，回复ID
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var reply = await _replyRepo.FindAsync(p => p.ReplyId == id);
            if(reply == null)
                return Json(new { status = "faile" });
            await _replyRepo.DeleteAsync(reply,false);
            var topic = await _topicRepo.FindAsync(p => p.TopicId == reply.Topic_Id);
            topic.Reply_Count -= 1;
            var topicSuccess = await _topicRepo.UpdateAsync(topic);
            if(topicSuccess)
                return Json(new { status = "success" });
            else
                return Json(new { status = "faile" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;

namespace RedMan.ViewComponentes
{
    /// <summary>
    /// 话题的回复
    /// </summary>
    public class TopicReplies : ViewComponent
    {
        #region - Private -
        private readonly ModelContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;
        private readonly IRepository<User> _userRepo;
        #endregion

        public TopicReplies(ModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._replyRepo = new Repository<Reply>(context);
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(Int64 topicId)
        {
            var topic = await this._topicRepo.FindAsync(p => p.TopicId == topicId);
            if (topic == null)
                return View(new TopicRepliesViewModel() { TopicId = topicId });
            var topicReplies = await this._replyRepo.FindAllDelayAsync(p => p.Topic_Id == topicId && !p.Deleted);
            var loginUser = await this._userRepo.FindAsync(p => p.Name == User.Identity.Name);
            var topicRepliesViewModel = new TopicRepliesViewModel()
            {
                TopicId = topic.TopicId,
                Topic = topic,
                LoginUser = loginUser,
                Replies = topicReplies,
            };
            return View(nameof(TopicReplies), topicRepliesViewModel);
        }
    }
}

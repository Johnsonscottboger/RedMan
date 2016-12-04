using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Services.EntitiesServices;
using Web.Services.IEntitiesServices;

namespace RedMan.ViewComponentes {
    /// <summary>
    /// 首页-右侧-无人回复的话题
    /// </summary>
    public class ZeroReplySubject:ViewComponent
    {
        private readonly MyContext _context;
        private readonly ITopicService _topicSer;

        public ZeroReplySubject(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicSer = new TopicService(context);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var zeroReplySubject = await _topicSer.GetTopicsInCount(p => p.Reply_Count == 0,5);
            return View(nameof(ZeroReplySubject),zeroReplySubject);
        }
    }
}

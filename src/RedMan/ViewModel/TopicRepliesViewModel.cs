using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 话题下所有回复视图模型
    /// </summary>
    public class TopicRepliesViewModel
    {
        /// <summary>
        /// 获取或设置话题 ID
        /// </summary>
        public long TopicId { get; set; }

        /// <summary>
        /// 获取或设置当前话题
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// 获取或设置当前登录的用户
        /// </summary>
        public User LoginUser { get; set; }

        /// <summary>
        /// 获取或设置话题的所有回复
        /// </summary>
        public IQueryable<Reply> Replies { get; set; }
    }
}

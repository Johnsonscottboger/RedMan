using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 用户首页视图模型
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// 获取或设置当前登录的用户是否为管理员
        /// </summary>
        public bool LoginUserIsAdmin { get; set; }

        /// <summary>
        /// 获取或设置当前用户实例
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 获取或设置用户发布过的话题
        /// </summary>
        public IEnumerable<Topic> TopicPublished { get; set; }

        /// <summary>
        /// 获取或设置用户参与过的话题
        /// </summary>
        public IEnumerable<Topic> TopicJoin { get; set; }
    }
}

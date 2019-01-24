using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;
using Web.Model.Paging;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 所有话题页面视图模型
    /// </summary>
    public class AllTopicViewModel
    {
        /// <summary>
        /// 获取或设置当前用户
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 获取或设置当前用户所有话题的分页模型实例
        /// </summary>
        public PagingModel<IndexTopicsViewModel> Topics { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 话题首页视图模型
    /// </summary>
    public class IndexTopicsViewModel
    {
        /// <summary>
        /// 获取或设置当前话题类别
        /// </summary>
        public TopicTapViewModel Tab { get; set; }

        /// <summary>
        /// 获取或设置当前话题分类
        /// </summary>
        public TopicTypeViewModel Type { get; set; }

        /// <summary>
        /// 获取或设置用户头像 Url
        /// </summary>
        public string UserAvatarUrl { get; set; }

        /// <summary>
        /// 获取或设置当前用户 ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 获取或设置当前用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置当前话题回复数量
        /// </summary>
        public int RepliesCount { get; set; }

        /// <summary>
        /// 获取或设置当前话题访问次数
        /// </summary>
        public int VisitsCount { get; set; }

        /// <summary>
        /// 获取或设置当前话题最新回复地址
        /// </summary>
        public string LastReplyUrl { get; set; }

        /// <summary>
        /// 获取或设置当前话题最新回复用户头像 Url
        /// </summary>
        public string LastReplyUserAvatarUrl { get; set; }

        /// <summary>
        /// 获取或设置当前话题最新回复时间
        /// </summary>
        public string LastReplyDateTime { get; set; }

        /// <summary>
        /// 获取或设置当前话题 ID
        /// </summary>
        public long TopicId { get; set; }

        /// <summary>
        /// 获取或设置当前话题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置当前话题是否置顶
        /// </summary>
        public bool Top { get; set; }

        /// <summary>
        /// 获取或设置当前话题是否为精华话题
        /// </summary>
        public bool Good { get; set; }

        /// <summary>
        /// 获取或设置当前话题创建时间
        /// </summary>
        public string CreateDateTime { get; set; }
    }

    /// <summary>
    /// 所有话题类别
    /// </summary>
    public enum TopicTapViewModel
    {
        all,
        good,
        share,
        ask,
        job
    }

    /// <summary>
    /// 话题分类
    /// </summary>
    public enum TopicTypeViewModel
    {
        置顶,
        精华,
        分享,
        问答,
        二手
    }
}

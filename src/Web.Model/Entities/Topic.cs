using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 话题
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// 获取或设置话题 ID
        /// </summary>
        [Key]
        public Int64 TopicId { get; set; }

        /// <summary>
        /// 获取或设置话题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置话题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取话题内容 Markdown 转换成的 HTML
        /// </summary>
        [NotMapped]
        public string Html { get { return new MarkdownSharp.Markdown().Transform(Content?.Trim()); } }

        /// <summary>
        /// 获取或设置作者 ID
        /// </summary>
        public Int64 AuthorId { get; set; }

        /// <summary>
        /// 获取或设置话题是否置顶
        /// </summary>
        public bool Top { get; set; } = false;

        /// <summary>
        /// 获取或设置话题是否为精华
        /// </summary>
        public bool Good { get; set; } = false;

        /// <summary>
        /// 获取或设置话题被锁定
        /// </summary>
        public bool Lock { get; set; } = false;

        /// <summary>
        /// 获取或设置话题的回复数量
        /// </summary>
        public Int32 ReplyCount { get; set; }

        /// <summary>
        /// 获取或设置话题的访问数量
        /// </summary>
        public Int32 VisitCount { get; set; }

        /// <summary>
        /// 获取或设置话题被收藏的次数
        /// </summary>
        public Int32 Collect_Count { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 获取或设置更新时间
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }
        
        /// <summary>
        /// 获取或设置最新回复的 ID
        /// </summary>
        public Int64? Last_Reply_Id { get; set; }

        /// <summary>
        /// 获取或设置最新回复的用户 ID
        /// </summary>
        public Int64? LastReplyUserId { get; set; }

        /// <summary>
        /// 获取或设置最新回复时间
        /// </summary>
        public DateTime? Last_ReplyDateTime { get; set; }

        /// <summary>
        /// 获取或设置设置话题的类别
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 获取或设置是否已被删除
        /// </summary>
        public bool Deleted { get; set; } = false;
    }
}

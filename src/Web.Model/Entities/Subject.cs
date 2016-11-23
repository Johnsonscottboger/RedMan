using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Web.Model.Enums;

namespace RedMan.Model.Entities
{
    /// <summary>
    /// 主题
    /// </summary>
    public class Subject
    {
        public Subject()
        {
            Replies = new HashSet<Reply>();
        }

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public Int64 SubjectId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(20,ErrorMessage ="输入的标题过长")]
        public string Title { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 浏览数量
        /// </summary>
        public Int64 VisitNumber { get; set; }

        /// <summary>
        /// 回复数量
        /// </summary>
        public Int64 ReplyNumber { get; set; }

        /// <summary>
        /// 被收藏数量
        /// </summary>
        public Int64 FavoriteNumber { get; set; }

        /// <summary>
        /// 是否已被删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime PubDateTime { get; set; }

        /// <summary>
        /// 最新回复时间
        /// </summary>
        public DateTime LastReplyDateTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(1024,ErrorMessage ="输入的内容过长")]
        public string Content { get; set; }

        /// <summary>
        /// 主题分类
        /// </summary>
        public SubjectType SubjectType { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public virtual ICollection<Reply> Replies { get; set; } 
    }
}

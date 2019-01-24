using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 回复
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// 获取或设置回复 ID
        /// </summary>
        [Key]
        public Int64 ReplyId { get; set; }

        /// <summary>
        /// 获取或设置回复内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取回复 Markdown 转换成的 HTML
        /// </summary>
        [NotMapped]
        public string Html { get { return new MarkdownSharp.Markdown().Transform(Content?.Trim()); } }

        /// <summary>
        /// 获取或设置回复的作者 ID
        /// </summary>
        public Int64 Author_Id { get; set; }

        /// <summary>
        /// 获取或设置回复的作者名称
        /// </summary>
        public string Author_Name { get; set; }

        /// <summary>
        /// 获取或设置话题 ID
        /// </summary>
        public Int64 Topic_Id { get; set; }

        /// <summary>
        /// 获取或设置回复 ID
        /// </summary>
        public Int64? Reply_Id { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 获取或设置更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
        
        /// <summary>
        /// 获取或设置是否已经删除
        /// </summary>
        public bool Deleted { get; set; } = false;
    }
}

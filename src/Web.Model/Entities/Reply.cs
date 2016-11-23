using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Enums;

namespace RedMan.Model.Entities
{
    /// <summary>
    /// 回复
    /// </summary>
    public class Reply
    {
        [Key]
        public Int64 ReplyId { get; set; }

        /// <summary>
        /// 楼层ID
        /// </summary>
        public Int64 Id { get; set; }
        
        /// <summary>
        /// 回复一个回复
        /// </summary>
        public Int64 ReplyToId { get; set; } 

        /// <summary>
        /// 发表人
        /// </summary>
        public User FromUser { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public User ToUser { get; set; }

        /// <summary>
        /// 赞
        /// </summary>
        public Int64 LikeNumber { get; set; }

        public Subject Subject { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType ReplyType { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime PubDateTime { get; set; }
    }
}

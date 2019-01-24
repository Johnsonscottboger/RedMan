using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 获取或设置消息 ID
        /// </summary>
        public Int64 MessageId { get; set; }

        /// <summary>
        /// 获取或设置消息送到的用户 ID
        /// </summary>
        public Int64 ToUserId { get; set; }

        /// <summary>
        /// 获取或设置发送消息的用户 ID
        /// </summary>
        public Int64 FromUserId { get; set; }

        /// <summary>
        /// 获取或设置发送消息的用户名称
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 获取或设置消息所在的话题 ID
        /// </summary>
        public Int64? Topic_Id { get; set; }

        /// <summary>
        /// 获取或设置消息的回复 ID
        /// </summary>
        public Int64? Reply_Id { get; set; }

        /// <summary>
        /// 获取或设置消息标题
        /// </summary>
        public string Tilte { get; set; }

        /// <summary>
        /// 获取或设置消息的来源回复 ID
        /// </summary>
        public Int64? FromReplyId { get; set; }

        /// <summary>
        /// 获取或设置消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置消息是否已经阅读
        /// </summary>
        public bool Has_Read { get; set; } = false;

        /// <summary>
        /// 获取或设置消息的创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 话题收藏
    /// </summary>
    public class TopicCollect
    {
        /// <summary>
        /// 获取或设置话题收藏 ID
        /// </summary>
        [Key]
        public Int64 TopicCollectId { get; set; }

        /// <summary>
        /// 获取或设置用户 ID
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        /// 获取或设置话题 ID
        /// </summary>
        public Int64 TopicId { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
    }
}

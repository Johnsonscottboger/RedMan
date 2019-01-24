using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 获取或设置用户 ID
        /// </summary>
        [Key]
        public Int64 UserId { get; set; }

        /// <summary>
        /// 获取或设用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 获取或设置用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 获取或设置用户积分
        /// </summary>
        public Int32 Score { get; set; }

        /// <summary>
        /// 获取或设置话题数量
        /// </summary>
        public Int32 Topic_Count { get; set; }

        /// <summary>
        /// 获取或设置回复数量
        /// </summary>
        public Int32 Reply_Count { get; set; }

        /// <summary>
        /// 获取或设置收藏话题的数量
        /// </summary>
        public Int32 Collect_Topic_Count { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 获取或设置更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// 获取或设置是否已激活
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// 获取或设置未读消息数量
        /// </summary>
        public int UnreadMsg_Count { get; set; }

        /// <summary>
        /// 获取或设置角色
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 获取或设置是否为管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.Model.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public User()
        {
            FollowingUsers = new HashSet<User>();
            FollowUsers = new HashSet<User>();
            Subjects = new HashSet<Subject>();
            FavoriteSubjects = new HashSet<Subject>();
            PubReplies = new HashSet<Reply>();
            ReceivedReplies = new HashSet<Reply>();
        }

        [Key]
        public int UserId { get; set; }

        [DataType(DataType.EmailAddress,ErrorMessage ="请输入正确的邮箱地址")]
        public string Eamil { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataType(DataType.Password,ErrorMessage ="请至少出入一个小写字母，一个大写母，一个符号")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(16,ErrorMessage ="输入的昵称过长")]
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public string Integrate { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDateTime { get; set; }

        /// <summary>
        /// 我关注的用户
        /// </summary>
        public virtual ICollection<User> FollowingUsers { get; set; }

        /// <summary>
        /// 关注我的用户
        /// </summary>
        public virtual ICollection<User> FollowUsers { get; set; }

        /// <summary>
        /// 发表过的主题
        /// </summary>
        public virtual ICollection<Subject> Subjects { get; set; }

        /// <summary>
        /// 收藏的主题
        /// </summary>
        public virtual ICollection<Subject> FavoriteSubjects { get; set; }

        /// <summary>
        /// 发表过的回复
        /// </summary>
        public virtual ICollection<Reply> PubReplies { get; set; }

        /// <summary>
        /// 接收到的回复
        /// </summary>
        public virtual ICollection<Reply> ReceivedReplies { get; set; }
    }
}

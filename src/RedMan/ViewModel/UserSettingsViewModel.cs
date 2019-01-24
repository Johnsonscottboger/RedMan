using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 用户设置视图模型
    /// </summary>
    public class UserSettingsViewModel
    {
        /// <summary>
        /// 获取或设置用户 ID
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        [Required(ErrorMessage = "请输入姓名")]
        [MinLength(2, ErrorMessage = "请输入至少两个字")]
        [MaxLength(10, ErrorMessage = "请输入少于10个字")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置邮箱地址
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置用户个性签名
        /// </summary>
        [MinLength(6, ErrorMessage = "请至少输入6个字")]
        [MaxLength(120, ErrorMessage = "请输入少于120个字")]
        public string Signature { get; set; }

        /// <summary>
        /// 获取或设置用户头像地址
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 获取或设置用户上传的头像文件
        /// </summary>
        public IFormFile AvatarFile { get; set; }
    }

    /// <summary>
    /// 修改密码视图模型
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// 获取或设置用户 ID
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        /// 获取或设置用户的旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 获取或设置用户的新密码
        /// </summary>
        [MinLength(6, ErrorMessage = "密码长度不得少于6个字")]
        [MaxLength(16, ErrorMessage = "密码上都不得超过16个字")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置用户再次确认的密码
        /// </summary>
        [Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 登录页面视图模型
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 获取或设置邮箱地址
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空!")]
        [RegularExpression(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\w\w)", ErrorMessage = "输入的邮箱地址不合法")]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空!")]
        public string Password { get; set; }
    }
}

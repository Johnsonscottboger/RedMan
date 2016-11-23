using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RedMan.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "用户名不能为空!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="请输入正确的邮箱地址")]
        [Remote("CheckEmailIsExist","Account")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "两次输入密码不一致!")]
        public string ConfirmPassword { get; set; }
    }
}

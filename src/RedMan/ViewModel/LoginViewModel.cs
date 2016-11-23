using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "邮箱不能为空!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="请输入正确的邮箱地址")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空!")]
        [MinLength(6,ErrorMessage ="密码长度过短")]
        [MaxLength(16,ErrorMessage ="密码长度过长")]
        public string Password { get; set; }
    }
}

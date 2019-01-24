using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 忘记密码视图模型
    /// </summary>
    public class ForgetPasswordViewModel
    {
        /// <summary>
        /// 获取或设置邮箱地址
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱地址")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }
    }
}

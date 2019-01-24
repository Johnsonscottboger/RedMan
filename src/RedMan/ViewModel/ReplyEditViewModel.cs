using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 修改回复页面的视图模型
    /// </summary>
    public class ReplyEditViewModel
    {
        /// <summary>
        /// 获取或设置回复的 ID
        /// </summary>
        public long ReplyId { get; set; }

        /// <summary>
        /// 获取或设置回复的内容
        /// </summary>
        [Required(ErrorMessage ="请输入内容")]
        [MinLength(5,ErrorMessage ="请至少输入5个字")]
        [MaxLength(2048,ErrorMessage ="不得超过2048个字")]
        public string Content { get; set; }
    }
}

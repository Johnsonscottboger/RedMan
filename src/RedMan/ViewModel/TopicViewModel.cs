using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewModel
{
    public class TopicViewModel
    {
        [Required(ErrorMessage ="请选择板块")]
        public int Tab { get; set; }
        [Required(ErrorMessage ="请输入标题")]
        [MinLength(5,ErrorMessage ="标题长度应输入 5 字以上")]
        public string Title { get; set; }

        [Required(ErrorMessage = "请输入内容")]
        [MinLength(10,ErrorMessage ="内容字数必须在 10 字以上")]
        [MaxLength(500,ErrorMessage ="内容字数在 500 字以内")]
        public string Content { get; set; }
    }
}

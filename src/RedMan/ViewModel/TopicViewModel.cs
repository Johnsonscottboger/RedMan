using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace RedMan.ViewModel
{
    /// <summary>
    /// 话题页面视图模型
    /// </summary>
    public class TopicViewModel
    {
        /// <summary>
        /// 获取或设置话题的类别
        /// </summary>
        [Required(ErrorMessage = "请选择板块")]
        public int Tab { get; set; }

        /// <summary>
        /// 获取或设置话题的标题
        /// </summary>
        [Required(ErrorMessage = "请输入标题")]
        [MinLength(2, ErrorMessage = "标题长度应输入 2 字以上")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置话题的内容
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        [MinLength(5, ErrorMessage = "内容字数必须在 5 字以上")]
        [MaxLength(500, ErrorMessage = "内容字数在 500 字以内")]
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置话题ID
        /// </summary>
        public Int64 TopicId { get; set; }

        /// <summary>
        /// 获取或设置话题实例
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// 获取或设置话题作者
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// 获取或设置当前登录用户
        /// </summary>
        public User LoginUser { get; set; }

        /// <summary>
        /// 获取或设置是否已被当前登录用户收藏
        /// </summary>
        public bool Collected { get; set; }

        /// <summary>
        /// 获取或设置 Markdown 转换的 HTML
        /// </summary>
        public string Html { get; set; }
    }
}

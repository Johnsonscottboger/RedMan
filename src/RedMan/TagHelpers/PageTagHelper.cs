using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Model.Paging;

namespace RedMan.TagHelpers
{
    /// <summary>
    ///用于在 HTML 中生成包含页码的元素
    /// </summary>
    public class PageTagHelper : TagHelper
    {
        /// <summary>
        /// 获取或设置分页信息<see cref="PagingInfo"/>实例
        /// </summary>
        private PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// 获取或设置生成 Url 的委托
        /// </summary>
        public Func<long, string> PageUrl { get; set; }

        /// <summary>
        /// 处理元素生成方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (PagingInfo == null)
                return;

            var i = 1L;
            var showPage = 7;     //显示的页数
            var midelPage = 4;    //中间页数
            var builder = new StringBuilder();

            #region 上一页
            if (!(PagingInfo.CurrentPage - 1 >= 1))
            {
                builder.Append("<li class=\"disabled\"><a>«</a></li>");
            }
            else
            {
                builder.Append($"<li><a href=\"{PageUrl(PagingInfo.CurrentPage - 1)}\">«</a></li>");
            }
            #endregion

            #region 中间页码

            #region 总页数小于显示的页数
            //总页数小于等于显示页数
            if (PagingInfo.TotalPages <= showPage)
            {
                i = 1;
                var endIndex = PagingInfo.TotalPages;
                var tagBuilder = string.Empty;
                for (; i <= endIndex; i++)
                {
                    if (i != PagingInfo.CurrentPage)
                        tagBuilder = $"<li><a href=\"{PageUrl(i)}\">{i}</a></li>";
                    else
                        tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                    builder.Append(tagBuilder);
                }
            }
            #endregion

            #region 总页数大于显示页数
            //总页数大于显示页数
            else if (PagingInfo.TotalPages > showPage)
            {
                //当前页小于中间页
                if (PagingInfo.CurrentPage <= midelPage)
                {
                    i = 1;
                    var endIndex = showPage;
                    string tagBuilder = string.Empty;
                    for (; i < endIndex; i++)
                    {
                        if (i != PagingInfo.CurrentPage)
                            tagBuilder = $"<li><a href=\"{PageUrl(i)}\">{i}</a></li>";
                        else
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        builder.Append(tagBuilder);
                    }
                    builder.Append("<li><a>...</a></li>");
                }
                //当前页大于中间页
                else if (PagingInfo.CurrentPage > midelPage && PagingInfo.TotalPages - PagingInfo.CurrentPage >= midelPage - 1)
                {
                    i = PagingInfo.CurrentPage - midelPage + 2;
                    var endIndex = PagingInfo.CurrentPage + midelPage - 1;
                    string tagBuilder = string.Empty;
                    builder.Append("<li><a>...</a></li>");
                    for (; i < endIndex; i++)
                    {
                        if (i != PagingInfo.CurrentPage)
                            tagBuilder = $"<li><a href=\"{PageUrl(i)}\">{i}</a></li>";
                        else
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        builder.Append(tagBuilder);
                    }
                    builder.Append("<li><a>...</a></li>");
                }
                else if (PagingInfo.TotalPages - PagingInfo.CurrentPage < midelPage)
                {
                    i = PagingInfo.CurrentPage - midelPage + 2;
                    var endIndex = PagingInfo.TotalPages;
                    string tagBuilder = string.Empty;
                    builder.Append("<li><a>...</a></li>");
                    for (; i <= endIndex; i++)
                    {
                        if (i != PagingInfo.CurrentPage)
                            tagBuilder = $"<li><a href=\"{PageUrl(i)}\">{i}</a></li>";
                        else
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        builder.Append(tagBuilder);
                    }
                }
            }
            #endregion

            #endregion

            #region 下一页
            if (!(PagingInfo.CurrentPage + 1 <= PagingInfo.TotalPages))
            {
                builder.Append("<li class=\"disable\"><a>»</a></li>");
            }
            else
            {
                builder.Append($"<li><a href=\"{PageUrl(PagingInfo.CurrentPage + 1)}\">»</a></li>");
            }
            #endregion

            output.TagName = "ul";
            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}

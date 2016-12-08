using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model.Paging;

namespace RedMan.TagHelpers
{
    public class PageTagHelper:TagHelper
    {
        public PagingInfo pagingInfo { get; set; }
        public Func<long,string> pageUrl { get; set; }

        public override void Process(TagHelperContext context,TagHelperOutput output)
        {
            base.Process(context,output);

            if(pagingInfo == null)
            {
                return;
            }

            Int64 i = 1;
            Int32 showPage = 7;
            Int32 midelPage = 4;
            var builder = new StringBuilder();

            if(!(pagingInfo.CurrentPage - 1 >= 1))
            {
                builder.Append("<li class=\"disabled\"><a>«</a></li>");
            }
            else
            {
                builder.Append($"<li><a href=\"{pageUrl(pagingInfo.CurrentPage - 1)}\">«</a></li>");
            }

            #region 生成页码
            //总页数小于等于显示页数
            if(pagingInfo.TotalPages <= showPage)
            {
                i = 1;
                string tagBuilder = string.Empty;
                for(; i <= pagingInfo.TotalPages; i++)
                {
                    if(i != pagingInfo.CurrentPage)
                    {
                        
                        tagBuilder = $"<li><a href=\"{pageUrl(i)}\">{i}</a></li>";
                    }
                    else
                    {
                        tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                    }
                    builder.Append(tagBuilder);
                }
            }
            //总页数大于显示页数
            else if(pagingInfo.TotalPages > showPage)
            {
                //当前页小于中间页
                if(pagingInfo.CurrentPage <= midelPage)
                {
                    i = 1;
                    string tagBuilder = string.Empty;
                    for(; i < showPage; i++)
                    {
                        if(i != pagingInfo.CurrentPage)
                        {
                            tagBuilder = $"<li><a href=\"{pageUrl(i)}\">{i}</a></li>";
                        }
                        else
                        {
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        }
                        builder.Append(tagBuilder);
                    }
                }
                //当前页大于中间页
                else if(pagingInfo.CurrentPage > midelPage && pagingInfo.TotalPages - pagingInfo.CurrentPage >= midelPage)
                {
                    i = pagingInfo.CurrentPage - midelPage + 1;
                    string tagBuilder = string.Empty;
                    for(; i < pagingInfo.CurrentPage + midelPage - 1; i++)
                    {
                        if(i != pagingInfo.CurrentPage)
                        {
                            tagBuilder = $"<li><a href=\"{pageUrl(i)}\">{i}</a></li>";
                        }
                        else
                        {
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        }
                        builder.Append(tagBuilder);
                    }
                }
                else if(pagingInfo.TotalPages - pagingInfo.CurrentPage < midelPage)
                {
                    i = pagingInfo.CurrentPage - midelPage + 1;
                    string tagBuilder = string.Empty;
                    for(; i <= pagingInfo.TotalPages; i++)
                    {
                        if(i != pagingInfo.CurrentPage)
                        {
                            tagBuilder = $"<li><a href=\"{pageUrl(i)}\">{i}</a></li>";
                        }
                        else
                        {
                            tagBuilder = $"<li class=\"disabled active\"><a>{i}</a></li>";
                        }
                        builder.Append(tagBuilder);
                    }
                }
            }
            #endregion

            if(!(pagingInfo.CurrentPage + 1 <= pagingInfo.TotalPages))
            {
                builder.Append("<li class=\"disable\"><a>»</a></li>");
            }
            else
            {
                builder.Append($"<li><a href=\"{pageUrl(pagingInfo.CurrentPage + 1)}\">»</a></li>");
            }

            output.TagName = "ul";
            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}

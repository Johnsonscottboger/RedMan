using Microsoft.AspNetCore.Mvc;
using RedMan.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewComponentes
{
    /// <summary>
    /// 首页-右侧-无人回复的话题
    /// </summary>
    public class ZeroReplySubject:ViewComponent
    {
        private readonly MyContext _context;
        public ZeroReplySubject(MyContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO:无人回复的话题
            await Task.FromResult(0);
            return View(nameof(ZeroReplySubject));
        }
    }
}

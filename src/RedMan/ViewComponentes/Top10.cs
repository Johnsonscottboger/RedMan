using Microsoft.AspNetCore.Mvc;
using RedMan.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewComponentes
{
    /// <summary>
    /// 首页-右侧-前10名用户
    /// </summary>
    public class Top10:ViewComponent
    {
        private readonly MyContext _context;
        public Top10(MyContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO:前10名用户
            await Task.FromResult(0);
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            return View("Top10",list);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RedMan.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.ViewComponentes
{
    /// <summary>
    /// 首页-右侧-用户面板
    /// </summary>
    public class UserPanel:ViewComponent
    {
        private readonly MyContext _context;
        public UserPanel(MyContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO:用户面板
            await Task.FromResult(0);
            return View(nameof(UserPanel));
        }
    }
}

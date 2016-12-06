using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;

namespace RedMan.ViewComponentes {
    /// <summary>
    /// 首页-头部-右侧-导航菜单
    /// </summary>
    public class TopNav:ViewComponent
    {
        private readonly MyContext _context;
        public TopNav(MyContext context)
        {
            this._context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO:首页-头部-右侧导航菜单
            await Task.FromResult(0);
            return View(nameof(TopNav),new User());
        }
    }
}

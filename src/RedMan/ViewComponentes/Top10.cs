using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Services.EntitiesServices;
using Web.Services.IEntitiesServices;

namespace RedMan.ViewComponentes {
    /// <summary>
    /// 首页-右侧-前10名用户
    /// </summary>
    public class Top10:ViewComponent
    {
        private readonly MyContext _context;
        private readonly IUserService _userService;

        public Top10(MyContext context)
        {
            this._context = context;
            this._userService = new UserService(context);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO:前10名用户
            var top10User = await _userService.GetTopScoreUser(10);
            return View("Top10",top10User);
        }
    }
}

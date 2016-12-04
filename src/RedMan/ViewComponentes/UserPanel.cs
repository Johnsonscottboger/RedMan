using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Services.EntitiesServices;
using Web.Services.IEntitiesServices;

namespace RedMan.ViewComponentes {
    /// <summary>
    /// 首页-右侧-用户面板
    /// </summary>
    public class UserPanel:ViewComponent
    {
        private readonly MyContext _context;
        private readonly IIdentityService _identitySer;
        private readonly IUserService _userSer;
        public UserPanel(MyContext context,IIdentityService iidentityService)
        {
            this._context = context;
            this._identitySer = iidentityService;
            this._userSer = new UserService(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(String username)
        {
            //当前登录用户名
            if(username== "loginUserName") {
                var loginUserName = User.Identity.Name;
                var loginUser = await _userSer.GetUserByLoginName(loginUserName);
                //已登录
                if(loginUserName != null) {
                    return View(nameof(UserPanel),loginUser);
                }
                //未登录
                else {
                    return View(nameof(UserPanel));
                }
            }
            //话题作者，或其他
            else {
                var showUser = await _userSer.GetUserByLoginName(username);
                if(showUser != null) {
                    return View(nameof(UserPanel),showUser);
                }
                else {
                    return View(nameof(UserPanel));
                }
            }
        }
    }
}

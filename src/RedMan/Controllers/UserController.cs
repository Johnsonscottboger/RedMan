using Microsoft.AspNetCore.Mvc;
using Web.Model.Context;

namespace RedMan.Controllers {
    public class UserController : Controller
    {
        private readonly MyContext _context;
        public UserController(MyContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// 获取积分排行前100名用户
        /// </summary>
        /// <returns></returns>
        [Route("/user/top100")]
        public IActionResult Top100()
        {
            return View();
        }

        
    }
}

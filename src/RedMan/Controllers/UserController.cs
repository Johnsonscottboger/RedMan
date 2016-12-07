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

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public string Avatar(int id)
        {
            return null;
        }
    }
}

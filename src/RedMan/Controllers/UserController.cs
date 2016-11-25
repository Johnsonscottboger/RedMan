using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedMan.Model.Context;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RedMan.Controllers
{
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;

namespace RedMan.Controllers {
    public class UserController : Controller
    {
        private readonly MyContext _context;
        private readonly IRepository<User> _userRepo;

        public UserController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
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
        /// 获取用户头像链接
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<FileContentResult> GetUserAvatarUrl(int id)
        {
            var user = await _userRepo.FindAsync(p => p.UserId == id);
            if(user == null)
                return null;
            var path = $"{Directory.GetCurrentDirectory()}/wwwroot/{user.Avatar}";
            try
            {
                using(FileStream fs = new FileStream(path,FileMode.Open))
                {
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes,0,bytes.Length);
                    return File(bytes,"application/x-png");
                }
            }
            catch(Exception)
            {
                return null;
            }
            
        }
        #region 辅助方法

        
        #endregion
    }
}

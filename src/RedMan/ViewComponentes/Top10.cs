using Microsoft.AspNetCore.Mvc;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using System;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;

namespace RedMan.ViewComponentes
{
    /// <summary>
    /// 首页-右侧-前10名用户
    /// </summary>
    public class Top10 : ViewComponent
    {
        #region - Private -
        private readonly ModelContext _context;
        private readonly IRepository<User> _userRepo;
        #endregion

        public Top10(ModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //前10名用户
            var top10User = await this._userRepo.FindTopDelayAsync(10, p => p.Active, p => -p.Score);
            return View("Top10", top10User);
        }
    }
}

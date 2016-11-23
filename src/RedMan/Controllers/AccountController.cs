using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Services.EntitiesServices;
using Microsoft.AspNetCore.Authorization;
using RedMan.ViewModel;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RedMan.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IdentityService _identityService;

        public AccountController(IdentityService identityService)
        {
            this._identityService = identityService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);
            var user = await _identityService.CheckUserAsync(model.Email, model.Password);
            if(user==null)
            {
                ModelState.AddModelError(string.Empty, "用户名或密码错误");
                model.Password = string.Empty;
                return View(model);
            }
            //登录
            await HttpContext.Authentication.SignInAsync(IdentityService.AuthenticationScheme, user);
            //重定向
            return RedirectToLocal(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model,string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);
            var result = await _identityService.RegisterAsync(model.Email, model.Password);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorString);
                return View(model);
            }
            //登录
            await HttpContext.Authentication.SignInAsync(IdentityService.AuthenticationScheme, result.User);
            return RedirectToLocal(returnUrl);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        #region 辅助
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Services.EntitiesServices;
using RedMan.ViewModel;

namespace RedMan.Controllers
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        #region - Private -
        private readonly IdentityService _identityService;
        #endregion

        /// <summary>
        /// 初始化账户管理控制器<see cref="AccountController"/>实例
        /// </summary>
        /// <param name="identityService">指定的认证服务<see cref="IdentityService"/>实例</param>
        public AccountController(IdentityService identityService)
        {
            this._identityService = identityService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl">指定登录成功后返回到的地址, 默认为<c>null</c></param>
        /// <returns>登录页面</returns>
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["Error"] = false;
            ViewData["ReturnUrl"] = returnUrl;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewData["Error"] = true;
                ModelState.AddModelError("", "此操作需要登录，请登录后继续!");
            }
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model">指定表单提交的模型<see cref="LoginViewModel"/>实例</param>
        /// <param name="returnUrl">指定登录成功后返回到的地址, 默认为<c>null</c></param>
        /// <returns>登录成功或失败跳转到的页面</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["Error"] = true;
            ViewData["ReturnUrl"] = returnUrl;
            model.Email = model.Email?.Trim();
            model.Password = model.Password?.Trim();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await this._identityService.CheckUserAsync(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "用户名或密码错误");
                model.Password = string.Empty;
                return View(model);
            }

            ViewData["Error"] = false;
            await HttpContext.Authentication.SignInAsync(IdentityService.AuthenticationScheme, user);
            return RedirectToUrlOrHome(returnUrl);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="returnUrl">指定注册成功后返回到的地址, 默认为<c>null</c></param>
        /// <returns>注册页面</returns>
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["Error"] = false;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">指定表单提交的模型<see cref="RegisterViewModel"/>实例</param>
        /// <param name="returnUrl">指定注册成功后返回到的地址, 默认为<c>null</c></param>
        /// <returns>注册成功或失败跳转到的页面</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["Error"] = true;
            ViewData["ReturnUrl"] = returnUrl;
            model.Name = model.Name?.Trim();
            model.Email = model.Email?.Trim();
            model.Password = model.Password?.Trim();
            model.ConfirmPassword = model.ConfirmPassword?.Trim();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await this._identityService.RegisterAsync(model.Email, model.Name, model.Password);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorString);
                return View(model);
            }
            ViewData["Error"] = false;
            await HttpContext.Authentication.SignInAsync(IdentityService.AuthenticationScheme, result.User);
            return RedirectToUrlOrHome(returnUrl);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns>注销成功之后跳转到的页面</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(IdentityService.AuthenticationScheme);
            return RedirectToUrlOrHome();
        }

        #region - Private Method -
        /// <summary>
        /// 重定向到指定Url或主页
        /// </summary>
        /// <param name="returnUrl">指定重定向到的 url</param>
        /// <returns>重定向结果</returns>
        private IActionResult RedirectToUrlOrHome(string returnUrl = null)
        {
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
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

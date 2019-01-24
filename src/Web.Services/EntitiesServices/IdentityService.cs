using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.DataAccess.IRepository;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public class IdentityService : IIdentityService
    {
        #region - Private -
        private IIdentityRepository<User> _identityRepository;
        #endregion

        /// <summary>
        /// 认证方案
        /// </summary>
        public const string AuthenticationScheme = "MyAuthCookie";

        /// <summary>
        /// 初始化<see cref="IdentityService"/>实例
        /// </summary>
        /// <param name="identityRepository">指定的仓储实例</param>
        public IdentityService(IIdentityRepository<User> identityRepository)
        {
            this._identityRepository = identityRepository;
        }

        /// <summary>
        /// 检索用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> CheckUserAsync(String email, String password)
        {
            var user = await this._identityRepository.GetUserAsync(p => p.Email == email && p.Password == password);
            if (user == null)
                return null;
            var ci = CreateClaimsIdentity(user);
            var roles = user.Roles;
            AddRoleClaims(ci, roles);
            return new ClaimsPrincipal(ci);
        }

        /// <summary>
        /// 检索用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> CheckUserAsync(string email, string username, string password)
        {
            var user = await this._identityRepository.GetUserAsync(p => p.Email == email && p.Name == username && p.Password == password);
            if (user == null)
                return null;
            var ci = CreateClaimsIdentity(user);
            var roles = user.Roles;
            AddRoleClaims(ci, roles);
            return new ClaimsPrincipal(ci);
        }

        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterAsync(string email, string username, string password)
        {
            if (await this._identityRepository.CheckEmailAsync(p => p.Name == username))
            {
                return new IdentityResult("用户名已存在！");
            }
            if (await this._identityRepository.CheckEmailAsync(p => p.Email == email))
            {
                return new IdentityResult("邮箱已存在");
            }
            var user = new User()
            {
                Email = email,
                Name = username,
                Password = password,
                LoginName = username,
                CreateDateTime = DateTime.Now,
                Active = true,
                Avatar = "/img/8791709.png",
                Roles = new List<Role>()
                {
                    new Role() {RoleName="普通用户" }
                }
            };
            await this._identityRepository.AddUserAsync(user);
            var ci = CreateClaimsIdentity(user);
            return new IdentityResult(new ClaimsPrincipal(ci));
        }

        #region 辅助方法
        private ClaimsIdentity CreateClaimsIdentity(User user)
        {
            //用当前用户信息创建一个ClaimsIdentity
            //AuthenticationScheme需要和Cookie中间件中的AuthenticationScheme一致
            //如果添加的角色使用的类型不是ClaimTypes.Role，则需要在此处指定类型
            //var result = new ClaimsIdentity(AuthenticationScheme,NameType,RoleType);
            var result = new ClaimsIdentity(AuthenticationScheme);
            result.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            return result;
        }

        private void AddRoleClaims(ClaimsIdentity claimsIdentity, ICollection<Role> roles)
        {
            if (roles != null)
            {
                foreach (var item in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
                }
            }
        }
        #endregion
    }
}

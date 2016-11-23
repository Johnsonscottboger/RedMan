using RedMan.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.DataAccess.IRepository;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class IdentityService : IIdentityService
    {
        public const string AuthenticationScheme = "MyAuthCookie";
        private IIdentityRepository<User> _identityRepository;
        public IdentityService(IIdentityRepository<User> identityRepository)
        {
            this._identityRepository = identityRepository;
        }

        /// <summary>
        /// 使用邮箱地址和密码获取用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> CheckUserAsync(string email, string password)
        {
            var user = await _identityRepository.GetUserAsync(p => p.Eamil == email && p.Password == password);
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
        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            if(await _identityRepository.CheckEmailAsync(p=>p.Eamil==email&&p.Password==password))
            {
                return new IdentityResult("用户名已存在！");
            }
            var user = new User()
            {
                Eamil = email,
                Password = password
            };
            await _identityRepository.AddUserAsync(user);
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
            result.AddClaim(new Claim(ClaimTypes.Name, user.Eamil));
            return result;
        }

        private void AddRoleClaims(ClaimsIdentity claimsIdentity,ICollection<Role> roles)
        {
            foreach(var item in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
            }
        }
        #endregion
    }
}

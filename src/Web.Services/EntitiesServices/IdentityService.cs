using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.DataAccess.IRepository;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices {
    public class IdentityService : IIdentityService
    {
        public const string AuthenticationScheme = "MyAuthCookie";
        private IIdentityRepository<User> _identityRepository;
        public IdentityService(IIdentityRepository<User> identityRepository)
        {
            this._identityRepository = identityRepository;
        }

        /// <summary>
        /// 检索用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> CheckUserAsync(string username, string password)
        {
            var user = await _identityRepository.GetUserAsync(p => p.Name == username && p.Password == password);
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
        public async Task<IdentityResult> RegisterAsync(string username, string password)
        {
            if(await _identityRepository.CheckEmailAsync(p=>p.Name==username&&p.Password==password))
            {
                return new IdentityResult("用户名已存在！");
            }
            var user = new User()
            {
                Name = username,
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
            result.AddClaim(new Claim(ClaimTypes.Name, user.Name));
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

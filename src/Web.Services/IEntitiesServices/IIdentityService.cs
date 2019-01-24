using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    /// <summary>
    /// 认证服务接口
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// 检查用户
        /// </summary>
        /// <param name="email">指定的邮箱地址</param>
        /// <param name="password">指定的密码</param>
        /// <returns><see cref="ClaimsPrincipal"/>实例</returns>
        Task<ClaimsPrincipal> CheckUserAsync(string email, string password);

        /// <summary>
        /// 检查用户
        /// </summary>
        /// <param name="email">指定的邮箱地址</param>
        /// <param name="username">指定的用户名称</param>
        /// <param name="password">指定的密码</param>
        /// <returns><see cref="ClaimsPrincipal"/>实例</returns>
        Task<ClaimsPrincipal> CheckUserAsync(string email, string username, string password);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="email">指定的邮箱地址</param>
        /// <param name="username">指定的用户名称</param>
        /// <param name="password">指定的密码</param>
        /// <returns>认证结果<see cref="IdentityResult"/>实例</returns>
        Task<IdentityResult> RegisterAsync(string email, string username, string password);
    }
}

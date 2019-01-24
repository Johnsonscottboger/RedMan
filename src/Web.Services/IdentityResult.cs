using System.Security.Claims;

namespace Web.Services
{
    /// <summary>
    /// 登录认证结果
    /// </summary>
    public class IdentityResult
    {
        /// <summary>
        /// 获取认证是否成功
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// 获取认证错误的信息
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// 获取认证的用户
        /// </summary>
        public ClaimsPrincipal User { get; }

        /// <summary>
        /// 使用指定的错误信息初始化<see cref="IdentityResult"/>实例
        /// </summary>
        /// <param name="error">指定的错误信息</param>
        public IdentityResult(string error)
        {
            Success = false;
            ErrorMessage = error;
        }

        /// <summary>
        /// 使用指定的<see cref="ClaimsPrincipal"/>实例初始化<see cref="IdentityResult"/>实例
        /// </summary>
        /// <param name="user">指定的<see cref="ClaimsPrincipal"/>实例</param>
        public IdentityResult(ClaimsPrincipal user)
        {
            Success = true;
            User = user;
        }
    }
}

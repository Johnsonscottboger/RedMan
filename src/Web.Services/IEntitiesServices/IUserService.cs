using RedMan.Model.Entities;
using System;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    public interface IUserService
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Task<bool> DeleteUser(Int64 id);

        
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result> Updata(User user);

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followUserId">被关注的用户ID</param>
        /// <returns></returns>
        Task<Result> Follow(Int64 userId, Int64 followUserId);
    }
}

using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.Model.Context;
using RedMan.Model.Entities;
using System;
using System.Threading.Tasks;
using Web.Services.IEntitiesServices;
using static Web.Services.Common.Common;

namespace Web.Services.EntitiesServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _user;
        private readonly MyContext context;

        public UserService(MyContext context)
        {
            this.context = context;
            _user = new Repository<User>(context);
        }

        /// <summary>
        /// 通过ID删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(long id)
        {
            var user = await _user.FindAsync(p => p.UserId == id);
            if (user == null)
                return true;
            //标记为删除
            user.IsDelete = true;
            return await _user.UpdateAsync(user);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followUserId">被关注用户ID</param>
        /// <returns></returns>
        public async Task<Result> Follow(Int64 userId, Int64 followUserId)
        {
            var user = await _user.FindAsync(p => p.UserId == userId);
            if (user == null || user.IsDelete)
            {
                return Fail("用户不存在！");
            }
            var followUser = await _user.FindAsync(p => p.UserId == followUserId);
            if (followUser == null || followUser.IsDelete)
            {
                return Fail("被关注的用户不存在！");
            }
            //模拟事务
            user.FollowingUsers.Add(followUser);
            followUser.FollowUsers.Add(user);
            var successUser = await _user.UpdateAsync(user, false);
            var successFollowUser = await _user.UpdateAsync(followUser, false);
            if (successUser && successFollowUser)
            {
                await context.SaveChangesAsync();
                return Success("关注成功!");
            }
            else
            {
                return Fail("关注失败");
            }

        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> Updata(User user)
        {
            var isSuccess = await _user.UpdateAsync(user);
            if (isSuccess)
                return await _user.FindAsync(p => p.UserId == user.UserId);
            else
                return null;
        }
    }
}

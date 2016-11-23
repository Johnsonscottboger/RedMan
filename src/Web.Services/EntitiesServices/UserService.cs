using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.Model.Context;
using RedMan.Model.Entities;
using System;
using System.Threading.Tasks;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> User;
        private readonly MyContext context;

        public UserService(MyContext context)
        {
            this.context = context;
            User = new Repository<User>(context);
        }

        /// <summary>
        /// 通过ID删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(long id)
        {
            var user = await User.FindAsync(p => p.UserId == id);
            if (user == null)
                return true;
            //标记为删除
            user.IsDelete = true;
            return await User.UpdateAsync(user);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="followUserId">被关注用户ID</param>
        /// <returns></returns>
        public async Task<Result> Follow(Int64 userId, Int64 followUserId)
        {
            var user = await User.FindAsync(p => p.UserId == userId);
            if (user == null || user.IsDelete)
            {
                return new Result() { Code = -200, Success = false, Message = "用户不存在" };
            }
            var followUser = await User.FindAsync(p => p.UserId == followUserId);
            if (followUser == null || followUser.IsDelete)
            {
                return new Result() { Code = -200, Success = false, Message = "被关注用户不存在" };
            }
            //模拟事务
            user.FollowingUsers.Add(followUser);
            followUser.FollowUsers.Add(user);
            var successUser = await User.UpdateAsync(user, false);
            var successFollowUser = await User.UpdateAsync(followUser, false);
            if (successUser && successFollowUser)
            {
                await context.SaveChangesAsync();
                return new Result() { Code = 200, Success = true, Message = "关注成功" };
            }
            else
            {
                return new Result() { Code = -200, Success = false, Message = "关注失败" };
            }

        }

        public Task<Result> Updata(User user)
        {
            throw new NotImplementedException();
        }
    }
}

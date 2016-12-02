using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class UserService:IUserService
    {
        private readonly MyContext _context;
        private readonly IRepository<User> _userRepo;
        public UserService(MyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._userRepo = new Repository<User>(context);
        }

        /// <summary>
        /// 获取默认头像链接
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetDefaultAvatarUrl()
        {
            //TODO:获取默认头像链接
            return string.Empty;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="loginName">登录名</param>
        /// <param name="pass">密码</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="avatar_url">头像URL</param>
        /// <returns></returns>
        public async Task<Result> AddUser(string name, string loginName, string pass, string email, string avatar_url)
        {
            if (string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(loginName)
                || string.IsNullOrEmpty(pass)
                || string.IsNullOrEmpty(email))
                throw new ArgumentNullException();
            //TODO:默认头像链接
            if (string.IsNullOrEmpty(avatar_url))
                avatar_url = await GetDefaultAvatarUrl();

            var user = new User()
            {
                Name = name,
                LoginName = loginName,
                Password = pass,
                Email = email,
                Avatar = avatar_url,
                Active = true,
                CreateDateTime=DateTime.Now
            };
            try
            {
                var IsSuccess = await _userRepo.AddAsync(user);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "添加成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "添加失败，未知错误" };
            }
            catch (Exception ex)
            {
                return new Result() { Code = -200, Success = false, Message = $"添加失败,{ex.Message}" };
            }
        }

        /// <summary>
        /// 获取用户头像链接
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task<string> GetGravatar(User user)
        {
            try
            {
                var currentUser = await _userRepo.FindAsync(p => p.UserId == user.UserId);
                if (currentUser == null)
                    return null;
                if (string.IsNullOrEmpty(user.Avatar))
                    return await GetDefaultAvatarUrl();
                else
                    return user.Avatar;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过ID，获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<User> GetUserById(long userId)
        {
            try
            {
                return await _userRepo.FindAsync(p => p.UserId == userId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过登录名，获取用户
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public async Task<User> GetUserByLoginName(string loginName)
        {
            try
            {
                return await _userRepo.FindAsync(p => p.LoginName == loginName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过邮箱，获取用户
        /// </summary>
        /// <param name="eamilAddress">邮箱地址</param>
        /// <returns></returns>
        public async Task<User> GetUserByMail(string eamilAddress)
        {
            try
            {
                return await _userRepo.FindAsync(p => p.Email == eamilAddress);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过ID列表，查找所有用户
        /// </summary>
        /// <param name="userIds">多个用户ID</param>
        /// <returns></returns>
        public IEnumerable<User> GetUsersByUserIds(IEnumerable<long> userIds)
        {
            foreach (var userId in userIds)
            {
                var user = GetUserById(userId);
                user.Wait();
                yield return user.Result;
            }
        }

        /// <summary>
        /// 通过用户名列表，查找所有用户
        /// </summary>
        /// <param name="userNames">多个用户名</param>
        /// <returns></returns>
        public IEnumerable<User> GetUsersByUserNames(IEnumerable<string> userNames)
        {
            foreach(var userName in userNames)
            {
                var user = _userRepo.FindAsync(p => p.Name == userName);
                user.Wait();
                yield return user.Result;
            }
        }
    }
}

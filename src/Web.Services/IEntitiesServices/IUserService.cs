using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace Web.Services.IEntitiesServices
{
    public interface IUserService
    {
        /// <summary>
        /// 根据用户名列表，获取用户列表
        /// </summary>
        /// <param name="userNames">用户名列表</param>
        /// <returns></returns>
        Task<IQueryable<User>> GetUsersByUserNames(IEnumerable<string> userNames);

        /// <summary>
        /// 根据登录名，查找用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        Task<User> GetUserByLoginName(string loginName);

        /// <summary>
        /// 根据用户ID，获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<User> GetUserById(Int64 userId);

        /// <summary>
        /// 根据邮箱地址，获取用户
        /// </summary>
        /// <param name="eamilAddress">邮箱地址</param>
        /// <returns></returns>
        Task<User> GetUserByMail(string eamilAddress);

        /// <summary>
        /// 根据用户ID列表，获取用户列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IQueryable<User>> GetUsersByUserIds(IEnumerable<Int64> userIds);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="loginName">登录名</param>
        /// <param name="pass">密码</param>
        /// <param name="email">邮箱</param>
        /// <param name="avatar_url">头像URL</param>
        /// <param name="active">是否激活</param>
        /// <returns></returns>
        Task<Result> AddUser(string name, string loginName, string pass, string email, string avatar_url);

        /// <summary>
        /// 获取用户头像URL
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        Task<string> GetGravatar(User user);
    }
}

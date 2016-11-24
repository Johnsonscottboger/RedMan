using RedMan.Model.Entities;
using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    public interface ISubjectService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="subject">主题</param>
        /// <returns></returns>
        Task<Subject> Add(User user,Subject subject);

        /// <summary>
        /// 修改主题
        /// </summary>
        /// <param name="subject">主题实体</param>
        /// <returns></returns>
        Task<Subject> UpdateSubject(Subject subject);

        /// <summary>
        /// 删除一条主题
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="id">主题ID</param>
        /// <returns></returns>
        Task<Result> DeleteSubject(User user,Int64 id);

        /// <summary>
        /// 获取一条主题通过ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Task<Subject> GetSubjectById(Int64 id);

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <returns></returns>
        Task<PagingModel<Subject>> GetAllSubject(Int32 pageIndex, Int32 pageSize);

        /// <summary>
        /// 收藏主题
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="subId">主题ID</param>
        /// <returns></returns>
        Task<Result> FavoritesSubject(User user, Int64 subId);

        /// <summary>
        /// 回复主题
        /// </summary>
        /// <param name="subId">主题ID</param>
        /// <param name="user">回复者</param>
        /// <param name="reply">回复实体</param>
        /// <returns></returns>
        Task<PagingModel<Reply>> ReplyToSubject(Int64 subId,User user, Reply reply);
    }
}

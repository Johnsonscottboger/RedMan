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
        /// <param name="subject">实体</param>
        /// <returns></returns>
        bool Add(Subject subject);

        /// <summary>
        /// 删除一条主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result DeleteSubject(Int64 id);

        /// <summary>
        /// 获取一条主题通过ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Subject GetSubjectById(Int64 id);

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <returns></returns>
        PagingModel<Subject> GetAllSubect();

        /// <summary>
        /// 收藏主题
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="subId">主题ID</param>
        /// <returns></returns>
        Result FavoritesSubject(Int64 userId, Int64 subId);

        /// <summary>
        /// 回复主题
        /// </summary>
        /// <param name="subId">主题ID</param>
        /// <param name="reply">回复实体</param>
        /// <returns></returns>
        Result Reply(Int64 subId, Reply reply);
    }
}

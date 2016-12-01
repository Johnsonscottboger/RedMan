using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace Web.Services.IEntitiesServices
{
    public interface ITopic_CollectService
    {
        /// <summary>
        /// 获取一条收藏主题
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        Task<TopicCollect> GetTopicCollect(Int64 userId, Int64 topicId);

        /// <summary>
        /// 根据用户ID，获取收藏的主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IQueryable<TopicCollect>> GetTopicCollectsByUserId(Int64 userId);


        /// <summary>
        /// 添加一条主题收藏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        Task<Result> AddTopicCollect(Int64 userId, Int64 topicId);

        /// <summary>
        /// 删除一条 主题收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<Result> Remove(Int64 userId, Int64 topicId);
    }
}

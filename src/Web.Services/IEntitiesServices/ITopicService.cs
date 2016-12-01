using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace Web.Services.IEntitiesServices
{
    public interface ITopicService
    {
        /// <summary>
        /// 根据主题ID，获取主题
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        Task<Topic> GetTopicById(Int64 topicId);

        /// <summary>
        /// 根据查询关键词，获取主题数量
        /// </summary>
        /// <param name="queryString">查询关键词</param>
        /// <returns></returns>
        Task<Int32> GetCountByQuery(string queryString);

        /// <summary>
        /// 根据查询关键词，获取主题列表
        /// </summary>
        /// <param name="queryString">查询关键词</param>
        /// <returns></returns>
        Task<PagingModel<Topic>> GetTopicsByQuery(string queryString, Int32 pageSize, Int32 pageIndex = 1);

        /// <summary>
        /// 更新主题的最后回复信息
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        Task<Result> UpdateLastReply(Int64 topicId, Int64 replyId);

        /// <summary>
        /// 新增一条主题
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="tab">分类</param>
        /// <param name="authorId">作者ID</param>
        /// <returns></returns>
        Task<Result> AddTopic(string title, string content, string tab, Int64 authorId);
    }
}

using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace Web.Services.IEntitiesServices
{
    public interface IReplyService
    {
        /// <summary>
        /// 获取一条回复消息
        /// </summary>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        Task<Reply> GetReplyById(Int64 replyId);

        /// <summary>
        /// 根据主题ID，获取回复列表
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        Task<PagingModel<Reply>> GetRepliesByTopicId(Int64 topicId, Int32 pageSize, Int32 pageIndex = 1);

        /// <summary>
        /// 创建并保存一条回复消息
        /// </summary>
        /// <param name="content">回复内容</param>
        /// <param name="topicId">主题ID</param>
        /// <param name="authorId">回复作者</param>
        /// <param name="replyId">回复ID，当二级回复时设定该值</param>
        /// <returns></returns>
        Task<Result> AddReply(string content, Int64 topicId, Int64 authorId, Int64? replyId);

        /// <summary>
        /// 根据TopicId获取到最新的一条回复
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<Reply> GetLastReplyByTopicId(Int64 topicId);

        /// <summary>
        /// 根据作者ID获取回复消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IQueryable<Reply>> GetRpliesByAuthorId(Int64 userId);

        /// <summary>
        /// 根据作者ID，获取回复数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Int32> GetCountByAuthorId(Int64 userId);
    }
}

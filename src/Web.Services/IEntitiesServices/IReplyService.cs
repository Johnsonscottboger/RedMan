using RedMan.Model.Entities;
using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    public interface IReplyService
    {
        /// <summary>
        /// 获取一条回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Reply> GetReplyById(Int64 id);

        /// <summary>
        /// 获取主题下的回复
        /// </summary>
        /// <param name="subId">主题ID</param>
        /// <returns></returns>
        Task<PagingModel<Reply>> GetRepliesBySubId(Int64 subId, Int32 pageSize, Int32 pageIndex = 1);

        /// <summary>
        /// 获取用户的所有回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<PagingModel<Reply>> GetRepliesByUserId(Int64 userId, Int32 pageSize, Int32 pageIndex = 1);

        /// <summary>
        /// 修改回复
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Task<Result> UpdateReply(Reply reply);

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteReply(Int64 id);

        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        Task<Result> LikeReply(Int64 replyId);

        /// <summary>
        /// 回复一条回复
        /// </summary>
        /// <param name="replyId">被回复的ID</param>
        /// <param name="reply">回复实体</param>
        /// <returns></returns>
        Task<Result> ReplyToReply(Int64 replyId, Reply reply);

        /// <summary>
        /// 获取用户收到的回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        Task<PagingModel<Reply>> GetRecivedRepliesByUserId(Int64 userId, Int32 pageSize, Int32 pageIndex = 1);
    }
}

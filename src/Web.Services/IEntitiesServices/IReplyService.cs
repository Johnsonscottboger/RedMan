using RedMan.Model.Entities;
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
        Reply GetReplyById(Int64 id);

        /// <summary>
        /// 获取主题下的回复
        /// </summary>
        /// <param name="subId">主题ID</param>
        /// <returns></returns>
        List<Reply> GetRepliesBySubId(Int64 subId);

        /// <summary>
        /// 获取用户的所有回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        List<Reply> GetRepliesByUserId(Int64 userId);

        /// <summary>
        /// 修改回复
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Result UpdateReply(Int64 id);

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result DeleteReply(Int64 id);

        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        Result LikeReply(Int64 userId, Int64 replyId);

        /// <summary>
        /// 回复一条回复
        /// </summary>
        /// <param name="replyId">被回复的ID</param>
        /// <param name="reply">回复实体</param>
        /// <returns></returns>
        Result ReplyToReply(Int64 replyId, Reply reply);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;

namespace Web.Services.IEntitiesServices
{
    public interface IMessageService
    {
        /// <summary>
        ///根据用户ID，获取未读消息的数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<Int32> GetMessageUnReadCountByUserId(Int64 userId);

        /// <summary>
        /// 根据消息Id获取消息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns></returns>
        Task<Message> GetMessageById(Int64 messageId);

        /// <summary>
        /// 根据用户ID获取已读消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<IQueryable<Message>> GetMessageReadByUserId(Int64 userId);

        /// <summary>
        /// 根据用户ID获取未读消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<IQueryable<Message>> GetMessageUnReadByUserId(Int64 userId);

        /// <summary>
        /// 将消息标记为已读
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        Task<Result> UpdateMessageToRead(Int64 userId, Message message);

        /// <summary>
        /// 将消息标记为已读
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="messages">未读消息列表</param>
        /// <returns></returns>
        Task<Result> UpdateMessageToRead(Int64 userId, IEnumerable<Message> messages);
    }
}

using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices {
    public class MessageService : IMessageService
    {
        private readonly MyContext _context;
        private readonly IRepository<Message> _messageRepo;

        public MessageService(MyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._messageRepo = new Repository<Message>(context);
        }

        /// <summary>
        /// 根据消息ID，获取消息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns></returns>
        public async Task<Message> GetMessageById(Int64 messageId)
        {
            try
            {
                return await _messageRepo.FindAsync(p => p.MessageId == messageId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，获取已读消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<IQueryable<Message>> GetMessageReadByUserId(Int64 userId)
        {
            try
            {
                return await _messageRepo.FindAllDelayAsync(p => p.MessageId == userId && p.Has_Read);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，获取未读消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<IQueryable<Message>> GetMessageUnReadByUserId(Int64 userId)
        {
            try
            {
                return await _messageRepo.FindAllDelayAsync(p => p.Master_Id == userId && !p.Has_Read);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，获取未读消息数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetMessageUnReadCountByUserId(Int64 userId)
        {
            try
            {

                return await _messageRepo.CountAsync(p => p.Master_Id == userId && !p.Has_Read);
            }
            catch (Exception ex)
            {
                return default(int);
            }
        }

        /// <summary>
        /// 更新消息列表为已读
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public async Task<Result> UpdateMessageToRead(Int64 userId, IEnumerable<Message> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            if (messages.Count() == 0)
                return new Result() { Code = 200, Success = true, Message = "未更新任何消息" };
            foreach (var item in messages)
            {
                item.Has_Read = true;
            }
            try
            {
                var IsSuccess = await _messageRepo.UpdateRangeAsync(messages);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "已全部标记为已读" };
                else
                    return new Result() { Code = -200, Success = false, Message = "出现异常，请稍后重试" };
            }
            catch (Exception ex)
            {
                return new Result() { Code = -200, Success = false, Message = "出现异常，请稍后重试" };
            }
        }

        /// <summary>
        /// 更新消息为已读
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task<Result> UpdateMessageToRead(Int64 userId, Message message)
        {
            message.Has_Read = true;
            try
            {
                var IsSuccess = await _messageRepo.UpdateAsync(message);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "" };
                else
                    return new Result() { Code = -200, Success = false, Message = "出现异常" };
            }
            catch(Exception ex)
            {
                return new Result() { Code = -200, Success = false, Message = "出现异常" };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class MessageService : IMessageService
    {
        public Message GetMessageById(long messageId)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessageReadByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessageUnReadByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public int GetMessageUnReadCountByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public Result UpdateMessageToRead(long userId, IEnumerable<Message> messages)
        {
            throw new NotImplementedException();
        }

        public Result UpdateMessageToRead(long userId, Message message)
        {
            throw new NotImplementedException();
        }
    }
}

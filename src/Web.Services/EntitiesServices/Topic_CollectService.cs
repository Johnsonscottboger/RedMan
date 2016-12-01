using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class Topic_CollectService : ITopic_CollectService
    {
        public Task<Result> AddTopicCollect(long userId, long topicId)
        {
            throw new NotImplementedException();
        }

        public Task<TopicCollect> GetTopicCollect(long userId, long topicId)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TopicCollect>> GetTopicCollectsByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Remove(long userId, long topicId)
        {
            throw new NotImplementedException();
        }
    }
}

using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices {
    public class Topic_CollectService : ITopic_CollectService
    {
        private readonly MyContext _context;
        private readonly IRepository<TopicCollect> _topicCollectRepo;

        public Topic_CollectService(MyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicCollectRepo = new Repository<TopicCollect>(context);
        }

        /// <summary>
        /// 添加话题收藏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topicId">话题ID</param>
        /// <returns></returns>
        public async Task<Result> AddTopicCollect(Int64 userId, Int64 topicId)
        {
            var topicCollect = new TopicCollect()
            {
                UserId = userId,
                TopicId = topicId,
                CreateDateTime = DateTime.Now
            };
            try
            {
                var IsSuccess = await _topicCollectRepo.AddAsync(topicCollect);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "收藏成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "收藏失败，未知错误" };
            }
            catch (Exception)
            {
                return new Result() { Code = -200, Success = false, Message = "收藏失败，服务器内部错误" };
            }
        }

        /// <summary>
        /// 根据用户ID，话题ID，获取话题收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<TopicCollect> GetTopicCollect(Int64 userId, Int64 topicId)
        {
            try
            {
                return await _topicCollectRepo.FindAsync(p => p.UserId == userId && p.TopicId == topicId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，获取所有收藏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<IQueryable<TopicCollect>> GetTopicCollectsByUserId(Int64 userId)
        {
            try
            {
                return await _topicCollectRepo.FindAllDelayAsync(p => p.UserId == userId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，话题ID， 移除收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<Result> Remove(Int64 userId, Int64 topicId)
        {
            try
            {
                var IsSuccess = await _topicCollectRepo.DeleteAsync(p => p.UserId == userId && p.TopicId == topicId);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "移除收藏成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "移除收藏失败，未知错误" };
            }
            catch (Exception)
            {
                return new Result() { Code = -200, Success = false, Message = "移除收藏失败，服务器内部错误" };
            }
        }
    }
}

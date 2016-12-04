using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Model.Paging;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices {
    public class TopicService:ITopicService
    {
        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Reply> _replyRepo;

        public TopicService(MyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicRepo = new Repository<Topic>(context);
            this._replyRepo = new Repository<Reply>(context);
        }

        /// <summary>
        /// 添加话题
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="tab">分类</param>
        /// <param name="authorId">作者ID</param>
        /// <returns></returns>
        public async Task<Result> AddTopic(string title, string content, string tab, Int64 authorId)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            var topic = new Topic()
            {
                Title = title,
                Content = content,
                Tap = tab,
                Author_Id = authorId
            };
            try
            {
                var IsSuccess = await _topicRepo.AddAsync(topic);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "发布成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "发布失败，未知错误" };
            }
            catch (Exception)
            {
                return new Result() { Code = -200, Success = false, Message = "发布失败，服务器内部错误" };
            }
        }

        /// <summary>
        /// 根据关键字，获取相关主题的数量
        /// </summary>
        /// <param name="queryString">查询关键字</param>
        /// <returns></returns>
        public async Task<int> GetCountByQuery(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
                throw new ArgumentNullException(nameof(queryString));
            try
            {
                return await _topicRepo.CountAsync(p => p.Title.Contains(queryString) || p.Content.Contains(queryString));
            }
            catch (Exception)
            {
                return default(int);
            }
        }

        /// <summary>
        /// 根据ID，获取话题
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        public async Task<Topic> GetTopicById(long topicId)
        {
            try
            {
                return await _topicRepo.FindAsync(p => p.TopicId == topicId);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// 根据查询关键字，获取相关主题列表
        /// </summary>
        /// <param name="queryString">查询关键字</param>
        /// <returns></returns>
        public async Task<PagingModel<Topic>> GetTopicsByQuery(string queryString,Int32 pageSize,Int32 pageIndex=1)
        {
            if (string.IsNullOrEmpty(queryString))
                throw new ArgumentNullException(nameof(queryString));
            var pagingModel = new PagingModel<Topic>()
            {
                ModelList = new List<Topic>(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize
                }
            };
            try
            {
                return await _topicRepo.FindPagingAsync(p => p.Title.Contains(queryString) || p.Content.Contains(queryString), pagingModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新最新回复
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        public async Task<Result> UpdateLastReply(long topicId, long replyId)
        {
            try
            {
                var lastReply = await _replyRepo.FindAsync(p => p.ReplyId == replyId);
                if (lastReply == null)
                    return new Result() { Code = -200, Success = false, Message = $"未找到ID为{replyId}的回复" };
                var topic = await _topicRepo.FindAsync(p => p.TopicId == topicId);
                if (topic == null)
                    return new Result() { Code = -200, Success = false, Message = $"未找到ID为{topicId}的回复" };
                topic.Last_Reply_Id = lastReply.ReplyId;
                topic.Last_ReplyDateTime = DateTime.Now;
                topic.Reply_Count += 1;

                //保存修改
                var IsSuccess = await _topicRepo.UpdateAsync(topic);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "修改成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "修改失败，未知错误" };
            }
            catch (Exception)
            {
                return new Result() { Code = -200, Success = false, Message = "修改失败,服务器内部错误" };
            }
        }

        public async Task<IEnumerable<Topic>> GetTopicsInCount(Expression<Func<Topic,Boolean>> predicate,Int32 count) {
            return await _topicRepo.FindTopAsync(count,predicate,p => p.CreateDateTime);
        }
    }
}

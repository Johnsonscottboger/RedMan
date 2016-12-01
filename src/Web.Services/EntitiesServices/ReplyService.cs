using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.Model.Context;
using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Entities;
using Web.Services.IEntitiesServices;

namespace Web.Services.EntitiesServices
{
    public class ReplyService : IReplyService
    {
        private readonly MyContext _context;
        private readonly IRepository<Reply> _replyRepo;
        public ReplyService(MyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._replyRepo = new Repository<Reply>(context);
        }

        /// <summary>
        /// 根据用户ID，获取回复的数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<int> GetCountByAuthorId(long userId)
        {
            try
            {
                return await _replyRepo.CountAsync(p => p.Author_Id == userId && !p.Deleted);
            }
            catch(Exception ex)
            {
                return default(int);
            }
        }

        /// <summary>
        /// 根据主题ID，获取最新的一条回复
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<Reply> GetLastReplyByTopicId(long topicId)
        {
            try
            {
                return await _replyRepo.FindAsync(p => p.Topic_Id == topicId && !p.Deleted);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据主题ID，获取所有回复
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        public async Task<PagingModel<Reply>> GetRepliesByTopicId(long topicId,Int32 pageSize,Int32 pageIndex=1)
        {
            try
            {
                PagingModel<Reply> pagingModel = new PagingModel<Reply>()
                {
                    ModelList = new List<Reply>(),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = pageIndex,
                        ItemsPerPage = pageSize,
                    }
                };

                return await _replyRepo.FindPagingOrderByAsync(p => p.Topic_Id == topicId, p => p.CreateDateTime, pagingModel);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据回复ID，获取回复
        /// </summary>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        public async Task<Reply> GetReplyById(long replyId)
        {
            try
            {
                return await _replyRepo.FindAsync(p => p.ReplyId == replyId);
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 根据用户ID，获取回复列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<IQueryable<Reply>> GetRpliesByAuthorId(long userId)
        {
            try
            {
                return await _replyRepo.FindAllDelayAsync(p => p.Author_Id == userId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="topicId">主题ID</param>
        /// <param name="authorId">用户ID</param>
        /// <param name="replyId">上级回复ID</param>
        /// <returns></returns>
        public async Task<Result> AddReply(string content, long topicId, long authorId, long? replyId)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));
            var reply = new Reply()
            {
                Content = content,
                Topic_Id = topicId,
                Author_Id = authorId,
                Reply_Id = replyId,
                CreateDateTime=DateTime.Now,
                UpdateDateTime=DateTime.Now,
                Content_Is_Html=false
            };

            try
            {
                var IsSuccess = await _replyRepo.AddAsync(reply);
                if (IsSuccess)
                    return new Result() { Code = 200, Success = true, Message = "回复成功" };
                else
                    return new Result() { Code = -200, Success = false, Message = "回复失败，未知错误" };
            }
            catch(Exception ex)
            {
                return new Result() { Code = 200, Success = false, Message = ex.Message };
            }
        }
    }
}

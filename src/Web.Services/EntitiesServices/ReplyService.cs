using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedMan.Model.Entities;
using Web.Services.IEntitiesServices;
using RedMan.Model.Context;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.Model.Paging;
using static Web.Services.Common.Common;

namespace Web.Services.EntitiesServices
{
    public class ReplyService : IReplyService
    {
        private readonly MyContext context;
        private readonly IRepository<User> _user;
        private readonly IRepository<Subject> _subject;
        private readonly IRepository<Reply> _reply;

        public ReplyService(MyContext context)
        {
            this.context = context;
            this._user = new Repository<User>(context);
            this._subject = new Repository<Subject>(context);
            this._reply = new Repository<Reply>(context);
        }

        /// <summary>
        /// 获取一条回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Reply> GetReplyById(Int64 id)
        {
            return await _reply.FindAsync(p => p.ReplyId == id);
        }

        /// <summary>
        /// 获取主题下的回复
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public async Task<PagingModel<Reply>> GetRepliesBySubId(Int64 subId, Int32 pageSize,Int32 pageIndex=1)
        {
            var subject = await _subject.FindAsync(p => p.SubjectId == subId);
            if (subject == null)
                return null;
            var result = subject.Replies;
            var pagingModel = new PagingModel<Reply>()
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = result.Count
                }
            };
            pagingModel.ModelList = result.Skip((Int32)(pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return pagingModel;
        }

        
        /// <summary>
        /// 修改回复
        /// </summary>
        /// <param name="id">回复实体</param>
        /// <returns></returns>
        public async Task<Result> UpdateReply(Reply reply)
        {
            if (reply == null)
                throw new ArgumentNullException(nameof(reply), "reply不能为空");
            var IsSuccess = await _reply.UpdateAsync(reply);
            if (IsSuccess)
                return Success("修改成功");
            else
                return Fail("修改失败");
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="id">回复ID</param>
        /// <returns></returns>
        public async Task<Result> DeleteReply(Int64 id)
        {
            var reply = await _reply.FindAsync(p => p.ReplyId == id);
            if (reply == null)
                return Fail("回复未找到");
            reply.IsDelete = true;
            var IsSuccess = await _reply.UpdateAsync(reply);
            if (IsSuccess)
                return Success("删除成功");
            else
                return Fail("删除失败");
        }

        /// <summary>
        /// 赞一条回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="replyId">回复ID</param>
        /// <returns></returns>
        public async Task<Result> LikeReply(Int64 replyId)
        {
            //是否做并发
            var reply = await _reply.FindAsync(p => p.ReplyId == replyId);
            if (reply == null)
                return Fail("回复不存在");
            reply.LikeNumber += 1;
            var IsSuccess = await _reply.UpdateAsync(reply);
            if (IsSuccess)
                return Success("赞 成功");
            else
                return Fail("赞 失败");
        }

        /// <summary>
        /// 回复一条回复
        /// </summary>
        /// <param name="replyId">回复ID</param>
        /// <param name="reply">新的回复实体</param>
        /// <returns></returns>
        public async Task<Result> ReplyToReply(Int64 replyId, Reply reply)
        {
            if (reply == null)
                throw new ArgumentNullException(nameof(reply), "回复不能为空");
            var replied = await _reply.FindAsync(p => p.ReplyId == replyId);
            if (replied == null)
                return Fail("回复不存在");
            replied.Replies.Add(reply);
            //保存
            var IsSuccess = await context.SaveChangesAsync() > 0;
            if (IsSuccess)
                return Success("回复成功");
            else
                return Fail("回复失败");
        }

        /// <summary>
        /// 获取用户发表的所有回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        public async Task<PagingModel<Reply>> GetRepliesByUserId(Int64 userId, int pageSize, int pageIndex = 1)
        {
            var user = await _user.FindAsync(p => p.UserId == userId);
            if (user == null)
                return null;
            var result = user.PubReplies;
            var pagingModel = new PagingModel<Reply>()
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = result.Count
                }
            };
            pagingModel.ModelList = result.Skip((Int32)(pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return pagingModel;
        }

        /// <summary>
        /// 获取用户收到的回复
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        public async Task<PagingModel<Reply>> GetRecivedRepliesByUserId(Int64 userId,Int32 pageSize,Int32 pageIndex=1)
        {
            var user = await _user.FindAsync(p => p.UserId == userId);
            if (user == null)
                return null;
            var result = user.ReceivedReplies;
            var pagingModel = new PagingModel<Reply>()
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = result.Count
                }
            };
            pagingModel.ModelList = result.Skip((Int32)(pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return pagingModel;
        }
    }
}

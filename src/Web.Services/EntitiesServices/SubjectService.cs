using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedMan.Model.Entities;
using RedMan.Model.Paging;
using Web.Services.IEntitiesServices;
using RedMan.Model.Context;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using static Web.Services.Common.Common;

namespace Web.Services.EntitiesServices
{
    public class SubjectService : ISubjectService
    {

        private readonly MyContext context;
        private readonly IRepository<User> _user;
        private readonly IRepository<Subject> _subject;
        private readonly IRepository<Reply> _reply;

        public SubjectService(MyContext context)
        {
            this.context = context;
            this._user = new Repository<User>(context);
            this._subject = new Repository<Subject>(context);
        }

        /// <summary>
        /// 添加主题
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="subject">主题</param>
        /// <returns></returns>
        public async Task<Subject> Add(User user, Subject subject)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "用户不能为空");
            if (subject == null)
                throw new ArgumentNullException(nameof(subject), "主题不能为空");
            var currentUser = await _user.FindAsync(p => p.UserId == user.UserId);
            if (currentUser == null)
                return null;
            //添加
            currentUser.Subjects.Add(subject);
            //添加成功
            if (await context.SaveChangesAsync() > 0)
                return subject;
            //添加失败
            else
                return null;
        }

        /// <summary>
        /// 修改主题
        /// </summary>
        /// <param name="subject">主题</param>
        /// <returns></returns>
        public async Task<Subject> UpdateSubject(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject), "主题不能为空");
            //修改成功
            if (await _subject.UpdateAsync(subject))
                return subject;
            //修改失败
            else
                return null;
            
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="id">主题ID</param>
        /// <returns></returns>
        public async Task<Result> DeleteSubject(User user, Int64 id)
        {
            var currentUser = await _user.FindAsync(p => p.UserId == user.UserId);
            if (currentUser == null)
                return null;
            var currrentSub = currentUser.Subjects.FirstOrDefault(p => p.SubjectId == id);
            if (currrentSub == null)
                return Fail("未找到主题");
            currrentSub.IsDelete = true;
            //保存成功
            if (await context.SaveChangesAsync() > 0)
                return Success("删除成功");
            else
                return Fail("删除失败");
        }

        /// <summary>
        /// 获取主题通过ID
        /// </summary>
        /// <param name="id">主题ID</param>
        /// <returns></returns>
        public async Task<Subject> GetSubjectById(Int64 id)
        {
            return await _subject.FindAsync(p => p.SubjectId == id);
        }

        /// <summary>
        /// 获取主题
        /// </summary>
        /// <returns></returns>
        public async Task<PagingModel<Subject>> GetAllSubject(Int32 pageIndex,Int32 pageSize)
        {
            var pagingModel = new PagingModel<Subject>()
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize
                }
            };
            //首先获取置顶主题
            await _subject.FindPagingOrderByAsync<DateTime>(p => p.IsTop == true, p=>p.PubDateTime, pagingModel);
            //获取主题，以时间倒序
            await _subject.FindPagingOrderByDescendingAsync<DateTime>(p => p.IsTop != true, p => p.PubDateTime, pagingModel);
            return pagingModel;
        }

        /// <summary>
        /// 收藏主题
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="subId">主题ID</param>
        /// <returns></returns>
        public async Task<Result> FavoritesSubject(User user, Int64 subId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "user不能为空！");
            var currentUser = await _user.FindAsync(p => p.UserId == user.UserId);
            if (currentUser == null)
                return Fail("当前用户未找到");
            var subject = await _subject.FindAsync(p => p.SubjectId == subId);
            if (subject == null)
                return Fail("当前主题未找到");
            currentUser.FavoriteSubjects.Add(subject);
            //保存
            var IsSuccess = await context.SaveChangesAsync() > 0;
            if (!IsSuccess)
                return Fail("收藏失败");
            return Success("加入收藏成功");
        }

        /// <summary>
        /// 回复主题
        /// 返回：
        ///     主题下的所有回复
        /// </summary>
        /// <param name="subId">主题ID</param>
        /// <param name="user">用户</param>
        /// <param name="reply">回复实体</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        public async Task<PagingModel<Reply>> ReplyToSubject(Int64 subId, User user, Reply reply,Int32 pageSize, Int32 pageIndex=1)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "用户不能为空");
            if (reply == null)
                throw new ArgumentNullException(nameof(reply), "回复不能为空");

            var subject = await _subject.FindAsync(p => p.SubjectId == subId);
            if (subject == null)
                return null;

            //添加
            subject.Replies.Add(reply);
            user.PubReplies.Add(reply);
            //保存
            var IsSuccess = await context.SaveChangesAsync() > 0;
            if (!IsSuccess)
                return null;

            //获取主题下的所有回复
            var result = subject.Replies;
            var pagingModel = new PagingModel<Reply>()
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems=result.Count   
                }
            };
            pagingModel.ModelList = result.Skip((int)(pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return pagingModel;
        }
    }
}

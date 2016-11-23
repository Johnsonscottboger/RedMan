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
            var topSubjects = await _subject.FindPagingAsync(p => p.IsTop == true,pagingModel);
            //获取主题，以时间倒序
            var subjects = await _subject.FindPagingAsync(p=>p.IsTop!=true,pagingModel);
            return pagingModel;
        }

        public Task<Result> FavoritesSubject(User user, long subId)
        {
            throw new NotImplementedException();
        }

        public Task<PagingModel<Reply>> Reply(long subId, User user, Reply reply)
        {
            throw new NotImplementedException();
        }
    }
}

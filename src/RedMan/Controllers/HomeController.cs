using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Model.Paging;

namespace RedMan.Controllers
{
    public class HomeController :Controller
    {

        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<User> _userRepo;

        public HomeController(MyContext context)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
        }

        public async Task<IActionResult> Index(int tab,int pageIndex = 1)
        {

            var pageSize = GetPageSize("Home/Index") ?? 40;
            var pagingModel = new PagingModel<Topic>()
            {
                ModelList = new List<Topic>(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize
                }
            };

            if(tab == 0)
            {
                pagingModel = await _topicRepo.FindPagingOrderByDescendingAsync(p => !p.Deleted,p => p.CreateDateTime,pagingModel);
            }
            else
            {
                pagingModel = await _topicRepo.FindPagingOrderByDescendingAsync(p => !p.Deleted && p.Type == tab,p => p.CreateDateTime,pagingModel);
            }
            pagingModel.ModelList.OrderBy(p => p.Top);
            var pagingViewModel = new PagingModel<IndexTopicsViewModel>();
            pagingViewModel.ModelList = new List<IndexTopicsViewModel>();
            pagingViewModel.PagingInfo = pagingModel.PagingInfo;

            var topicUsers = new List<User>();
            pagingModel.ModelList.ForEach(item =>
            {
                topicUsers.AddRange(_userRepo.FindAll(p => p.UserId == item.Author_Id || p.UserId == item.Last_Reply_UserId).Distinct());
            });

            pagingModel.ModelList.ForEach(item =>
            {
                pagingViewModel.ModelList.Add(new IndexTopicsViewModel()
                {
                    Tab = (TopicTapViewModel)tab,
                    Type = (TopicTypeViewModel)item.Type,
                    UserAvatarUrl = topicUsers.Where(p => p.UserId == item.Author_Id).FirstOrDefault().Avatar,
                    UserName = topicUsers.Where(p => p.UserId == item.Author_Id).FirstOrDefault().Name,
                    RepliesCount = item.Reply_Count,
                    VisitsCount = item.Visit_Count,
                    LastReplyUrl = item.Last_Reply_Id == null ? null : Url.Content($"/Topic/{item.TopicId}/#{item.Last_Reply_Id}"),
                    LastReplyUserAvatarUrl = item.Last_Reply_UserId == null ? null : topicUsers.Where(p => p.UserId == item.Last_Reply_UserId).FirstOrDefault().Avatar,
                    LastReplyDateTime = item.Last_ReplyDateTime.ToString(),
                    TopicId = item.TopicId,
                    Title = item.Title
                });
            });
            pagingModel.ModelList.OrderByDescending(p => p.CreateDateTime);
            ViewData["tab"] = tab;
            return View(pagingViewModel);
        }

        #region 附加方法

        /// <summary>
        /// 获取分页大小
        /// </summary>
        /// <param name="wherePageSize">分页位置</param>
        /// <returns></returns>
        private int? GetPageSize(string wherePageSize)
        {
            var directory = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile($"{directory}/appsettings.json",true,true).Build();
            var pagingConfig = configuration.GetSection("Paging");
            var pageSize = pagingConfig.GetValue(typeof(int),wherePageSize);
            return (int?)pageSize;
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Model.Paging;

namespace RedMan.Controllers {
    public class HomeController :Controller {

        private readonly MyContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<User> _userRepo;

        public HomeController(MyContext context) {
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
        }

        public IActionResult Index(int tab,int pageIndex = 1)  {

            var pageSize = GetPageSize("Home/Index") ?? 40;
            var pagingModel = new PagingModel<Topic>() {
                ModelList = new List<Topic>(),
                PagingInfo = new PagingInfo() {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize
                }
            };

            if(tab == 0) {
                pagingModel = _topicRepo.FindPagingOrderByDescending(p => !p.Deleted,p => p.CreateDateTime,pagingModel);
            }
            else {
                pagingModel = _topicRepo.FindPagingOrderByDescending(p => !p.Deleted && p.Type == tab,p => p.CreateDateTime,pagingModel);
            }
            pagingModel.ModelList.OrderBy(p => p.Top);
            var pagingViewModel = new PagingModel<IndexTopicsViewModel>();
            pagingViewModel.ModelList = new List<IndexTopicsViewModel>();
            pagingViewModel.PagingInfo = pagingModel.PagingInfo;

            var topicUsers = new List<User>();
            pagingModel.ModelList.AsParallel().ForAll(item => {
                topicUsers.Add(_userRepo.Find(p => p.UserId == item.Author_Id));
            });

            pagingModel.ModelList.AsParallel().ForAll(item => {
                pagingViewModel.ModelList.Add(new IndexTopicsViewModel() {
                    Tab = (TopicTapViewModel)tab,
                    Type = (TopicTypeViewModel)item.Type,
                    //UserAvatarUrl = Url.Action("AvatarUrl","User",new { userId = item.Author_Id }),
                    UserAvatarUrl= "~/img/8791709.png",
                    UserName = topicUsers.Where(p => p.UserId == item.Author_Id).FirstOrDefault().Name,
                    RepliesCount = item.Reply_Count,
                    VisitsCount = item.Visit_Count,
                    LastReplyUrl = Url.Content($"/Topic/{item.TopicId}/#{item.Last_Reply_Id}"),
                    //LastReplyUserAvatarUrl = Url.Action("AvatarUrl","User",new { userId = item.Last_Reply_UserId }),
                    LastReplyUserAvatarUrl= "/img/8791709.png",
                    LastReplyDateTime = item.Last_ReplyDateTime.ToString(),
                    TopicId = item.TopicId,
                    Title = item.Title
                });
            });

            ViewData["tab"] = tab;
            return View(pagingViewModel);
        }

        #region 附加方法

        /// <summary>
        /// 获取分页大小
        /// </summary>
        /// <param name="wherePageSize">分页位置</param>
        /// <returns></returns>
        private int? GetPageSize(string wherePageSize) {
            var directory = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile($"{directory}/appsettings.json",true,true).Build();
            var pagingConfig = configuration.GetSection("Paging");
            var pageSize = pagingConfig.GetValue(typeof(int),wherePageSize);
            return (int?)pageSize;
        }

        #endregion
    }
}

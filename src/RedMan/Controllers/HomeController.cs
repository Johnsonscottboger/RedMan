using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedMan.DataAccess.IRepository;
using RedMan.DataAccess.Repository;
using RedMan.ViewModel;
using Web.DataAccess.ExpressionExtend;
using Web.Model.Context;
using Web.Model.Entities;
using Web.Model.Paging;

namespace RedMan.Controllers
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    public class HomeController : Controller
    {
        #region - Private -
        private static readonly Lazy<int> s_defaultPageSizeLazy = new Lazy<int>(() =>
        {
            try
            {
                var directory = Directory.GetCurrentDirectory();
                var configuration = new ConfigurationBuilder()
                                       .AddJsonFile($"{directory}/appsettings.json", true, true)
                                       .Build();
                var pagingConfig = configuration.GetSection("Paging");
                var pageSize = pagingConfig.GetValue(typeof(int), "Home/Index");
                return (int)pageSize;
            }
            catch
            {
                return 40;
            }
        }, true);

        private readonly ModelContext _context;
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<User> _userRepo;
        #endregion

        /// <summary>
        /// 获取默认分页大小
        /// </summary>
        private int DefaultPageSize
        {
            get { return s_defaultPageSizeLazy.Value; }
        }

        /// <summary>
        /// 初始化主页控制器<see cref="HomeController"/>实例
        /// </summary>
        /// <param name="context">指定的模型上下文<see cref="ModelContext"/>实例</param>
        public HomeController(ModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this._context = context;
            this._topicRepo = new Repository<Topic>(context);
            this._userRepo = new Repository<User>(context);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="tab">指定首页打开的标签</param>
        /// <param name="keyWord">指定查询的关键字</param>
        /// <param name="pageIndex">指定的页码</param>
        /// <returns>首页视图</returns>
        [Route("")]
        [Route("Home/Index/{tab}")]
        [Route("Home/Index/{tab}/{q}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10, VaryByQueryKeys = new string[] { "pageIndex" })]
        public async Task<IActionResult> Index(int tab, string keyWord = null, int pageIndex = 1)
        {
            var pageSize = this.DefaultPageSize;
            var pagingModel = new PagingModel<Topic>()
            {
                ModelList = new List<Topic>(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize
                }
            };

            Expression<Func<Topic, bool>> predicate = p => !p.Deleted;
            switch (tab)
            {
                case 0:
                    break;
                case 1:
                    predicate = predicate.And(p => p.Good);
                    break;
                default:
                    predicate = predicate.And(p => p.Type == tab);
                    break;
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                ViewData["q"] = keyWord;
                if (tab == 0)
                    predicate = predicate.And(p => (p.Title.Contains(keyWord) || p.Content.Contains(keyWord)));
            }
            pagingModel = await this._topicRepo.FindPagingOrderByDescendingAsync(predicate, p => p.CreateDateTime, pagingModel);
            pagingModel.ModelList = pagingModel.ModelList.OrderByDescending(p => p.Top).ToList();
            var pagingViewModel = GetViewModel(pagingModel, tab);
            ViewData["tab"] = tab;
            return View(pagingViewModel);
        }

        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns>返回系统提示错误</returns>
        [Route("/Error")]
        public IActionResult Error()
        {
            return Content("<h1>Sorry,服务器内部出错了!</h1>", "text/html");
        }

        #region - Private Method -
        
        /// <summary>
        /// 根据指定的话题, 查找话题对应的用户列表, 返回视图模型<see cref="IndexTopicsViewModel"/>实例
        /// </summary>
        /// <param name="pagingModel">指定的话题分页列表</param>
        /// <param name="tab">指定当前首页打开的标签</param>
        /// <returns>话题及其对应的用户信息分页列表</returns>
        private PagingModel<IndexTopicsViewModel> GetViewModel(PagingModel<Topic> pagingModel, int tab = 0)
        {
            var pagingViewModel = new PagingModel<IndexTopicsViewModel>
            {
                ModelList = new List<IndexTopicsViewModel>(),
                PagingInfo = pagingModel.PagingInfo
            };

            //根据话题,查找相关用户
            var topicAuthors = this._userRepo.Join(inner: pagingModel.ModelList,
                                                    outerKeySelector: user => user.UserId,
                                                    innerKeySelector: topic => topic.Author_Id,
                                                    resultSelector: (user, topic) => user).ToList();
            var topicLastReplyUsers = this._userRepo.Join(inner: pagingModel.ModelList,
                                                          outerKeySelector: user => user.UserId,
                                                          innerKeySelector: topic => topic.Last_Reply_UserId,
                                                          resultSelector: (user, topic) => user).ToList();
            var topicUsers = topicAuthors.Concat(topicLastReplyUsers);

            pagingModel.ModelList.ForEach(item =>
            {
                pagingViewModel.ModelList.Add(new IndexTopicsViewModel()
                {
                    Tab = (TopicTapViewModel)tab,
                    Type = (TopicTypeViewModel)item.Type,
                    UserAvatarUrl = topicUsers.Where(p => p.UserId == item.Author_Id).FirstOrDefault().Avatar,
                    UserId = item.Author_Id,
                    UserName = topicUsers.Where(p => p.UserId == item.Author_Id).FirstOrDefault().Name,
                    RepliesCount = item.Reply_Count,
                    VisitsCount = item.Visit_Count,
                    LastReplyUrl = item.Last_Reply_Id == null ? null : Url.Content($"/Topic/Index/{item.TopicId}/#{item.Last_Reply_Id}"),
                    LastReplyUserAvatarUrl = item.Last_Reply_UserId == null ? null : topicUsers.Where(p => p.UserId == item.Last_Reply_UserId).FirstOrDefault().Avatar,
                    LastReplyDateTime = item.Last_ReplyDateTime.ToString(),
                    TopicId = item.TopicId,
                    Title = item.Title,
                    Top = item.Top,
                    Good = item.Good,
                    CreateDateTime = item.CreateDateTime.ToString()
                });
            });
            return pagingViewModel;
        }

        #endregion
    }
}

using Microsoft.Extensions.DependencyInjection;
using Web.DataAccess.IRepository;
using Web.DataAccess.Repository;
using Web.Model.Entities;
using Web.Services.EntitiesServices;

namespace RedMan.Extensions
{
    /// <summary>
    /// 定义<see cref="IServiceCollection"/>实例的扩展方法
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 向指定的服务集合中添加Identity服务
        /// </summary>
        /// <param name="services">指定的<see cref="IServiceCollection"/>实例</param>
        /// <returns>修改后的服务集合实例</returns>
        public static IServiceCollection AddMyIdentity(this IServiceCollection services)
        {
            services.AddTransient<IIdentityRepository<User>, IdentityRepository<User>>();
            services.AddTransient<IdentityService>();
            return services;
        }
    }
}

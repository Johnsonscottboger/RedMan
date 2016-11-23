using Microsoft.Extensions.DependencyInjection;
using RedMan.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DataAccess.IRepository;
using Web.DataAccess.Repository;
using Web.Services.EntitiesServices;

namespace RedMan.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMyIdentity(this IServiceCollection services)
        {
            services.AddTransient<IIdentityRepository<User>, IdentityRepository<User>>();
            services.AddTransient<IdentityService>();
            return services;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    public interface IIdentityService
    {
        Task<ClaimsPrincipal> CheckUserAsync(string email, string password);

        Task<IdentityResult> RegisterAsync(string email, string password);
    }
}

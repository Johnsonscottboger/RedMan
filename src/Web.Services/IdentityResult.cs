using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Services
{
    public class IdentityResult
    {
        public bool IsSuccess { get; }
        public string ErrorString { get; }
        public ClaimsPrincipal User { get; }

        public IdentityResult(string error)
        {
            IsSuccess = false;
            ErrorString = error;
        }

        public IdentityResult(ClaimsPrincipal user)
        {
            IsSuccess = true;
            User = user;
        }
    }
}

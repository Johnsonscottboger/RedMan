using System.Security.Claims;

namespace Web.Services {
    public class IdentityResult
    {
        public bool Success { get; }
        public string ErrorString { get; }
        public ClaimsPrincipal User { get; }

        public IdentityResult(string error)
        {
            Success = false;
            ErrorString = error;
        }

        public IdentityResult(ClaimsPrincipal user)
        {
            Success = true;
            User = user;
        }
    }
}

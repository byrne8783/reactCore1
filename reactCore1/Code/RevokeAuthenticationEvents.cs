using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
namespace ReactCore1
{
    public class RevokeAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IMemoryCache cache;
        private readonly ILogger logger;
        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal.Claims.First(c => c.Type == ClaimTypes.Name);

            if (cache.Get<bool>("revoke-" + userId.Value))
            {
                context.RejectPrincipal();
                cache.Remove("revoke-" + userId.Value);
                logger.LogDebug("Access has been revoked for: " + userId.Value + ".");
            }
            return Task.CompletedTask;
        }
        #region _________________________________________________________constructors
        public RevokeAuthenticationEvents(IMemoryCache cache, ILogger<RevokeAuthenticationEvents> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }
        #endregion
    }
}

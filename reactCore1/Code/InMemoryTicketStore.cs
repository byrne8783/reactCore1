using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace ReactCore1
{
    public class InMemoryTicketStore : ITicketStore
    {
        private readonly IMemoryCache cache;
        public Task RemoveAsync(string key)
        {
            cache.Remove(key);
            return Task.CompletedTask;
        }
        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var ticket = cache.Get<AuthenticationTicket>(key);
            return Task.FromResult(ticket);
        }
        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            cache.Set(key, ticket);
            return Task.CompletedTask;
        }
        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = ticket.Principal.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            cache.Set(key, ticket);
            return Task.FromResult(key);
        }
        #region ___________________________________________________________________constructors
        public InMemoryTicketStore(IMemoryCache cache)
        {
            this.cache = cache;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace ReactCore1
{
    public class ConfigureCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        private readonly ITicketStore ticketStore;
        public ConfigureCookieAuthenticationOptions(ITicketStore ticketStore)
        {
            this.ticketStore = ticketStore;
        }
        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            options.SessionStore = ticketStore;
        }
    }
}

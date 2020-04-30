using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using ReactCore1.Extensions;
using ReactCore1.Web;

namespace ReactCore1
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            services.AddScoped<IdentityDatabase>();
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddCustomStores();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogout = (context) =>
                    {
                        context.Response.StatusCode = 400;
                        return Task.CompletedTask;
                    };
                    // https://stackoverflow.com/questions/58222039/how-to-change-aspnetcore-identity-application-cookie-expiration
                    options.Cookie.Name = ".AspNetCore.reactCore1.Cookies"; // defaults to .AspNetCore.Cookies
                })
                ;
            services.AddScoped<RevokeAuthenticationEvents>();
            services.AddTransient<ITicketStore, InMemoryTicketStore>();
            services.AddMvc()  //options => options.Filters.Add(new AuthorizeFilter())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(b =>
                {
                    b.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    b.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebpackDevMiddleware(new Microsoft.AspNetCore.SpaServices.Webpack.WebpackDevMiddlewareOptions
                {
                    //ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                    HotModuleReplacementClientOptions = new Dictionary<string, string>()
                    {
                        { "reload", "true" }
                    },
                    HotModuleReplacement = true
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            IHttpContextAccessor httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContextExtensions.Configure(httpContextAccessor);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                          name: "default",
                          template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        #region _____________________________________________________________________constructors
        public Startup(IHostingEnvironment env, ILogger<Startup> ler)
        {
            hostingEnvironment = env;
            logger = ler;
        }
        #endregion
    }
}

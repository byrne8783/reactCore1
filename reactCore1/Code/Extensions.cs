using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReactCore1.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReactCore1.Extensions
{

    public enum CacheKey
    {
        Users 

    }

public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddCustomStores(this IdentityBuilder builder)
        {
            builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>().
                AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>()
                // .AddTransient<IUserClaimStore<ApplicationUser>,ApplicationUserClaimStore>()
                ;

            return builder;
        }

    }



    public static class MemoryCacheExtensions
    {
        public static string Tables(this Microsoft.Extensions.Caching.Memory.IMemoryCache cacheIn,CacheKey key)
        {
            switch (key) {
                case CacheKey.Users : return "UserTable";

                default: return string.Empty;



            }

        }


    }


    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Gets the raw target of an HTTP request.
        /// </summary>
        /// <returns>Raw target of an HTTP request</returns>
        /// <remarks>
        /// ASP.NET Core manipulates the HTTP request parameters exposed to pipeline
        /// components via the HttpRequest class. This extension method delivers an untainted
        /// request target. https://tools.ietf.org/html/rfc7230#section-5.3
        /// </remarks>
        public static string GetRawTarget(this HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            return httpRequestFeature.RawTarget;
        }
    }

    public static class HttpContextExtensions
    {
        private static IHttpContextAccessor HttpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public static Uri GetAbsoluteUri()
        {
            var request = HttpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder() {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// A util that uses regular expression to verify if a string is in valid email format.
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        /// </remarks>
        public static bool IsEmailFormat(this string strIn,int timeout = 400)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;
            try
            {
                string DomainMapper(Match match)
                {
                    IdnMapping idn = new IdnMapping();
                    string domainName = match.Groups[2].Value;
                    try
                    {
                        domainName = idn.GetAscii(domainName);
                    }
                    catch (ArgumentException)
                    {
                        return null;
                    }
                    return match.Groups[1].Value + domainName;
                }
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,             // convert Unicode domain names.
                                      RegexOptions.None, TimeSpan.FromMilliseconds(timeout / 2));
                if (string.IsNullOrEmpty(strIn))
                    return false;
                else
                    return Regex.IsMatch(strIn,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(timeout / 2));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

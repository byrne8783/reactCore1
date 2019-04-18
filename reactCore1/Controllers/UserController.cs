using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;


namespace ReactCore1.Web
{
    [Route("User/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region _____________________________________________________________Constructors


    public UserController(IdentityDatabase context
    ,UserManager<ApplicationUser> userManager
        ,SignInManager<ApplicationUser> signInManager
            ,IMemoryCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
            _signInManager = signInManager;
            this._cache = cache;
        }



    #endregion
    [HttpGet("[action]")]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            return Ok(new { CurrentUser = new ApplicationUser (_context.Users.Find(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value)) });
        }
        [AllowAnonymous]
        [HttpPost] // 
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginData)
        {
            ActionResult result;
            var aU = _userManager.FindByNameAsync(loginData.UserId).Result;
            var attempt = aU == null ? await _signInManager.CheckPasswordSignInAsync(aU, loginData.Password, lockoutOnFailure: false)
                                    : await _signInManager.PasswordSignInAsync(loginData.UserId, loginData.Password, false, lockoutOnFailure: false);
            if (!attempt.Succeeded && aU != null)
            {
                await _signInManager.SignInAsync(aU, true);
            }
            var cp = await _signInManager.CreateUserPrincipalAsync(aU);
            if (_signInManager.IsSignedIn(cp))
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, aU.Id)
                            ,new Claim("access_token", GetAccessToken(aU.Id))
                        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , new ClaimsPrincipal(claimsIdentity)
                    , new AuthenticationProperties());
                var x = $"{RouteData.Values["controller"]}.{RouteData.Values["action"]}";
                result = CreatedAtAction(x, new ResponseData<object>(x, new { UserId = aU.UserName, aU.Name }));
            }
            else
            {
                result = ValidationProblem(new ValidationProblemDetails() { Title = $"Login Failed for User {loginData?.UserId} " });
            }
            return result;
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            //        {
            //            new Claim(ClaimTypes.NameIdentifier, "Value1")
            //            ,new Claim(ClaimTypes.Role, "Value2")
            //        }, CookieAuthenticationDefaults.AuthenticationScheme)),
            //    new AuthenticationProperties
            //        {
            //            AllowRefresh = true     // Refreshing the authentication session should be allowed
            //         ,  ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15)    // The absolute time at which the authentication ticket expires
            //         ,  IsPersistent = true     // Whether the authentication session is persisted across 
            //                                    // multiple requests. When used with cookies, controls
            //                                    // whether the cookie's lifetime is absolute (matching the
            //                                    // lifetime of the authentication ticket) or session-based.
            //        ,  IssuedUtc = DateTimeOffset.UtcNow
            //        ,   RedirectUri = null      // The full path or absolute URI to be used as an http 
            //                                    // redirect response value.
            //        }
            //    );



            //           return new UnauthorizedResult();

            //var userId = Guid.NewGuid().ToString();
            //var claims = new List<Claim>
            //  {
            //    new Claim(ClaimTypes.Name, userId),
            //    new Claim("access_token", GetAccessToken(userId))   // stick the JWT bearer token into the list of claims
            //  };

            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var authProperties = new AuthenticationProperties();

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity),
            //    authProperties);


        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // the authentication scheme tells the signout which auth cookie to revoke
            return Ok();
        }
        public IActionResult Revoke()
        {
            var principal = HttpContext.User as ClaimsPrincipal;
            var userId = principal?.Claims.First(c => c.Type == ClaimTypes.Name);
            _cache.Set("revoke-" + userId.Value, true);

            return Ok();
        }






        #region ___________________________________________privates
        private readonly IdentityDatabase _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMemoryCache _cache;
        /// <summary>
        /// Return a JWT token
        /// </summary>
        /// <remarks>Rubbish - in real life use an asymmetric signing key with public and private keys.</remarks>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string GetAccessToken(string userId)
        {
            const string issuer = "localhost";
            const string audience = "localhost";

            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("sub", userId)
            });

            var bytes = Encoding.UTF8.GetBytes(userId);
            var key = new SymmetricSecurityKey(bytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(
                issuer, audience, identity, now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);

            return handler.WriteToken(token);
        }
        #endregion


    }
}

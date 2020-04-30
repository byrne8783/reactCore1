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
    public class UserController : ApiController
    {
        #region _____________________________________________________________Constructors


    public UserController(IdentityDatabase context
            ,UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,IMemoryCache cache) : base(userManager, signInManager,cache )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_userManager = userManager;
            //_signInManager = signInManager;
            //this._cache = cache;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status205ResetContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginData)
        {
            var minimumDurationMilliS = 300;
            ActionResult result;
            ClaimsPrincipal cp = new ClaimsPrincipal();
            var aU = _userManager.FindByNameAsync(loginData.UserId).Result;
            if (aU != null)
            {
                var attempt = await _signInManager.PasswordSignInAsync(loginData.UserId, loginData.Password, false, lockoutOnFailure: false);
                if (!attempt.Succeeded )
                {
                    await _signInManager.SignInAsync(aU, true);
                }
                cp = await _signInManager.CreateUserPrincipalAsync(aU);
            }
            else
            {
                aU = new ApplicationUser();
            }
            if (_signInManager.IsSignedIn(cp))
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, aU.Id)
                            ,new Claim("access_token", GetAccessToken(aU.Id))
                        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // there's a ControllerBase.SignIn(ClaimsPrincipal,AuthenticationProperties,CookieAuthenticationDefaults.AuthenticationScheme)
                // which creates a SignInResult
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                //    , new ClaimsPrincipal(claimsIdentity)
                //    , new AuthenticationProperties());
                // first off my LoginRequest does not have to contain contain a ReturnTo Url value, since it knows where to redirect to
                // hard to figure out the success response to a login.  
                var returnToUrl = Url.Action("index", "home", values: null, protocol: Request.Scheme);
                // 205(ResetContent) looks like the right stuff but nobody uses it
                var result2 = new AcceptedResult(returnToUrl, new ResponseData<object>(null, new { UserId = aU.UserName, aU.Name })) { StatusCode = StatusCodes.Status205ResetContent };
                // 202(accepted) is not entirely appropriate since its supposed to indicate something that's not fully finished
                var result1b = new AcceptedResult(returnToUrl, new ResponseData<object>(null, new { UserId = aU.UserName, aU.Name }));
                var result1a = AcceptedAtAction(returnToUrl, new ResponseData<object>(null, new { UserId = aU.UserName, aU.Name }));
                // 201(created) is just as good as Accepted, but usually reserved for persistent creations
                var result0b = new CreatedResult(returnToUrl, new ResponseData<object>(null, new { UserId = aU.UserName, aU.Name }));
                var authenticationProperties = new AuthenticationProperties() { IsPersistent = false, AllowRefresh=true };

                var result0a = SignIn(new ClaimsPrincipal(claimsIdentity),authenticationProperties ,CookieAuthenticationDefaults.AuthenticationScheme);
                
                result = result0a;
                minimumDurationMilliS = 300;
            }
            else
            {
                result = ValidationProblem(new ValidationProblemDetails() { Title = $"Login Failed for User {loginData?.UserId} " });
                minimumDurationMilliS = 400;
            }


            await Task.Delay(minimumDurationMilliS);
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
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IMemoryCache _cache;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReactCore1.Web
{
    /// <summary>
    /// A local base class for an MVC controller with View support
    /// </summary>
    public abstract class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        #region ___________________________________________properties

        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IMemoryCache _cache;


        #endregion
        #region ____________________________________________constructors
        protected Controller(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMemoryCache cache) : base()
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
        }

        #endregion

    }


    /// <summary>
    /// A local base class for an MVC controller without View support
    /// </summary>
    public abstract class ApiController : Microsoft.AspNetCore.Mvc.ControllerBase
    {

        #region ___________________________________________properties

        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IMemoryCache _cache;


        #endregion


        #region ____________________________________________constructors
        protected ApiController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMemoryCache cache) : base()
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
        }

        #endregion
    }





}

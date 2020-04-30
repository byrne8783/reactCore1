using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ReactCore1.Web
{
    [Authorize]
    public class PersonalController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        #region ____________________________________________constructors
        public PersonalController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMemoryCache cache) : base(userManager, signInManager, cache)
        {

        }

        #endregion


    }
}

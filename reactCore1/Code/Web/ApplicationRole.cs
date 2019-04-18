using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1.Web
{
    public class ApplicationRole : IdentityRole
    {

        public static implicit operator ApplicationRole(string input) =>
            input == null ? null : new ApplicationRole { Name = input };
    }
}

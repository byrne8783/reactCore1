using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1.Web
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName {get;private set;}
        public string LastName { get;private set; }
        public string Name { get { return $"{FirstName} {LastName}"; } }
        public DateTime LastChangedUtc { get; private set; }
        #region _________________________________________________________constructors
        public ApplicationUser() : base()
        {
            base.EmailConfirmed = false;
            FirstName = String.Empty;
            LastName = String.Empty;
            LastChangedUtc = DateTime.UtcNow;
        }
        public ApplicationUser(Guid userId) : this()
        {
            base.Id = userId.ToString("D");
            base.UserName = String.Empty;
            base.NormalizedUserName = base.UserName.ToUpper();
            base.Email = String.Empty;
            base.NormalizedEmail = base.Email.ToUpper();
        }
        public ApplicationUser(User user) : this(user.UserId)
        {
            base.UserName = user.Email ?? String.Empty;
            base.NormalizedUserName = base.UserName.ToUpper();
            base.Email = user.Email?? String.Empty;
            base.NormalizedEmail = user.Key?? base.Email.ToUpper();
            FirstName = user.FirstName?? String.Empty;
            LastName = user.LastName ?? String.Empty;
            LastChangedUtc = user.LastChangedUtc;
        }
        #endregion

        //public User User { get; private set; }

    }
}




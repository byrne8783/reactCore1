using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ReactCore1.Extensions;

namespace ReactCore1.Web
{
    /// <summary>
    /// Implements interfaces AspNetCode.Identity can use to shovel data in and out of the ApplicationUser objects
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <see cref="http://www.elemarjr.com/en/2017/05/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-ravendb/"/>
    public class ApplicationUserStore : IUserStore<ApplicationUser>
        ,IUserPasswordStore<ApplicationUser>
        ,IUserEmailStore<ApplicationUser>

    {
        #region ______________________________________________________________constructors
        public ApplicationUserStore(IdentityDatabase context, IdentityErrorDescriber errorDescriber = null)
        {
            ErrorDescriber = errorDescriber;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion
        public IdentityErrorDescriber ErrorDescriber { get; private set; }
        public IdentityDatabase Context { get; private set; }

        #region __________________________________IUserStore<ApplicationUser>

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                DaUsual(cancellationToken, this);
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                var u = getUserEntity(user);
                Context.Users.Add(u);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }

        }
        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                DaUsual(cancellationToken, this);
                if (user == null)
                    throw new ArgumentNullException(nameof(user));
                Context.Users.Remove(user.NormalizedUserName);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }
        /// <summary>
        /// Find an ApplicationUser using the string Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Null or an ApplicationUser </returns>
        public Task<ApplicationUser> FindByIdAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));
            var u = Context.Users.Find(userName.ToString());
            return Task.FromResult(u == null ? null : new ApplicationUser(u));
        }

        /// <summary>
        /// Find an ApplicationUser using a normalized UserName
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>Null or an ApplicationUser </returns>
        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (string.IsNullOrWhiteSpace(normalizedUserName))
                throw new ArgumentNullException(nameof(normalizedUserName));
            return FindByIdAsync(normalizedUserName, cancellationToken);
        }
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }
        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (normalizedName == null)
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }
            if ( normalizedName.ToUpper() == user.UserName?.ToUpper())
            {
                throw new InvalidOperationException($"Invalid value for {nameof(normalizedName)}");
            }
            user.NormalizedUserName = normalizedName.ToUpper();
            return Task.CompletedTask;
        }
        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            user.UserName = userName;
            return SetNormalizedUserNameAsync(user, userName, cancellationToken);
        }
        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            DaUsual(cancellationToken, this); 
            if (user?.Id == null )
            {
                throw new ArgumentNullException(nameof(user));
            }
            try
            {
                Context.Users.Update(getUserEntity(user));
            }
            catch (Exception)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "Failed", Description = $"User {user?.Id} Update Failed" }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
        #endregion


        #region _________________________________________________________________IUserEmailStore<ApplicationUser> Members
        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Email = email.IsEmailFormat() ? email : throw new InvalidOperationException();
            SetUserNameAsync(user,user.Email);
            user.NormalizedEmail=user.Email.ToUpper();
            return SetEmailConfirmedAsync(user,false, cancellationToken);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken,this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                throw new ArgumentNullException(nameof(normalizedEmail));
            var userEntity = Context.Users.FindByNormalizedEMail(normalizedEmail);
            return Task.FromResult(getApplicationUser(userEntity));
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (normalizedEmail == null)
            {
                throw new ArgumentNullException(nameof(normalizedEmail));
            }
            if (normalizedEmail.ToUpper() == user.Email?.ToUpper())
            {
                throw new InvalidOperationException($"Invalid value for {nameof(normalizedEmail)}");
            }
            user.NormalizedEmail = normalizedEmail.ToUpper();
            return Task.CompletedTask;
        }
        #endregion

        #region __________________________________________________________IUserPasswordStore
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(HasPasswordAsync(user, cancellationToken).Result ? user.PasswordHash : string.Empty);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }
        #endregion




        #region _______________________________________________________________________________________Private Methods
        private User getUserEntity(ApplicationUser applicationUser)
        {
            Func<User, User> fillUser = (u) => { populateUserEntity(u, applicationUser); return u; };
            return applicationUser == null ? null : fillUser(new User());
        }

        private void populateUserEntity(User entity, ApplicationUser applicationUser)
        {
            var x = new Guid();
            entity.UserId = Guid.TryParse(applicationUser.Id, out x ) ? x : throw new InvalidOperationException($"Invalid value for {nameof(applicationUser.Id)}");
            entity.Email = applicationUser.Email;
            entity.FirstName = applicationUser.FirstName;
            entity.LastName = applicationUser.LastName;
        }

        private ApplicationUser getApplicationUser(User entity)
        {
            return entity == null ? null : new ApplicationUser(entity);
        }
        private Action<CancellationToken, ApplicationUserStore> DaUsual = (c,s) => {c.ThrowIfCancellationRequested(); s.ThrowIfDisposed();return; };

        #endregion

        #region IDisposable
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            _disposed = true;
        }


        #endregion

    }
}









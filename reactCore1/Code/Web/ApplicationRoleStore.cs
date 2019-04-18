using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactCore1.Web
{
    public class ApplicationRoleStore : IRoleStore<ApplicationRole>
    {
        #region ______________________________________________________________constructors
        public ApplicationRoleStore(IdentityDatabase context, IdentityErrorDescriber errorDescriber = null)
        {
            ErrorDescriber = errorDescriber;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion
        public IdentityErrorDescriber ErrorDescriber { get; private set; }
        public IdentityDatabase Context { get; private set; }

        #region __________________________________IUserStore<ApplicationRole>
        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            DaUsual(cancellationToken, this);
            throw new NotImplementedException();
        }


        private Action<CancellationToken, ApplicationRoleStore> DaUsual = (c, s) => { c.ThrowIfCancellationRequested(); s.ThrowIfDisposed(); return; };
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

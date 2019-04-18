using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1
{
    public class IdentityRole
    {

        private readonly List<SomeClaim> _claims;

        public IdentityRole()
        {
            _claims = new List<SomeClaim>();
        }

        public string Id { get; internal set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public IEnumerable<SomeClaim> Claims
        {
            get => _claims;
            internal set
            {
                if (value != null) _claims.AddRange(value);
            }
        }

        internal void AddClaim(SomeClaim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            _claims.Add(claim);
        }

        internal void RemoveClaim(SomeClaim claim)
        {
            _claims.Remove(claim);
        }

        public static implicit operator IdentityRole(string input) =>
            input == null ? null : new IdentityRole { Name = input };
    }
}

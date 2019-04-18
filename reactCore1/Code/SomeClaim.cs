using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReactCore1
{
    public class SomeClaim : IEquatable<SomeClaim>, IEquatable<System.Security.Claims.Claim>
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public static implicit operator SomeClaim(System.Security.Claims.Claim original) =>
            new SomeClaim { Type = original.Type, Value = original.Value };

        public static implicit operator System.Security.Claims.Claim(SomeClaim quick) =>
            new System.Security.Claims.Claim(quick.Type, quick.Value);

        public bool Equals(SomeClaim other)
            => Type == other.Type && Value == other.Value;

        public bool Equals(System.Security.Claims.Claim other)
            => Type == other.Type && Value == other.Value;
    }
}

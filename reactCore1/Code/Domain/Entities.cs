using Microsoft.Extensions.Caching.Memory;
using ReactCore1.Data;
using ReactCore1.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1
{
    public class User : IRepositoryItem<User,string>
    {
        public Guid UserId { get { return _UserId; } internal set { TimeStampChange(); _UserId = value; } }
        public string Email { get { return _Email; } set { TimeStampChange(); _Email = value; } }
        public string Key { get { return Email?.ToUpper(); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return $"{FirstName} {LastName}"; } }
        public DateTime LastChangedUtc { get; private set; }
        public Guid _UserId;
        private string _Email;
        private void TimeStampChange()
        {
            LastChangedUtc = DateTime.UtcNow;
        }

    }
    public class IdentityDatabase : QuickDatabase
    {
        #region ______________________________________________________________constructors
        public IdentityDatabase(IMemoryCache cache)
        {
            this.Cache = cache;
            this.UserTable = Cache.GetOrCreate(CacheKey.Users, users =>
            {
                var defaultUsers = new List<User>
                 {
                    new User{ UserId=new Guid("8E446F26-D3FF-4BF0-9474-8F87C969201E"),Email="fred@a.com",FirstName = "Fredrick", LastName = "Murphy"}
                    ,new User{ UserId=new Guid("7B5186E4-6D9C-4147-B50B-870B45FB66D0"),Email="jack@b.com",FirstName = "Jacques", LastName = "Kelly"}
                    ,new User{ UserId=new Guid("CB560CE9-C74B-4DA0-806E-B6DFA39821BA"),Email="mick@c.com",FirstName = "Micheal", LastName = "Byrne"}
                    ,new User{ UserId=new Guid("B4390F01-7B7F-4104-997C-61384C49079A"),Email="tony@d.com",FirstName = "Antonio", LastName = "Ward"}
                 };
                users.SetOptions(_defaultMCEntryOptions);
                return new UserTable(defaultUsers);
            });
        }
        #endregion
        


        public INamedRepository<User> Users
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(UserTable));
            }
        }
        public override void Create<TEntity>(IEnumerable<TEntity> entities)
        {
            switch (entities)
            {
                case IEnumerable<User> u:
                    UserTable = UserTable == null ? new UserTable(u) : throw new ArgumentNullException($"{nameof(u)} already exists");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Save()
        {
            Cache.Set(CacheKey.Users, UserTable, _defaultMCEntryOptions);
        }
        private MemoryCacheEntryOptions _defaultMCEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove)
                .SetSlidingExpiration(TimeSpan.FromHours(1));
        private INamedRepository<User> _userRepository;
        private IMemoryCache Cache { get; set; }
        private UserTable UserTable { get; set; }
    }
}

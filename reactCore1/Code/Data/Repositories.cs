using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1.Data
{
    internal abstract class RepositoryBase<TEntity, TKey> where TEntity : class,IRepositoryItem<TEntity,TKey>
    {


    }

    internal class UserRepository : RepositoryBase<User, string>, INamedRepository<User>
    {

        UserTable _db;

        public void Add(User entity)
        {
            _db.Add(entity);
        }
        public IEnumerable<User> All()
        {
            return _db.AsEnumerable();
        }
        /// <summary>
        /// A User entity or null
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A User entity</returns>
        public User Find(string key)
        {
            return _db.Select(key);
        }
        public IEnumerable<User> FindByNormalizedName(string normalizedUserName)
        {
            return All().Where(x=> normalizedUserName == x.Name );
        }
        public User FindByNormalizedEMail(string normalizedUserEMail)
        {
            return Find(normalizedUserEMail);
        }
        public void Update(User entity)
        {
            try
            {
                if (_db.Remove(entity.Key))
                {
                    _db.Add(entity);
                }
                else
                {
                    throw new Exception($"User {entity.Key} Not found");
                }
            }
            catch ( Exception e)
            {
                throw e;
            }
        }

        public bool Remove(string key)
        {
            return _db.Remove(key);
        }
        #region _________________________________________constructors
        public UserRepository (UserTable db)
        {
            _db = db;
 
        }
        #endregion
    }

}

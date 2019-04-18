using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1.Data
{
    public class QuickEqualityComparer<TKey> : EqualityComparer<TKey>
    {
        public override bool Equals(TKey str1,TKey str2)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(str1.ToString(), str2.ToString());
        }

        public override int GetHashCode (TKey str1)
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(str1.ToString());
        }
    }

    public abstract class QuickTable<TEntity,TKey> : KeyedCollection<TKey,TEntity> where TEntity : class,IRepositoryItem<TEntity,TKey>
    {
        #region _____________________________________constructors
        protected QuickTable () : base(new QuickEqualityComparer<TKey>()) { }

        protected QuickTable(IEqualityComparer<TKey> comparer) : base(comparer) { }

        protected QuickTable(IEnumerable<TEntity> entities) : base(new QuickEqualityComparer<TKey>()) { entities.Distinct().ToList().ForEach(r => Add(r)); }
        #endregion
        protected override TKey GetKeyForItem(TEntity entity)
        {
            return entity.Key;
        }

        public TEntity Select(TKey key) { TEntity result; return TryGetValue(key, out result) ?  result : null ; }

    }

    public abstract class QuickDatabase
    {
        public abstract void Create<TEntity>(IEnumerable<TEntity> entities);
        public abstract void Save();

    }

    public class UserTable : QuickTable<User,string>
    {
        #region _____________________________________constructors
        internal UserTable(IEnumerable<User> users) : base(users) { }
        #endregion
    }
}

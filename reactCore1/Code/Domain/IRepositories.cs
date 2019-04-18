using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactCore1
{

    public interface IRepositoryItem<TItem, TKey> where TItem : class
    {
        TKey Key { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    ///<see cref="https://timschreiber.com/2018/05/07/aspnet-core-identity-with-patterns-2/"/>
    public interface IRepository<TEntity, TKey> where TEntity : class,IRepositoryItem<TEntity,TKey>
    {
        IEnumerable<TEntity> All();

        TEntity Find(TKey key);

        void Add(TEntity entity);

        void Update(TEntity entity);

        bool Remove(TKey key);
    }

    /// <summary>
    /// not using this as I just added the concrete IdentityDatabase to the container
    /// </summary>
    public interface IRepositoryContainer<IRepository,TEntity, TKey> where TEntity : class, IRepositoryItem<TEntity, TKey>
    {
        IDictionary<string, IRepository<TEntity,TKey>> All();
    }

    public interface INamedRepository<TEntity> : IRepository<TEntity, string> where TEntity : class, IRepositoryItem<TEntity,string>
    {
        IEnumerable<TEntity> FindByNormalizedName(string normalizedUserName);

        TEntity FindByNormalizedEMail(string normalizedEmail);
    }



}

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository to retrieve the instances of the entity from the database
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to retrieve</typeparam>
    /// <typeparam name="TKey">The type of the key of the entity</typeparam>
    public abstract class QueryRepository<TEntity, TKey> : IQueryRepository
        where TEntity : Entity<TKey>, new()
    {
        public string ConnectionName { get; set; }

        public abstract TEntity GetById(TKey id);

        public IEntity GetById(object id)
        {
            return GetById((TKey)id);
        }

        protected TEntity CreateEntity(TKey id)
        {
            return new TEntity
            {
                Id = id
            };
        }
    }
}

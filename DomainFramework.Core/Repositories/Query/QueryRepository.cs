namespace DomainFramework.Core
{
<<<<<<< HEAD
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
=======
    public abstract class QueryRepository<E, K, T> : IQueryRepository
        where E : Entity<K, T>, new()
    {
        public string ConnectionName { get; set; }

        public IRepositoryContext RepositoryContext { get; set; }

        public QueryRepository(IRepositoryContext context)
        {
            RepositoryContext = context;
        }

        public abstract E GetById(K id);

        public IEntity GetById(object id)
        {
            return GetById((K)id);
        }

        protected E CreateEntity(K id, T data)
        {
            return new E
            {
                Id = id,
                Data = data
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
            };
        }
    }
}

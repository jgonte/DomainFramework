namespace DomainFramework.Core
{
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
            };
        }
    }
}

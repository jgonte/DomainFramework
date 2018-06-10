using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class QueryAggregateCollection<TAggregate, TEntity, TKey> : IQueryAggregateCollection<TAggregate, TEntity>
        where TAggregate : IQueryAggregate<TKey, TEntity>, new()
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public QueryAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }

        /// <summary>
        /// Loads a collection of aggregates based on the parameters provided
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        public virtual void Load(QueryParameters queryParameters, IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = repository.Get(queryParameters).Cast<TEntity>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                aggregate.LoadAggregatedEntities();

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }
        }

        public virtual async Task LoadAsync(QueryParameters queryParameters, IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {           
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = repository.Get(queryParameters).Cast<TEntity>();

            var tasks = new Queue<Task>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                tasks.Enqueue(
                    aggregate.LoadAggregatedEntitiesAsync()
                );

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class QueryAggregateCollection<TAggregate, TEntity, TKey, TDto> : IQueryAggregateCollection<TAggregate, TEntity>
        where TAggregate : IQueryAggregate<TEntity, TKey, TDto>, new()
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public QueryAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }

        public IEnumerable<TDto> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            Load(queryParameters, user);

            return Aggregates.Select(a => a.GetDataTransferObject());
        }

        public async Task<IEnumerable<TDto>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            await LoadAsync(queryParameters, user);

            return Aggregates.Select(a => a.GetDataTransferObject());
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

            var entities = ((IEntityQueryRepository)repository).Get(queryParameters, user).Cast<TEntity>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                aggregate.LoadLinks();

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }
        }

        public virtual async Task LoadAsync(QueryParameters queryParameters, IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {           
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = ((IEntityQueryRepository)repository).Get(queryParameters, user).Cast<TEntity>();

            var tasks = new Queue<Task>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                tasks.Enqueue(
                    aggregate.LoadLinksAsync()
                );

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);
        }
    }
}

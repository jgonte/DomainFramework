using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetLinkedAggregateQuerySingleItemOperation<TKey, TEntity, TOutputDto> : IQueryOperation
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, IEntity> GetLinkedEntity { get; set; }

        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, Task<IEntity>> GetLinkedEntityAsync { get; set; }

        public Func<IEntity, IQueryAggregate> CreateLinkedQueryAggregate { get; set; }

        private IOutputDataTransferObject _outputDto;

        public TOutputDto OutputDto => (TOutputDto)_outputDto;

        /// <summary>
        /// Whether to execute the operation
        /// </summary>
        public Func<IEntity, bool> OnBeforeExecute { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            if (OnBeforeExecute != null &&
                !OnBeforeExecute(entity))
            {
                return;
            }

            var repository = repositoryContext.GetQueryRepository(typeof(TEntity));

            var linkedEntity = GetLinkedEntity((IEntityQueryRepository)repository, (TKey)entity.Id, user);

            if (linkedEntity == null)
            {
                return;
            }

            var aggregate = CreateLinkedQueryAggregate(linkedEntity);

            aggregate.RootEntity = linkedEntity;

            aggregate.LoadLinks();

            aggregate.PopulateDto();

            _outputDto = aggregate.OutputDto;
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            if (OnBeforeExecute != null &&
                !OnBeforeExecute(entity))
            {
                return;
            }

            var repository = repositoryContext.GetQueryRepository(typeof(TEntity));

            var linkedEntity = await GetLinkedEntityAsync((IEntityQueryRepository)repository, (TKey)entity.Id, user);

            if (linkedEntity == null)
            {
                return;
            }

            var aggregate = CreateLinkedQueryAggregate(linkedEntity);

            aggregate.RootEntity = linkedEntity;

            await aggregate.LoadLinksAsync();

            aggregate.PopulateDto();

            _outputDto = aggregate.OutputDto;
        }
    }
}

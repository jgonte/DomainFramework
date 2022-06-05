using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetAllLinkedAggregateQueryCollectionOperation<TKey, TEntity, TOutputDto> : IQueryOperation
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, IEnumerable<IEntity>> GetAllLinkedEntities { get; set; }

        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, Task<IEnumerable<IEntity>>> GetAllLinkedEntitiesAsync { get; set; }

        public Func<IEntity, IQueryAggregate> CreateLinkedQueryAggregate { get; set; }

        private List<IOutputDataTransferObject> _outputDtos = new List<IOutputDataTransferObject>();

        public IEnumerable<TOutputDto> OutputDtos => _outputDtos.Cast<TOutputDto>();

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = repositoryContext.GetQueryRepository(typeof(TEntity));

            _outputDtos.Clear();

            foreach (var linkedEntity in GetAllLinkedEntities((IEntityQueryRepository)repository, (TKey)entity.Id, user))
            {
                var aggregate = CreateLinkedQueryAggregate(linkedEntity);

                aggregate.RootEntity = linkedEntity;

                aggregate.LoadLinks();

                aggregate.PopulateDto();

                _outputDtos.Add(aggregate.OutputDto);
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = repositoryContext.GetQueryRepository(typeof(TEntity));

            _outputDtos.Clear();

            foreach (var linkedEntity in await GetAllLinkedEntitiesAsync((IEntityQueryRepository)repository, (TKey)entity.Id, user))
            {
                await LoadLinkedAggregateAsync(linkedEntity);
            }
        }

        private async Task LoadLinkedAggregateAsync(IEntity linkedEntity)
        {
            var aggregate = CreateLinkedQueryAggregate(linkedEntity);

            aggregate.RootEntity = linkedEntity;

            await aggregate.LoadLinksAsync();

            aggregate.PopulateDto();

            _outputDtos.Add(aggregate.OutputDto);
        }
    }
}

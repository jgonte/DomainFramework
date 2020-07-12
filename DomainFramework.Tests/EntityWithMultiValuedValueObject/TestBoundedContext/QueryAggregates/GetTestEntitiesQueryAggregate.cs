using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class GetTestEntitiesQueryAggregate : QueryAggregateCollection<TestEntity, TestEntityOutputDto, GetTestEntityByIdQueryAggregate>
    {
        public GetTestEntitiesQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            TestEntityQueryRepository.Register(context);

            TestEntity_TypeValues1_QueryRepository.Register(context);
        }

        public (int, IEnumerable<TestEntityOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (TestEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TestEntity));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<TestEntityOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (TestEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TestEntity));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntityQueryRepository : EntityQueryRepository<TestEntity, int?>
    {
        public override (int, IEnumerable<TestEntity>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<TestEntity>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<TestEntity>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<TestEntity>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<TestEntity> GetAll()
        {
            var result = Query<TestEntity>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<TestEntity>> GetAllAsync()
        {
            var result = await Query<TestEntity>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override TestEntity GetById(int? testEntityId)
        {
            var result = Query<TestEntity>
                .Single()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetById]")
                .Parameters(
                    p => p.Name("testEntityId").Value(testEntityId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<TestEntity> GetByIdAsync(int? testEntityId)
        {
            var result = await Query<TestEntity>
                .Single()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetById]")
                .Parameters(
                    p => p.Name("testEntityId").Value(testEntityId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<TestEntity>(new TestEntityQueryRepository());
        }

    }
}
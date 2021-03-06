using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntity_TypeValues1_QueryRepository : ValueObjectQueryRepository<int, TypeValue>
    {
        public override (int, IEnumerable<TypeValue>) Get(int testEntityId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public override Task<(int, IEnumerable<TypeValue>)> GetAsync(int testEntityId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TypeValue> GetAll(int testEntityId)
        {
            var result = Query<TypeValue>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetAllTypeValues1]")
                .Parameters(
                    p => p.Name("testEntityId").Value(testEntityId)
                )
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<TypeValue>> GetAllAsync(int testEntityId)
        {
            var result = await Query<TypeValue>
                .Collection()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_GetAllTypeValues1]")
                .Parameters(
                    p => p.Name("testEntityId").Value(testEntityId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<RepositoryKey>(new TestEntity_TypeValues1_QueryRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
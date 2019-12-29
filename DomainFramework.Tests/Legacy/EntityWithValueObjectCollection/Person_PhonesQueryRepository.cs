using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class Person_PhonesQueryRepository : ValueObjectQueryRepository<int?, Phone>
    {
        public override IEnumerable<Phone> GetAll(int? ownerId)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetPhones")
                .Parameters(
                    p => p.Name("personId").Value(ownerId)
                )
                .Execute();

            return result.Data;
        }

        public override Task<IEnumerable<Phone>> GetAllAsync(int? ownerId)
        {
            throw new System.NotImplementedException();
        }

        public override (int, IEnumerable<Phone>) Get(int? ownerId, CollectionQueryParameters queryParameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<Phone>)> GetAsync(int? ownerId, CollectionQueryParameters queryParameters)
        {
            throw new System.NotImplementedException();
        }

        internal class RepositoryKey
        {
        }
    }
}
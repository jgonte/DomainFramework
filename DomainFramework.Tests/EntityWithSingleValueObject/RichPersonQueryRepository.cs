using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    class RichPersonQueryRepository : QueryRepository<RichPersonEntity, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override RichPersonEntity GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<RichPersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_RichPerson_Get")
                .Parameters(
                    p => p.Name("richPersonId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    entity.FirstName = reader.GetString(0);

                    entity.Capital = new Money(reader.GetDecimal(1));
                })
                .Execute();

            return result.Data;
        }

        public override async Task<RichPersonEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<RichPersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_RichPerson_Get")
                .Parameters(
                    p => p.Name("richPersonId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    entity.FirstName = reader.GetString(0);

                    entity.Capital = new Money(reader.GetDecimal(1));
                })
                .ExecuteAsync();

            return result.Data;
        }
    }
}
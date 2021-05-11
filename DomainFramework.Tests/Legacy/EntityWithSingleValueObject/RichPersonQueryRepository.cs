using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    class RichPersonQueryRepository : EntityQueryRepository<RichPersonEntity, int?>
    {
        public override (int, IEnumerable<RichPersonEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<RichPersonEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override RichPersonEntity GetById(int? id)
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

        public override async Task<RichPersonEntity> GetByIdAsync(int? id)
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
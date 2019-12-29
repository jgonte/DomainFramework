using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class BookQueryRepository : Core.EntityQueryRepository<BookEntity, int?>
    {
        public override (int, IEnumerable<BookEntity>) Get(CollectionQueryParameters parameters)
        {
            var result = Query<BookEntity>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_GetAll")
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = reader.GetInt32(0);

                    entity.Title = reader.GetString(1);
                })
                .Execute();

            return (result.Data.Count, result.Data);
        }

        public override Task<(int, IEnumerable<BookEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override BookEntity GetById(int? id)
        {
            var result = Query<BookEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Get")
                .Parameters(
                    p => p.Name("bookId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    entity.Title = reader.GetString(0);
                })
                .Execute();

            return result.Data;
        }

        public override async Task<BookEntity> GetByIdAsync(int? id)
        {
            var result = await Query<BookEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Get")
                .Parameters(
                    p => p.Name("bookId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    entity.Title = reader.GetString(0);
                })
                .ExecuteAsync();

            return result.Data;
        }
    }
}

using DataAccess;

namespace DomainFramework.Tests
{
    public class BookQueryRepository : Core.QueryRepository<BookEntity, int?>
    {
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

                    var book = new BookData
                    {
                        Title = reader.GetString(0)
                    };

                    entity.Data = book;
                })
                .Execute();

            return result.Data;
        }
    }
}

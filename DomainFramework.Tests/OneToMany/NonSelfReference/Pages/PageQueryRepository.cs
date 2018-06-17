using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class PageQueryRepository : QueryRepository<PageEntity, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override PageEntity GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Get")
                .Parameters(
                    p => p.Name("pageId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    var page = new PageData
                    {
                        Index = reader.GetInt32(0)
                    };

                    entity.Data = page;

                    entity.BookId = reader.GetInt32(1);
                })
                .Execute();

            result.Data.Id = id.Value;

            return result.Data;
        }

        public override async Task<PageEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Get")
                .Parameters(
                    p => p.Name("pageId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    var page = new PageData
                    {
                        Index = reader.GetInt32(0)
                    };

                    entity.Data = page;

                    entity.BookId = reader.GetInt32(1);
                })
                .ExecuteAsync();

            result.Data.Id = id.Value;

            return result.Data;
        }

        public IEnumerable<PageEntity> GetForBook(int? id, IAuthenticatedUser user)
        {
            var result = Query<PageEntity>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_GetPages")
                .Parameters(
                    p => p.Name("bookId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = reader.GetInt32(0);

                    var page = new PageData
                    {
                        Index = reader.GetInt32(1)
                    };

                    entity.Data = page;

                    entity.BookId = reader.GetInt32(2);
                })
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<PageEntity>> GetForBookAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<PageEntity>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_GetPages")
                .Parameters(
                    p => p.Name("bookId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = reader.GetInt32(0);

                    var page = new PageData
                    {
                        Index = reader.GetInt32(1)
                    };

                    entity.Data = page;

                    entity.BookId = reader.GetInt32(2);
                })
                .ExecuteAsync();

            return result.Data;
        }
    }
}

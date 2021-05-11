using DataAccess;
using DomainFramework.Core;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    public class PageCommandRepository : DataAccess.EntityCommandRepository<PageEntity>
    {
        protected override Command CreateInsertCommand(PageEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Create")
                .Parameters(
                    p => p.Name("index").Value(entity.Index)
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PageEntity>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var e = (BookEntity)Dependencies().Single().Entity;

                    entity.BookId = e.Id.Value;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("BookId").Value(entity.BookId)
                    );
                });
        }

        protected override Command CreateUpdateCommand(PageEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Update")
                .Parameters(
                    p => p.Name("pageId").Value(entity.Id.Value),
                    p => p.Name("index").Value(entity.Index),
                    p => p.Name("bookId").Value(entity.BookId)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<PageEntity>)command).Execute();
        }
    }
}

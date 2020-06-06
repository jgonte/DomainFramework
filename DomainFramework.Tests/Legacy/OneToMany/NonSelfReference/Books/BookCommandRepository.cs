using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class BookCommandRepository : DataAccess.EntityCommandRepository<BookEntity>
    {      
        protected override Command CreateInsertCommand(BookEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<BookEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Create")
                .Parameters(
                    p => p.Name("title").Value(entity.Title)
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<BookEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(BookEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Update")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id.Value),
                    p => p.Name("title").Value(entity.Title)
                );
        }

        protected override Command CreateDeleteCommand(BookEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Delete")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<BookEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteLinksCommand(BookEntity entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                default: return Command
                    .NonQuery()
                    .Connection(ConnectionName)
                    .StoredProcedure("p_Book_RemovePages")
                    .Parameters(
                        p => p.Name("bookId").Value(entity.Id)
                    );
            }
            
        }

        protected override bool HandleDeleteLinks(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }
    }
}

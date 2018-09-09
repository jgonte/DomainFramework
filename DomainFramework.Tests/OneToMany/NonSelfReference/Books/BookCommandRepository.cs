using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    public class BookCommandRepository : DataAccess.EntityCommandRepository<BookEntity>
    {      
        protected override Command CreateInsertCommand(BookEntity entity, IAuthenticatedUser user)
        {
            return Query<BookEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Create")
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<BookEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(BookEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Book_Update")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                );
        }

        protected override Command CreateDeleteCommand(BookEntity entity, IAuthenticatedUser user)
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

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}

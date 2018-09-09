using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    public class PageCommandRepository : DataAccess.EntityCommandRepository<PageEntity>
    {
        protected override Command CreateInsertCommand(PageEntity entity, IAuthenticatedUser user)
        {
            return Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: entity.Data
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PageEntity>(m => m.Id),//.Index(0),
                    pm => pm.Map<PageEntity>(m => m.Data).Ignore()
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var e = (BookEntity)TransferEntities().Single();

                    entity.BookId = e.Id.Value;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("BookId").Value(entity.BookId)
                    );
                });
        }

        protected override Command CreateUpdateCommand(PageEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Update")
                .Parameters(
                    p => p.Name("pageId").Value(entity.Id.Value),
                    p => p.Name("bookId").Value(entity.BookId)
                )
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                );
        }

        protected override Command CreateDeleteCommand(PageEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<PageEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command commandy)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}

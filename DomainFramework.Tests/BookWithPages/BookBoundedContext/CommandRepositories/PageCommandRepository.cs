using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class PageCommandRepository : EntityCommandRepository<Page>
    {
        protected override Command CreateInsertCommand(Page entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Page>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Insert]")
                .Parameters(
                    p => p.Name("index").Value(entity.Index),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var bookDependency = (Book)dependencies?.SingleOrDefault()?.Entity;

                    if (bookDependency != null)
                    {
                        entity.BookId = bookDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("bookId").Value(entity.BookId)
                    );
                })
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Page>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Page>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Page entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Update]")
                .Parameters(
                    p => p.Name("pageId").Value(entity.Id),
                    p => p.Name("index").Value(entity.Index),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("bookId").Value(entity.BookId)
                );
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleUpdateAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteCommand(Page entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Delete]")
                .Parameters(
                    p => p.Name("pageId").Value(entity.Id)
                );
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
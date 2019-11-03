using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class BookCommandRepository : EntityCommandRepository<Book>
    {
        protected override Command CreateInsertCommand(Book entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Insert]")
                .Parameters(
                    p => p.Name("title").Value(entity.Title),
                    p => p.Name("category").Value(entity.Category),
                    p => p.Name("datePublished").Value(entity.DatePublished),
                    p => p.Name("publisherId").Value(entity.PublisherId),
                    p => p.Name("isHardCopy").Value(entity.IsHardCopy),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Book>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Book>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Book entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Update]")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id),
                    p => p.Name("title").Value(entity.Title),
                    p => p.Name("category").Value(entity.Category),
                    p => p.Name("datePublished").Value(entity.DatePublished),
                    p => p.Name("publisherId").Value(entity.PublisherId),
                    p => p.Name("isHardCopy").Value(entity.IsHardCopy),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
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

        protected override Command CreateDeleteCommand(Book entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Delete]")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Book entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_DeletePages]")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
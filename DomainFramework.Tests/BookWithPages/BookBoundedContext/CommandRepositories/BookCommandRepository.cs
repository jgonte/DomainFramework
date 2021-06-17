using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utilities;

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

            var command = Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Insert]")
                .Parameters(
                    //p => p.Set("title", entity.Title),
                    //p => p.Name("category").Value(entity.Category),
                    //p => p.Name("datePublished").Value(entity.DatePublished),
                    //p => p.Name("publisherId").Value(entity.PublisherId),
                    //p => p.Name("isHardCopy").Value(entity.IsHardCopy),
                    //p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("bookId").SqlType((int)SqlDbType.Int).IsOutput()
                )
                .AutoGenerateParameters(excludedProperties: new Expression<Func<Book, object>>[]{
                        m => m.Id,
                        m => m.CreatedDateTime,
                        m => m.UpdatedBy,
                        m => m.UpdatedDateTime
                })
                .RecordInstance(entity)
                //.MapProperties(
                //    p => p.Name("Id").Index(0)
                //)
                .MapOutputParameters(
                    map => map.Name("bookId").Property<Book>(b => b.Id)
                )
                //.OnAfterCommandExecuted(cmd =>

                //    entity.Id = (int)cmd.Parameters
                //    .Where(p => 
                //        p.Direction == System.Data.ParameterDirection.Output && 
                //        p.Name == "bookId"
                //    )
                //    .Single().Value
                //)
                ;

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

        protected override Command CreateUpdateCommand(Book entity, IAuthenticatedUser user, string selector)
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

        protected override Command CreateDeleteCommand(Book entity, IAuthenticatedUser user, string selector)
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

        protected override Command CreateDeleteLinksCommand(Book entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_UnlinkPages]")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteLinks(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteLinksAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
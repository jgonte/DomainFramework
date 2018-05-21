using DataAccess;
using DomainFramework.DataAccess;
using System;

namespace DomainFramework.Tests
{
    public class BookCommandRepository : CommandRepository<BookEntity, int?, Book>
    {      
        protected override Command CreateInsertCommand(BookEntity entity)
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
                    pm => pm.Map(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(BookEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override Command CreateDeleteCommand(BookEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command, BookEntity entity)
        {
            ((SingleQuery<BookEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command, BookEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command, BookEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

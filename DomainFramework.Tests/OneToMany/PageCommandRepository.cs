using DataAccess;
using DomainFramework.DataAccess;
using System;

namespace DomainFramework.Tests
{
    public class PageCommandRepository : CommandRepository<PageEntity, int?, Page>
    {      
        protected override Command CreateInsertCommand(PageEntity entity)
        {
            return Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: entity.Data
                )
                .Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("BookId").Value(entity.BookId)
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(PageEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override Command CreateDeleteCommand(PageEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command, PageEntity entity)
        {
            ((SingleQuery<PageEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command, PageEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command, PageEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

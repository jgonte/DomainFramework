﻿using DataAccess;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    public class PageCommandRepository : CommandRepository<PageEntity, int?>
    {
        protected override Command CreateInsertCommand(PageEntity entity)
        {
            // TransferEntities is mutable store the current one to use it later
            var transferEntities = TransferEntities;

            var command = Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Page_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: entity.Data
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map(m => m.Id)//.Index(0),
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var e = (BookEntity)transferEntities().Single();

                entity.BookId = e.Id.Value;

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("BookId").Value(entity.BookId)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(PageEntity entity)
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

        protected override Command CreateDeleteCommand(PageEntity entity)
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
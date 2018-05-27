using DataAccess;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class CapitalCityCommandRepository : CommandRepository<CapitalCityEntity, int?>
    {
        protected override Command CreateInsertCommand(CapitalCityEntity entity)
        {
            // TransferEntities is mutable store the current one to use it later
            var transferEntities = TransferEntities;

            var command = Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: entity.Data
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map(m => m.Id)//.Index(0),
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var e = (CountryEntity)transferEntities().Single();

                entity.CountryCode = e.Id;

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("CountryCode").Value(entity.CountryCode)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(CapitalCityEntity entity)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Update")
                .Parameters(
                    p => p.Name("capitalCityId").Value(entity.Id.Value),
                    p => p.Name("countryCode").Value(entity.CountryCode)
                )
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                );
        }

        protected override Command CreateDeleteCommand(CapitalCityEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<CapitalCityEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command)
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
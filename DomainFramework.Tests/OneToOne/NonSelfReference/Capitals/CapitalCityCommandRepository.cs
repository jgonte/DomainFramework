using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class CapitalCityCommandRepository : DataAccess.CommandEntityRepository<CapitalCityEntity>
    {
        protected override Command CreateInsertCommand(CapitalCityEntity entity, IAuthenticatedUser user)
        {
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
                var e = (CountryEntity)TransferEntities().Single();

                entity.CountryCode = e.Id;

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("CountryCode").Value(entity.CountryCode)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(CapitalCityEntity entity, IAuthenticatedUser user)
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

        protected override Command CreateDeleteCommand(CapitalCityEntity entity, IAuthenticatedUser user)
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
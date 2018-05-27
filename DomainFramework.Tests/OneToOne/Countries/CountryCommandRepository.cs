using System.Threading.Tasks;
using DataAccess;
using DomainFramework.DataAccess;

namespace DomainFramework.Tests
{
    class CountryCommandRepository : CommandRepository<CountryEntity, string>
    {
        protected override Command CreateInsertCommand(CountryEntity entity)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Create")
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                )
                .OnAfterCommandExecuted(() => { entity.Id = entity.Data.CountryCode; });
        }

        protected override Command CreateUpdateCommand(CountryEntity entity)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Update")
                .AutoGenerateParameters(
                    qbeObject: entity.Data
                );
        }

        protected override Command CreateDeleteCommand(CountryEntity entity)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Delete")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
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
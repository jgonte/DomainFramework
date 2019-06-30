using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryCommandRepository : DataAccess.EntityCommandRepository<CountryEntity>
    {
        protected override Command CreateInsertCommand(CountryEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Create")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name)
                );
        }

        protected override Command CreateUpdateCommand(CountryEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Update")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name)
                );
        }

        protected override Command CreateDeleteCommand(CountryEntity entity, IAuthenticatedUser user)
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
    }
}
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CapitalCityCommandRepository : EntityCommandRepository<CapitalCity>
    {
        protected override Command CreateInsertCommand(CapitalCity entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var countryDependency = (Country)dependencies?.SingleOrDefault()?.Entity;

                    if (countryDependency != null)
                    {
                        entity.CountryCode = countryDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("countryCode").Value(entity.CountryCode)
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
            ((SingleQuery<CapitalCity>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<CapitalCity>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(CapitalCity entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Update]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("countryCode").Value(entity.CountryCode)
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

        protected override Command CreateDeleteCommand(CapitalCity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Delete]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(entity.Id)
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
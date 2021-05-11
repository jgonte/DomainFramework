using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryCommandRepository : EntityCommandRepository<Country>
    {
        protected override Command CreateInsertCommand(Country entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Insert]")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("isActive").Value(entity.IsActive),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Country entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            switch (selector)
            {
                case "Activate": return Command
                    .NonQuery()
                    .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                    .StoredProcedure("[CountryBoundedContext].[pCountry_Activate]")
                    .Parameters(
                        p => p.Name("countryCode").Value(entity.Id),
                        p => p.Name("updatedBy").Value(entity.UpdatedBy)
                    );

                case "Inactivate": return Command
                    .NonQuery()
                    .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                    .StoredProcedure("[CountryBoundedContext].[pCountry_Inactivate]")
                    .Parameters(
                        p => p.Set("countryCode", entity.Id),
                        p => p.Name("updatedBy").Value(entity.UpdatedBy)
                    );

                default: return Command
                    .NonQuery()
                    .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                    .StoredProcedure("[CountryBoundedContext].[pCountry_Update]")
                    .Parameters(
                        p => p.Name("countryCode").Value(entity.Id),
                        p => p.Name("name").Value(entity.Name),
                        p => p.Name("isActive").Value(entity.IsActive),
                        p => p.Name("updatedBy").Value(entity.UpdatedBy)
                    );
            }
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

        public bool Activate(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user, "Activate");

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleUpdate(command);
            }
        }

        public async Task<bool> ActivateAsync(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user, "Activate");

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleUpdateAsync(command);
            }
        }

        public bool Inactivate(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user, "Inactivate");

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleUpdate(command);
            }
        }

        public async Task<bool> InactivateAsync(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user, "Inactivate");

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleUpdateAsync(command);
            }
        }

        protected override Command CreateDeleteLinksCommand(Country entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_UnlinkCapitalCity]")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id)
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
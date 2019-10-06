using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        protected override Command CreateUpdateCommand(Country entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
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
            var command = CreateActivateCommand(entity, user);

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
            var command = CreateActivateCommand(entity, user);

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

        protected Command CreateActivateCommand(Country entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Activate]")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id),
                    p => p.Name("isActive").Value(entity.IsActive),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
                );
        }

        public bool Deactivate(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeactivateCommand(entity, user);

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

        public async Task<bool> DeactivateAsync(Country entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeactivateCommand(entity, user);

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

        protected Command CreateDeactivateCommand(Country entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Deactivate]")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id),
                    p => p.Name("isActive").Value(entity.IsActive),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
                );
        }

        protected override Command CreateDeleteCollectionCommand(Country entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_DeleteCapitalCity]")
                .Parameters(
                    p => p.Name("countryCode").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
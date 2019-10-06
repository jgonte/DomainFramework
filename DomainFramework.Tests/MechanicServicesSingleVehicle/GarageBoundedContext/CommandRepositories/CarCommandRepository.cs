using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarCommandRepository : EntityCommandRepository<Car>
    {
        protected override Command CreateInsertCommand(Car entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Car>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Insert]")
                .Parameters(
                    p => p.Name("passengers").Value(entity.Passengers),
                    p => p.Name("model").Value(entity.Model),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("mechanicId").Value(entity.MechanicId)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Car>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Car>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Car entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Update]")
                .Parameters(
                    p => p.Name("carId").Value(entity.Id),
                    p => p.Name("passengers").Value(entity.Passengers),
                    p => p.Name("model").Value(entity.Model),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("mechanicId").Value(entity.MechanicId)
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

        protected override Command CreateDeleteCommand(Car entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Delete]")
                .Parameters(
                    p => p.Name("carId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Car entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_DeleteDoors]")
                .Parameters(
                    p => p.Name("carId").Value(entity.Id)
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
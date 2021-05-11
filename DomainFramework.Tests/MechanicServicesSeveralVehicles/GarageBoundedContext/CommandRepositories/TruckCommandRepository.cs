using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class TruckCommandRepository : EntityCommandRepository<Truck>
    {
        protected override Command CreateInsertCommand(Truck entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Truck>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Insert]")
                .Parameters(
                    p => p.Name("weight").Value(entity.Weight),
                    p => p.Name("model").Value(entity.Model),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var mechanicDependency = (Mechanic)dependencies?.SingleOrDefault()?.Entity;

                    if (mechanicDependency != null)
                    {
                        entity.MechanicId = mechanicDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("mechanicId").Value(entity.MechanicId)
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
            ((SingleQuery<Truck>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Truck>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Truck entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Update]")
                .Parameters(
                    p => p.Name("truckId").Value(entity.Id),
                    p => p.Name("weight").Value(entity.Weight),
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

        protected override Command CreateDeleteCommand(Truck entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Delete]")
                .Parameters(
                    p => p.Name("truckId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(Truck entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "UnlinkMechanicFromVehicle": return Command
                    .NonQuery()
                    .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                    .StoredProcedure("[GarageBoundedContext].[pVehicle_UnlinkMechanic]")
                    .Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
                    );

                case "DeleteCylindersFromVehicle": return Command
                    .NonQuery()
                    .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                    .StoredProcedure("[GarageBoundedContext].[pVehicle_DeleteCylinders]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
                    );

                case "DeleteInspectionsFromTruck": return Command
                    .NonQuery()
                    .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                    .StoredProcedure("[GarageBoundedContext].[pTruck_DeleteInspections]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("truckId").Value(entity.Id)
                    );

                default: throw new InvalidOperationException();
            }
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
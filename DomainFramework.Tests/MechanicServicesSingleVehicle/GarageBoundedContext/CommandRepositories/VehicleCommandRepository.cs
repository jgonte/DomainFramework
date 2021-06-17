using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleCommandRepository : EntityCommandRepository<Vehicle>
    {
        protected override Command CreateInsertCommand(Vehicle entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Insert]")
                .Parameters(
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
                .RecordInstance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Vehicle>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Vehicle>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Vehicle entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Update]")
                .Parameters(
                    p => p.Name("vehicleId").Value(entity.Id),
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

        protected override Command CreateDeleteCommand(Vehicle entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Delete]")
                .Parameters(
                    p => p.Name("vehicleId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(Vehicle entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "UnlinkMechanicFromVehicle": return Command
                    .NonQuery()
                    .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                    .StoredProcedure("[GarageBoundedContext].[pVehicle_UnlinkMechanic]")
                    .Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
                    );

                case "DeleteCylindersFromVehicle": return Command
                    .NonQuery()
                    .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                    .StoredProcedure("[GarageBoundedContext].[pVehicle_DeleteCylinders]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
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
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Vehicle_Cylinders_CommandRepository : LinkedValueObjectCommandRepository<Cylinder>
    {
        protected override Command CreateInsertCommand(Cylinder valueObject, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pInsert_Cylinders_For_Vehicle]")
                .Parameters(
                    p => p.Name("diameter").Value(valueObject.Diameter)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Vehicle)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
                    );
                });
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateDeleteCollectionCommand(IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pDelete_Cylinders_For_Vehicle]")
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Vehicle)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("vehicleId").Value(entity.Id)
                    );
                });
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

        public static void RegisterFactory(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterCommandRepositoryFactory<RepositoryKey>(() => new Vehicle_Cylinders_CommandRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
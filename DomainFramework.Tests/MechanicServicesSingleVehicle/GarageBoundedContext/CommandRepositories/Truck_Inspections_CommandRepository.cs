using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Truck_Inspections_CommandRepository : LinkedValueObjectCommandRepository<Inspection>
    {
        protected override Command CreateInsertCommand(Inspection valueObject, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pInsert_Inspections_For_Truck]")
                .Parameters(
                    p => p.Name("date").Value(valueObject.Date)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Truck)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("truckId").Value(entity.Id)
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
                .StoredProcedure("[GarageBoundedContext].[pDelete_Inspections_For_Truck]")
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Truck)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("truckId").Value(entity.Id)
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
            context.RegisterCommandRepositoryFactory<RepositoryKey>(() => new Truck_Inspections_CommandRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
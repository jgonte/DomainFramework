using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class Car_Doors_CommandRepository : LinkedValueObjectCommandRepository<Door>
    {
        protected override Command CreateInsertCommand(Door valueObject, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pInsert_Doors_For_Car]")
                .Parameters(
                    p => p.Name("number").Value(valueObject.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Car)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("carId").Value(entity.Id)
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
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pDelete_Doors_For_Car]")
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (Car)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("carId").Value(entity.Id)
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
            context.RegisterCommandRepositoryFactory<RepositoryKey>(() => new Car_Doors_CommandRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
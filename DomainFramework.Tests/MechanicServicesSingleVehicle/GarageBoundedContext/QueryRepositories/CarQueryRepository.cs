using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarQueryRepository : EntityQueryRepository<Car, int>
    {
        public override (int, IEnumerable<Car>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Car>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Car> GetAll()
        {
            var result = Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Car>> GetAllAsync()
        {
            var result = await Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Car GetById(int carId)
        {
            var result = Query<Car>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetById]")
                .Parameters(
                    p => p.Name("carId").Value(carId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Car> GetByIdAsync(int carId)
        {
            var result = await Query<Car>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetById]")
                .Parameters(
                    p => p.Name("carId").Value(carId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public Vehicle GetVehicleForMechanic(int mechanicId)
        {
            var result = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetVehicle]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .Execute();

            return result.Record;
        }

        public async Task<Vehicle> GetVehicleForMechanicAsync(int mechanicId)
        {
            var result = await Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetVehicle]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Car>(new CarQueryRepository());
        }

    }
}
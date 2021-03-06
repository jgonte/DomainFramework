using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleQueryRepository : EntityQueryRepository<Vehicle, int>
    {
        public override (int, IEnumerable<Vehicle>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Vehicle>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Vehicle> GetAll()
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAll]")
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAll]")
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public override Vehicle GetById(int vehicleId)
        {
            var result = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetById]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
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

        public async override Task<Vehicle> GetByIdAsync(int vehicleId)
        {
            var result = await Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetById]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
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
            context.RegisterQueryRepository<Vehicle>(new VehicleQueryRepository());
        }

    }
}
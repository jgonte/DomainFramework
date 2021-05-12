using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class TruckQueryRepository : EntityQueryRepository<Truck, int>
    {
        public override (int, IEnumerable<Truck>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Truck>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Truck> GetAll()
        {
            var result = Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Truck>> GetAllAsync()
        {
            var result = await Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Truck GetById(int truckId)
        {
            var result = Query<Truck>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Truck> GetByIdAsync(int truckId)
        {
            var result = await Query<Truck>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
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
            context.RegisterQueryRepository<Truck>(new TruckQueryRepository());
        }

    }
}
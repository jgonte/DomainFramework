using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class TruckQueryRepository : EntityQueryRepository<Truck, int>
    {
        public override (int, IEnumerable<Truck>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Truck>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Truck>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Truck>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Truck> GetAll()
        {
            var result = Query<Truck>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Truck>> GetAllAsync()
        {
            var result = await Query<Truck>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Truck GetById(int truckId)
        {
            var result = Query<Truck>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Truck> GetByIdAsync(int truckId)
        {
            var result = await Query<Truck>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Vehicle> GetAllVehiclesForMechanic(int mechanicId)
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetAllVehicles]")
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

            return result.Data;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesForMechanicAsync(int mechanicId)
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetAllVehicles]")
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

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Truck>(new TruckQueryRepository());
        }

    }
}
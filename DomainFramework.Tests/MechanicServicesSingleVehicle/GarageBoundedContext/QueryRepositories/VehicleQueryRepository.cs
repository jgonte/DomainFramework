using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleQueryRepository : EntityQueryRepository<Vehicle, int?>
    {
        public override Vehicle GetById(int? vehicleId, IAuthenticatedUser user)
        {
            var result = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetById]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<Vehicle>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Vehicle_Cylinders_QueryRepository();

                    entity.Cylinders = repository.Get(entity.Id, user).ToList();

                    if (entity is Truck)
                    {
                        var repository1 = new Truck_Inspections_QueryRepository();

                        var truck = (Truck)entity;

                        truck.Inspections = repository1.Get(entity.Id, user).ToList();
                    }

                    if (entity is Car)
                    {
                        var repository2 = new Car_Doors_QueryRepository();

                        var car = (Car)entity;

                        car.Doors = repository2.Get(entity.Id, user).ToList();
                    }
                })
                .Execute();

            return result.Data;
        }

        public async override Task<Vehicle> GetByIdAsync(int? vehicleId, IAuthenticatedUser user)
        {
            var result = await Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetById]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<Vehicle>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Vehicle_Cylinders_QueryRepository();

                    var valueObjects = await repository.GetAsync(entity.Id, user);

                    entity.Cylinders = valueObjects.ToList();

                    if (entity is Truck)
                    {
                        var repository1 = new Truck_Inspections_QueryRepository();

                        var valueObjects1 = await repository1.GetAsync(entity.Id, user);

                        var truck = (Truck)entity;

                        truck.Inspections = valueObjects1.ToList();
                    }

                    if (entity is Car)
                    {
                        var repository2 = new Car_Doors_QueryRepository();

                        var valueObjects2 = await repository2.GetAsync(entity.Id, user);

                        var car = (Car)entity;

                        car.Doors = valueObjects2.ToList();
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Vehicle> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<Vehicle>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Vehicle_Cylinders_QueryRepository();

                        entity.Cylinders = repository.Get(entity.Id, user).ToList();

                        if (entity is Truck)
                        {
                            var repository1 = new Truck_Inspections_QueryRepository();

                            var truck = (Truck)entity;

                            truck.Inspections = repository1.Get(entity.Id, user).ToList();
                        }

                        if (entity is Car)
                        {
                            var repository2 = new Car_Doors_QueryRepository();

                            var car = (Car)entity;

                            car.Doors = repository2.Get(entity.Id, user).ToList();
                        }
                    }
                })
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Vehicle>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<Vehicle>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Vehicle_Cylinders_QueryRepository();

                        var valueObjects = await repository.GetAsync(entity.Id, user);

                        entity.Cylinders = valueObjects.ToList();

                        if (entity is Truck)
                        {
                            var repository1 = new Truck_Inspections_QueryRepository();

                            var valueObjects1 = await repository1.GetAsync(entity.Id, user);

                            var truck = (Truck)entity;

                            truck.Inspections = valueObjects1.ToList();
                        }

                        if (entity is Car)
                        {
                            var repository2 = new Car_Doors_QueryRepository();

                            var valueObjects2 = await repository2.GetAsync(entity.Id, user);

                            var car = (Car)entity;

                            car.Doors = valueObjects2.ToList();
                        }
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public Vehicle GetVehicleForMechanic(int? mechanicId, IAuthenticatedUser user)
        {
            var result = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetVehicle_ForMechanic]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<Vehicle>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Vehicle_Cylinders_QueryRepository();

                    entity.Cylinders = repository.Get(entity.Id, user).ToList();

                    if (entity is Truck)
                    {
                        var repository1 = new Truck_Inspections_QueryRepository();

                        var truck = (Truck)entity;

                        truck.Inspections = repository1.Get(entity.Id, user).ToList();
                    }

                    if (entity is Car)
                    {
                        var repository2 = new Car_Doors_QueryRepository();

                        var car = (Car)entity;

                        car.Doors = repository2.Get(entity.Id, user).ToList();
                    }
                })
                .Execute();

            return result.Data;
        }

        public async Task<Vehicle> GetVehicleForMechanicAsync(int? mechanicId, IAuthenticatedUser user)
        {
            var result = await Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetVehicle_ForMechanic]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Truck)).Index(1),
                    tm => tm.Type(typeof(Car)).Index(2),
                    tm => tm.Type(typeof(Vehicle)).Index(3)
                )
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<Vehicle>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Vehicle_Cylinders_QueryRepository();

                    var valueObjects = await repository.GetAsync(entity.Id, user);

                    entity.Cylinders = valueObjects.ToList();

                    if (entity is Truck)
                    {
                        var repository1 = new Truck_Inspections_QueryRepository();

                        var valueObjects1 = await repository1.GetAsync(entity.Id, user);

                        var truck = (Truck)entity;

                        truck.Inspections = valueObjects1.ToList();
                    }

                    if (entity is Car)
                    {
                        var repository2 = new Car_Doors_QueryRepository();

                        var valueObjects2 = await repository2.GetAsync(entity.Id, user);

                        var car = (Car)entity;

                        car.Doors = valueObjects2.ToList();
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Vehicle>(new VehicleQueryRepository());
        }

    }
}
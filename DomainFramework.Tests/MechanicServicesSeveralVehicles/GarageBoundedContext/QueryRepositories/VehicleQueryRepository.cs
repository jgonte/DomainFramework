using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class VehicleQueryRepository : EntityQueryRepository<Vehicle, int?>
    {
        public override (int, IEnumerable<Vehicle>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
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
                        if (entity is Truck)
                        {
                            var repository = new Truck_Inspections_QueryRepository();

                            var truck = (Truck)entity;

                            truck.Inspections = repository.GetAll(entity.Id).ToList();
                        }

                        if (entity is Car)
                        {
                            var repository1 = new Car_Doors_QueryRepository();

                            var car = (Car)entity;

                            car.Doors = repository1.GetAll(entity.Id).ToList();
                        }
                    }
                })
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Vehicle>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
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
                        if (entity is Truck)
                        {
                            var repository = new Truck_Inspections_QueryRepository();

                            var valueObjects = await repository.GetAllAsync(entity.Id);

                            var truck = (Truck)entity;

                            truck.Inspections = valueObjects.ToList();
                        }

                        if (entity is Car)
                        {
                            var repository1 = new Car_Doors_QueryRepository();

                            var valueObjects1 = await repository1.GetAllAsync(entity.Id);

                            var car = (Car)entity;

                            car.Doors = valueObjects1.ToList();
                        }
                    }
                })
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Vehicle> GetAll()
        {
            var result = Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAll]")
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
                        if (entity is Truck)
                        {
                            var repository = new Truck_Inspections_QueryRepository();

                            var truck = (Truck)entity;

                            truck.Inspections = repository.GetAll(entity.Id).ToList();
                        }

                        if (entity is Car)
                        {
                            var repository1 = new Car_Doors_QueryRepository();

                            var car = (Car)entity;

                            car.Doors = repository1.GetAll(entity.Id).ToList();
                        }
                    }
                })
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var result = await Query<Vehicle>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAll]")
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
                        if (entity is Truck)
                        {
                            var repository = new Truck_Inspections_QueryRepository();

                            var valueObjects = await repository.GetAllAsync(entity.Id);

                            var truck = (Truck)entity;

                            truck.Inspections = valueObjects.ToList();
                        }

                        if (entity is Car)
                        {
                            var repository1 = new Car_Doors_QueryRepository();

                            var valueObjects1 = await repository1.GetAllAsync(entity.Id);

                            var car = (Car)entity;

                            car.Doors = valueObjects1.ToList();
                        }
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public override Vehicle GetById(int? vehicleId)
        {
            var result = Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
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

                    if (entity is Truck)
                    {
                        var repository = new Truck_Inspections_QueryRepository();

                        var truck = (Truck)entity;

                        truck.Inspections = repository.GetAll(entity.Id).ToList();
                    }

                    if (entity is Car)
                    {
                        var repository1 = new Car_Doors_QueryRepository();

                        var car = (Car)entity;

                        car.Doors = repository1.GetAll(entity.Id).ToList();
                    }
                })
                .Execute();

            return result.Data;
        }

        public async override Task<Vehicle> GetByIdAsync(int? vehicleId)
        {
            var result = await Query<Vehicle>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
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

                    if (entity is Truck)
                    {
                        var repository = new Truck_Inspections_QueryRepository();

                        var valueObjects = await repository.GetAllAsync(entity.Id);

                        var truck = (Truck)entity;

                        truck.Inspections = valueObjects.ToList();
                    }

                    if (entity is Car)
                    {
                        var repository1 = new Car_Doors_QueryRepository();

                        var valueObjects1 = await repository1.GetAllAsync(entity.Id);

                        var car = (Car)entity;

                        car.Doors = valueObjects1.ToList();
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
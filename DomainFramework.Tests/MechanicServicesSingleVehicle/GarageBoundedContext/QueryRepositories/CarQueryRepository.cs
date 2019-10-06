using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarQueryRepository : EntityQueryRepository<Car, int?>
    {
        public override Car GetById(int? carId, IAuthenticatedUser user)
        {
            var result = Query<Car>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetById]")
                .Parameters(
                    p => p.Name("carId").Value(carId.Value)
                )
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<Car>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Car_Doors_QueryRepository();

                    entity.Doors = repository.Get(entity.Id, user).ToList();
                })
                .Execute();

            return result.Data;
        }

        public async override Task<Car> GetByIdAsync(int? carId, IAuthenticatedUser user)
        {
            var result = await Query<Car>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetById]")
                .Parameters(
                    p => p.Name("carId").Value(carId.Value)
                )
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<Car>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Car_Doors_QueryRepository();

                    var valueObjects = await repository.GetAsync(entity.Id, user);

                    entity.Doors = valueObjects.ToList();
                })
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Car> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Car_Doors_QueryRepository();

                        entity.Doors = repository.Get(entity.Id, user).ToList();
                    }
                })
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Car>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Car>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Car_Doors_QueryRepository();

                        var valueObjects = await repository.GetAsync(entity.Id, user);

                        entity.Doors = valueObjects.ToList();
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Car>(new CarQueryRepository());
        }

    }
}
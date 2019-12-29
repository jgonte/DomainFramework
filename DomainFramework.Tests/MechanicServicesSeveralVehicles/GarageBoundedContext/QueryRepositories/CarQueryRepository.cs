using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class CarQueryRepository : EntityQueryRepository<Car, int?>
    {
        public override (int, IEnumerable<Car>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Car>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Car>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Car>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Car> GetAll()
        {
            var result = Query<Car>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAll]")
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            var result = await Query<Car>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAll]")
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<Car>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public override Car GetById(int? carId)
        {
            var result = Query<Car>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
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
                })
                .Execute();

            return result.Data;
        }

        public async override Task<Car> GetByIdAsync(int? carId)
        {
            var result = await Query<Car>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
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
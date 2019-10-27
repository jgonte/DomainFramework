using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class TruckQueryRepository : EntityQueryRepository<Truck, int?>
    {
        public override Truck GetById(int? truckId, IAuthenticatedUser user)
        {
            var result = Query<Truck>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<Truck>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Truck_Inspections_QueryRepository();

                    entity.Inspections = repository.Get(entity.Id, user).ToList();
                })
                .Execute();

            return result.Data;
        }

        public async override Task<Truck> GetByIdAsync(int? truckId, IAuthenticatedUser user)
        {
            var result = await Query<Truck>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetById]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<Truck>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }

                    var repository = new Truck_Inspections_QueryRepository();

                    var valueObjects = await repository.GetAsync(entity.Id, user);

                    entity.Inspections = valueObjects.ToList();
                })
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Truck>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<Truck>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Truck_Inspections_QueryRepository();

                        entity.Inspections = repository.Get(entity.Id, user).ToList();
                    }
                })
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Truck>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Truck>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<Truck>)cmd;

                    foreach (var entity in query.Data)
                    {
                        var repository = new Truck_Inspections_QueryRepository();

                        var valueObjects = await repository.GetAsync(entity.Id, user);

                        entity.Inspections = valueObjects.ToList();
                    }
                })
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Truck>(new TruckQueryRepository());
        }

    }
}
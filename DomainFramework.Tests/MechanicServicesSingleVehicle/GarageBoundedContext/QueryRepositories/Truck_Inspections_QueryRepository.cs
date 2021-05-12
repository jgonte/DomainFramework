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
    public class Truck_Inspections_QueryRepository : ValueObjectQueryRepository<int, Inspection>
    {
        public override (int, IEnumerable<Inspection>) Get(int truckId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetInspections]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Inspection>)> GetAsync(int truckId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetInspections]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Inspection> GetAll(int truckId)
        {
            var result = Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAllInspections]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Inspection>> GetAllAsync(int truckId)
        {
            var result = await Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pTruck_GetAllInspections]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<RepositoryKey>(new Truck_Inspections_QueryRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
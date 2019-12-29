using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Truck_Inspections_QueryRepository : ValueObjectQueryRepository<int?, Inspection>
    {
        public override (int, IEnumerable<Inspection>) Get(int? truckId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Inspections_For_Truck]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Inspection>)> GetAsync(int? truckId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Inspections_For_Truck]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Inspection> GetAll(int? truckId)
        {
            throw new NotImplementedException();
        }

        public async override Task<IEnumerable<Inspection>> GetAllAsync(int? truckId)
        {
            throw new NotImplementedException();
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
using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Truck_Inspections_QueryRepository : ValueObjectQueryRepository<int?, Inspection>
    {
        public override IEnumerable<Inspection> Get(int? truckId, IAuthenticatedUser user)
        {
            var result = Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Inspections_For_Truck]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Inspection>> GetAsync(int? truckId, IAuthenticatedUser user)
        {
            var result = await Query<Inspection>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Inspections_For_Truck]")
                .Parameters(
                    p => p.Name("truckId").Value(truckId.Value)
                )
                .ExecuteAsync();

            return result.Data;
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
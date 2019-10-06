using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Vehicle_Cylinders_QueryRepository : ValueObjectQueryRepository<int?, Cylinder>
    {
        public override IEnumerable<Cylinder> Get(int? vehicleId, IAuthenticatedUser user)
        {
            var result = Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Cylinders_For_Vehicle]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Cylinder>> GetAsync(int? vehicleId, IAuthenticatedUser user)
        {
            var result = await Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Cylinders_For_Vehicle]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<RepositoryKey>(new Vehicle_Cylinders_QueryRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
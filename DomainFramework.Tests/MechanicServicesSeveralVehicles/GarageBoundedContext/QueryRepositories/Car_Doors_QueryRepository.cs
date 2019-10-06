using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class Car_Doors_QueryRepository : ValueObjectQueryRepository<int?, Door>
    {
        public override IEnumerable<Door> Get(int? carId, IAuthenticatedUser user)
        {
            var result = Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Doors_For_Car]")
                .Parameters(
                    p => p.Name("carId").Value(carId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Door>> GetAsync(int? carId, IAuthenticatedUser user)
        {
            var result = await Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGet_Doors_For_Car]")
                .Parameters(
                    p => p.Name("carId").Value(carId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<RepositoryKey>(new Car_Doors_QueryRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
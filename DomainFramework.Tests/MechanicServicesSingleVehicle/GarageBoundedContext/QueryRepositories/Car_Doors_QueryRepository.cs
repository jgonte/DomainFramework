using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Car_Doors_QueryRepository : ValueObjectQueryRepository<int?, Door>
    {
        public override (int, IEnumerable<Door>) Get(int? carId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public async override Task<(int, IEnumerable<Door>)> GetAsync(int? carId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Door> GetAll(int? carId)
        {
            var result = Query<Door>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGetAll_Doors_For_Car]")
                .Parameters(
                    p => p.Name("carId").Value(carId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Door>> GetAllAsync(int? carId)
        {
            var result = await Query<Door>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGetAll_Doors_For_Car]")
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
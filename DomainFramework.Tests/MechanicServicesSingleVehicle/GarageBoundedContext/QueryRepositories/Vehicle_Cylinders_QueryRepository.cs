using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Vehicle_Cylinders_QueryRepository : ValueObjectQueryRepository<int?, Cylinder>
    {
        public override (int, IEnumerable<Cylinder>) Get(int? vehicleId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public async override Task<(int, IEnumerable<Cylinder>)> GetAsync(int? vehicleId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Cylinder> GetAll(int? vehicleId)
        {
            var result = Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGetAll_Cylinders_For_Vehicle]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Cylinder>> GetAllAsync(int? vehicleId)
        {
            var result = await Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pGetAll_Cylinders_For_Vehicle]")
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
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class Vehicle_Cylinders_QueryRepository : ValueObjectQueryRepository<int, Cylinder>
    {
        public override (int, IEnumerable<Cylinder>) Get(int vehicleId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetCylinders]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Cylinder>)> GetAsync(int vehicleId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetCylinders]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Cylinder> GetAll(int vehicleId)
        {
            var result = Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAllCylinders]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Cylinder>> GetAllAsync(int vehicleId)
        {
            var result = await Query<Cylinder>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetAllCylinders]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
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
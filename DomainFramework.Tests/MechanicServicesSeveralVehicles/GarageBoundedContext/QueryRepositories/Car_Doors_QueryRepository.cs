using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class Car_Doors_QueryRepository : ValueObjectQueryRepository<int, Door>
    {
        public override (int, IEnumerable<Door>) Get(int carId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetDoors]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("carId").Value(carId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Door>)> GetAsync(int carId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetDoors]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("carId").Value(carId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Door> GetAll(int carId)
        {
            var result = Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAllDoors]")
                .Parameters(
                    p => p.Name("carId").Value(carId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Door>> GetAllAsync(int carId)
        {
            var result = await Query<Door>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pCar_GetAllDoors]")
                .Parameters(
                    p => p.Name("carId").Value(carId)
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
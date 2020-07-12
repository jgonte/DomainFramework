using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class MechanicQueryRepository : EntityQueryRepository<Mechanic, int>
    {
        public override (int, IEnumerable<Mechanic>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Mechanic>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Mechanic> GetAll()
        {
            var result = Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Mechanic>> GetAllAsync()
        {
            var result = await Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Mechanic GetById(int mechanicId)
        {
            var result = Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetById]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Mechanic> GetByIdAsync(int mechanicId)
        {
            var result = await Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetById]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Mechanic GetMechanicForVehicle(int vehicleId)
        {
            var result = Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetMechanic]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Mechanic> GetMechanicForVehicleAsync(int vehicleId)
        {
            var result = await Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSingleVehicleConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pVehicle_GetMechanic]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Mechanic>(new MechanicQueryRepository());
        }

    }
}
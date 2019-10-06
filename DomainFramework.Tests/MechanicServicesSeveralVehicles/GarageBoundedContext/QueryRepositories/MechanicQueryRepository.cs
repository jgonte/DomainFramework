using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class MechanicQueryRepository : EntityQueryRepository<Mechanic, int?>
    {
        public override Mechanic GetById(int? mechanicId, IAuthenticatedUser user)
        {
            var result = Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetById]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Mechanic> GetByIdAsync(int? mechanicId, IAuthenticatedUser user)
        {
            var result = await Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetById]")
                .Parameters(
                    p => p.Name("mechanicId").Value(mechanicId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Mechanic> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Mechanic>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Mechanic>
                .Collection()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public Mechanic GetMechanicForVehicle(int? vehicleId, IAuthenticatedUser user)
        {
            var result = Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetMechanic_ForVehicle]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Mechanic> GetMechanicForVehicleAsync(int? vehicleId, IAuthenticatedUser user)
        {
            var result = await Query<Mechanic>
                .Single()
                .Connection(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName())
                .StoredProcedure("[GarageBoundedContext].[pMechanic_GetMechanic_ForVehicle]")
                .Parameters(
                    p => p.Name("vehicleId").Value(vehicleId.Value)
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
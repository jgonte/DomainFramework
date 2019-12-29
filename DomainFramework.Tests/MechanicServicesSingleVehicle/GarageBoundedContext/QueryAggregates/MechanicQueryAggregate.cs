using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class MechanicQueryAggregate : GetByIdQueryAggregate<Mechanic, int?, MechanicOutputDto>
    {
        public MechanicQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName());

            MechanicQueryRepository.Register(context);

            VehicleQueryRepository.Register(context);

            RepositoryContext = context;
        }

        public override void PopulateDto(Mechanic entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            //OutputDto.Vehicle = GetVehicleDto();
        }

    }
}
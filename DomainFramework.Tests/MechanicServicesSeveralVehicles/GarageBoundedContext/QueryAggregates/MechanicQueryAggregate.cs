using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class MechanicQueryAggregate : GetByIdQueryAggregate<Mechanic, int?, MechanicOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<Vehicle> GetVehiclesQueryOperation { get; }

        public IEnumerable<Vehicle> Vehicles => GetVehiclesQueryOperation.LinkedEntities;

        public MechanicQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName());

            MechanicQueryRepository.Register(context);

            VehicleQueryRepository.Register(context);

            RepositoryContext = context;

            GetVehiclesQueryOperation = new GetCollectionLinkedEntityQueryOperation<Vehicle>
            {
                GetLinkedEntities = (repository, entity, user) => ((VehicleQueryRepository)repository).GetVehiclesForMechanic(RootEntity.Id, user).ToList(),
                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((VehicleQueryRepository)repository).GetVehiclesForMechanicAsync(RootEntity.Id, user);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(GetVehiclesQueryOperation);
        }

        public List<VehicleOutputDto> GetVehiclesDtos()
        {
            if (Vehicles?.Any() == true)
            {
                var vehicles = new List<VehicleOutputDto>();

                foreach (var vehicle in Vehicles)
                {
                    if (vehicle is Truck)
                    {
                        var truck = (Truck)vehicle;

                        var dto = new TruckOutputDto
                        {
                            Id = truck.Id.Value,
                            Weight = truck.Weight,
                            Model = truck.Model,
                            MechanicId = truck.MechanicId,
                            Inspections = truck.Inspections.Select(vo => new InspectionOutputDto
                            {
                                Date = vo.Date
                            }).ToList(),
                            Cylinders = truck.Cylinders.Select(vo => new CylinderOutputDto
                            {
                                Diameter = vo.Diameter
                            }).ToList()
                        };

                        vehicles.Add(dto);
                    }
                    else if (vehicle is Car)
                    {
                        var car = (Car)vehicle;

                        var dto = new CarOutputDto
                        {
                            Id = car.Id.Value,
                            Passengers = car.Passengers,
                            Model = car.Model,
                            MechanicId = car.MechanicId,
                            Doors = car.Doors.Select(vo => new DoorOutputDto
                            {
                                Number = vo.Number
                            }).ToList(),
                            Cylinders = car.Cylinders.Select(vo => new CylinderOutputDto
                            {
                                Diameter = vo.Diameter
                            }).ToList()
                        };

                        vehicles.Add(dto);
                    }
                    else
                    {
                        var dto = new VehicleOutputDto
                        {
                            Id = vehicle.Id.Value,
                            Model = vehicle.Model,
                            MechanicId = vehicle.MechanicId,
                            Cylinders = vehicle.Cylinders.Select(vo => new CylinderOutputDto
                            {
                                Diameter = vo.Diameter
                            }).ToList()
                        };

                        vehicles.Add(dto);
                    }
                }

                return vehicles;
            }

            return null;
        }

        public override void PopulateDto(Mechanic entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            OutputDto.Vehicles = GetVehiclesDtos();
        }

    }
}
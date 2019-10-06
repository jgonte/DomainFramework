using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class TruckInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public int Weight { get; set; }

        public string Model { get; set; }

        public int MechanicId { get; set; }

        public List<InspectionInputDto> Inspections { get; set; } = new List<InspectionInputDto>();

        public List<CylinderInputDto> Cylinders { get; set; } = new List<CylinderInputDto>();

        public void Validate(ValidationResult result)
        {
            Weight.ValidateNotZero(result, nameof(Weight));

            Model.ValidateNotEmpty(result, nameof(Model));

            Model.ValidateMaxLength(result, nameof(Model), 50);

            MechanicId.ValidateNotZero(result, nameof(MechanicId));

            var inspectionsCount = (uint)Inspections.Where(item => item != null).Count();

            inspectionsCount.ValidateCountIsBetween(result, 2, 12, nameof(Inspections));

            foreach (var inspection in Inspections)
            {
                inspection.Validate(result);
            }

            var cylindersCount = (uint)Cylinders.Where(item => item != null).Count();

            cylindersCount.ValidateCountIsBetween(result, 2, 12, nameof(Cylinders));

            foreach (var cylinder in Cylinders)
            {
                cylinder.Validate(result);
            }
        }

    }
}
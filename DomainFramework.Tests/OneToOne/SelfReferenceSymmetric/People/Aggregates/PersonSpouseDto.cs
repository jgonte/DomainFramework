using DomainFramework.Core;
using Utilities.Validation;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    public class PersonSpouseDto : IInputDataTransferObject
    {
        public string FirstName { get; internal set; }

        public string SpouseName { get; internal set; }

        public void Validate(ValidationResult result)
        {
            FirstName.ValidateNotEmpty(result, nameof(FirstName));

            SpouseName.ValidateNotEmpty(result, nameof(SpouseName));
        }
    }
}
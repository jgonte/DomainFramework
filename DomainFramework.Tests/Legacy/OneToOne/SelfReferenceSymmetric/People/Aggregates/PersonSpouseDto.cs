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
            FirstName.ValidateRequired(result, nameof(FirstName));

            SpouseName.ValidateRequired(result, nameof(SpouseName));
        }
    }
}
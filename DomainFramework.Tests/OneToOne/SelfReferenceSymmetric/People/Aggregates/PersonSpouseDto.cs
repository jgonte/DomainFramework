using DomainFramework.Core;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    public class PersonSpouseDto : InputDto
    {
        public string FirstName { get; internal set; }

        public string SpouseName { get; internal set; }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
namespace DomainFramework.Tests.Entitites
{
    class StudentRole : IPersonRole
    {
        public int? PersonId { get; set; }

        public string StudentNumber { get; set; }
    }
}

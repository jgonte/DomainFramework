namespace DomainFramework.Tests
{
    internal class EmployeeDto
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public EmployeeDto()
        {
        }

        public EmployeeDto(PersonEntity3 employee)
        {
            Id = employee.Id;

            FirstName = employee.FirstName;
        }
    }
}
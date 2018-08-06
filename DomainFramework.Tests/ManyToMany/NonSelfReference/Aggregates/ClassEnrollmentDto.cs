using System.Collections.Generic;

namespace DomainFramework.Tests
{
    public class ClassEnrollmentDto
    {
        public string Name { get; internal set; }

        public List<StudentToEnrollDto> StudentsToEnroll { get; internal set; }
    }
}
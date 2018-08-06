using System;

namespace DomainFramework.Tests
{
    public class StudentToEnrollDto
    {
        public string FirstName { get; internal set; }

        public DateTime StartedDateTime { get; internal set; }
    }
}
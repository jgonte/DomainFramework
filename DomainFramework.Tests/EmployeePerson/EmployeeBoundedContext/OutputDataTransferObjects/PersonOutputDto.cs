using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

    }
}
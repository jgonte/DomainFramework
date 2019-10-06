using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PersonOutputDto Spouse { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

    }
}
using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProviderEmployeeId { get; set; }

        public PhoneNumberOutputDto CellPhone { get; set; }

    }
}
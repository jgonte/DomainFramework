using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

    }
}
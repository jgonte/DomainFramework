using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
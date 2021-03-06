using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class EmployeeOutputDto : IOutputDataTransferObject
    {
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public int? SupervisorId { get; set; }

    }
}
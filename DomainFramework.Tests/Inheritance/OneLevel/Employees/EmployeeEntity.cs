using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    public class EmployeeEntity : Entity<int?>
    {
        public Decimal Salary { get; set; }
    }
}

using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class PhoneNumberOutputDto : IOutputDataTransferObject
    {
        public string AreaCode { get; set; }

        public string Exchange { get; set; }

        public string Number { get; set; }

    }
}
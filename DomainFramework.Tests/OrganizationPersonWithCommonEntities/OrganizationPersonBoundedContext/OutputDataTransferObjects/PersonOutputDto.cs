using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PhoneOutputDto> Phones { get; set; }

        public AddressOutputDto Address { get; set; }

    }
}
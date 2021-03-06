using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class OrganizationOutputDto : IOutputDataTransferObject
    {
        public int OrganizationId { get; set; }

        public string Name { get; set; }

        public IEnumerable<PhoneOutputDto> Phones { get; set; }

        public AddressOutputDto Address { get; set; }

    }
}
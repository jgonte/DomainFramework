using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class AddressOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonId { get; set; }

    }
}
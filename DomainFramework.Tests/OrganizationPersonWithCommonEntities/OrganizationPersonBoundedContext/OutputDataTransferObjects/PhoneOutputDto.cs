using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class PhoneOutputDto : IOutputDataTransferObject
    {
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonId { get; set; }

    }
}
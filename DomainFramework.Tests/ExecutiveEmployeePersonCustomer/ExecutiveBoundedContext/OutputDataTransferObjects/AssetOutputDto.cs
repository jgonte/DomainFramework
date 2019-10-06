using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class AssetOutputDto : IOutputDataTransferObject
    {
        public int? Number { get; set; }

    }
}
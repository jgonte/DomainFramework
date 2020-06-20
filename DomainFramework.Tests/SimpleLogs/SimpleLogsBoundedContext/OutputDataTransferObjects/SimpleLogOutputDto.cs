using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLogOutputDto : IOutputDataTransferObject
    {
        public int SimpleLogId { get; set; }

        public string MessageType { get; set; }

        public string Message { get; set; }

        public DateTime When { get; set; }

    }
}
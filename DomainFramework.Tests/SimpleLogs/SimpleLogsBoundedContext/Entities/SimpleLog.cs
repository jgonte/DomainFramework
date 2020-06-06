using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLog : Entity<int?>
    {
        public string MessageType { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// When it was logged
        /// </summary>
        public DateTime When { get; set; }

    }
}
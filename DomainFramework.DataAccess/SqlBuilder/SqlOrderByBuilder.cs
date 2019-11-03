using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.DataAccess
{
    public class SqlOrderByBuilder
    {
        public SqlOrderByBuilder(DatabaseDriver databaseDriver)
        {
        }

        public string Build(Queue<SorterNode> sorters) => string.Join(", ", sorters.Select(s => s.ToString()));
    }
}
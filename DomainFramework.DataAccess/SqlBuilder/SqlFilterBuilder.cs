using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Text;

namespace DomainFramework.DataAccess
{
    public class SqlFilterBuilder
    {
        public SqlFilterBuilder(DatabaseDriver databaseDriver)
        {

        }

        public string Build(Queue<FilterNode> filter)
        {
            var builder = new StringBuilder();

            foreach (var filterNode in filter)
            {
                builder.Append(filterNode.ToString());
            }

            return builder.ToString();
        }
    }
}

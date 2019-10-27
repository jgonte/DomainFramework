using System.Collections.Generic;

namespace DomainFramework.Core
{
    public class CollectionQueryParameters
    {
        public int? Top { get; set; }

        public int? Skip { get; set; }

        public List<string> Select { get; set; } = new List<string>();

        public Queue<FilterNode> Filter { get; set; }

        public string OrderBy { get; set; }

        public Dictionary<string, string> ExtraParameters { get; set; } = new Dictionary<string, string>();
    }
}
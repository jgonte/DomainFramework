using System.Collections.Generic;

namespace DomainFramework.Core
{
    public class MultiValueOperatorFilterNode : FilterNode,
        IFieldNameHolder,
        IMultiValue
    {
        public string FieldName { get; set; }

        public List<object> FieldValues { get; set; } = new List<object>();

        public override string ToString() => $"{FieldName} {Name} ({string.Join(", ", FieldValues)})";
    }
}

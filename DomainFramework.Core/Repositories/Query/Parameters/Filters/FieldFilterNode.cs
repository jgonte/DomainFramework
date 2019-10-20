namespace DomainFramework.Core
{
    public abstract class FieldFilterNode : FilterNode
    {
        public string FieldName { get; set; }

        public FilterNode Operator { get; set; }
    }
}

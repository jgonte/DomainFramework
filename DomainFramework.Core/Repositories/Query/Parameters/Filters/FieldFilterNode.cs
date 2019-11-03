namespace DomainFramework.Core
{
    public abstract class FieldFilterNode : FilterNode,
        IFieldNameHolder
    {
        public string FieldName { get; set; }

        public FilterNode Operator { get; set; }
    }
}

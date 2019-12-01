namespace DomainFramework.Core
{
    public class SingleValueFieldFilterNode : FieldFilterNode,
        ISingleValue
    {
        public object FieldValue { get; set; }

        public override string ToString() => $"{FieldName} {Operator.ToString()} {this.GetFieldValue()}";
    }
}

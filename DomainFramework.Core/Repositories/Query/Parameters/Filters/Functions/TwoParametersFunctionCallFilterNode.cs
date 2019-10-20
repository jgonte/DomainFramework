namespace DomainFramework.Core
{
    /// <summary>
    /// Function call that have as parameters the name of the field and the value of the field
    /// </summary>
    public class TwoParametersFunctionCallFilterNode : FilterNode,
        ISingleValue
    {
        public string FieldName { get; set; }

        public object FieldValue { get; set; }

    }
}

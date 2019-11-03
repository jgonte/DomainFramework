namespace DomainFramework.Core
{
    public class ContainsFunctionCallFilterNode : TwoParametersFunctionCallFilterNode
    {
        public ContainsFunctionCallFilterNode()
        {
            Name = Like;
        }

        public override string ToString() => $"{FieldName} {Name} '%{FieldValue.ToString()}%'";
    }
}

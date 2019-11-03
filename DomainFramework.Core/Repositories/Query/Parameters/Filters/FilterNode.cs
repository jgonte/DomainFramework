namespace DomainFramework.Core
{
    public class FilterNode
    {
        public static readonly string Like = "LIKE";

        public string Name { get; protected set; }

        public FilterNode()
        {
        }

        public FilterNode(string name)
        {
            Name = name;
        }

        public static FilterNode BeginGrouping { get; } = new FilterNode(" (");

        public static FilterNode EndGrouping { get; } = new FilterNode(") ");

        public static FilterNode Not { get; } = new FilterNode(" NOT ");

        public static FilterNode And { get; } = new FilterNode(" AND ");

        public static FilterNode Or { get; } = new FilterNode(" OR ");

        public static FilterNode IsEqual { get; } = new FilterNode(" = ");

        public static FilterNode IsNotEqual { get; } = new FilterNode(" != ");

        public override string ToString() => Name;

    }
}
namespace DomainFramework.Core
{
    public class SorterNode
    {
        public string FieldName { get; set; }

        public enum SortingOrders
        {
            Ascending,
            Descending
        }

        public SortingOrders SortingOrder { get; set; }

        public override string ToString() => $"{FieldName} {(SortingOrder == SortingOrders.Descending ? "DESC" : "ASC")}";
    }
}
namespace DomainFramework.Core
{
    public class QueryParameters
    {
        public string Select { get; set; }

        public string Where { get; set; }

        public string OrderBy { get; set; }

        public PagingParameters PagingParameters { get; set; }
    }
}
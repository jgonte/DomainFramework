using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class BookEntity : ContainerEntity<int?, BookData>
    {
        public BookEntity()
        {
        }

        internal BookEntity(BookData data, int? id = null) : base(data, id)
        {
        }       
    }
}

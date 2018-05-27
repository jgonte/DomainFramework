using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class CollectionPageEntityLink : CollectionEntityLink<BookEntity, PageEntity>
    {
        public override void SetForeignKey(BookEntity bookEntity, PageEntity pageEntity)
        {
            ((PageEntity)pageEntity).BookId = ((BookEntity)bookEntity).Id.Value;
        }
    }
}
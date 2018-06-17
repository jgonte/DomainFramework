using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class BookPagesQueryAggregate : QueryAggregate<int?, BookEntity>
    {
        public GetCollectionLinkedEntityLoadOperation<PageEntity> GetPagesLoadOperation { get; }

        public IEnumerable<PageEntity> Pages => GetPagesLoadOperation.LinkedEntities;

        public BookPagesQueryAggregate() : this(null)
        {
        }

        public BookPagesQueryAggregate(RepositoryContext context) : base(context)
        {
            GetPagesLoadOperation = new GetCollectionLinkedEntityLoadOperation<PageEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((PageQueryRepository)repository).GetForBook(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PageQueryRepository)repository).GetForBookAsync(RootEntity.Id, user: null);

                    return entities.ToList();
                }
            };

            LoadOperations.Enqueue(
                GetPagesLoadOperation
            );
        }
    }
}
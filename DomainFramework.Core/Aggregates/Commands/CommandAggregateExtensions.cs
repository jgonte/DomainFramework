using System.Collections.Generic;

namespace DomainFramework.Core
{
    public static class CommandAggregateExtensions
    {
        public static void ReplaceValueObjectsOperation<TEntity, TValueObject, TRepositoryKey>(
            this CommandAggregate<TEntity> aggregate,
            IEnumerable<TValueObject> valueObjects
        )
            where TEntity : class, IEntity
            where TValueObject : IValueObject
        {
            var rootEntity = aggregate.RootEntity;

            var deleteCollection = new DeleteValueObjectCollectionCommandOperation<TEntity, TValueObject, TRepositoryKey>(rootEntity);

            aggregate.Enqueue(deleteCollection);

            foreach (var valueObject in valueObjects)
            {
                var addPhone = new AddLinkedValueObjectCommandOperation<TEntity, TValueObject, TRepositoryKey>(
                    rootEntity,
                    () => valueObject
                );

                aggregate.Enqueue(addPhone);
            }
        }
    }
}

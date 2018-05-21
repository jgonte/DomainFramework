using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Core
{
    /// <summary>
    /// Coordinates transactions through more than one entity
    /// </summary>
    public class CommandAggregate<TEntity> : ICommandAggregate<TEntity>
        where TEntity : IEntity
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        public List<IInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        public List<ISingleEntityLink> SingleEntityLinks { get; set; }

        public List<ICollectionEntityLink> CollectionEntityLinks { get; set; }

        protected virtual bool RequiresUnitOfWork =>
            (InheritanceEntityLinks != null && InheritanceEntityLinks.Any()) ||
            (SingleEntityLinks != null && SingleEntityLinks.Any()) ||
            (CollectionEntityLinks != null && CollectionEntityLinks.Any());

        public CommandAggregate(RepositoryContext context, TEntity entity)
        {
            RepositoryContext = context;

            RootEntity = entity;
        }

        public virtual void Save(IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            if (InheritanceEntityLinks != null)
            {
                foreach (var link in InheritanceEntityLinks)
                {
                    var repository = RepositoryContext.GetCommandRepository(link.LinkedEntityType);

                    var linkedEntity = link.GetLinkedEntity();

                    link.SetForeignKey(RootEntity, linkedEntity);

                    repository.Save(linkedEntity, unitOfWork);
                }
            }

            var rootRepository = RepositoryContext.GetCommandRepository(typeof(TEntity));

            rootRepository.Save(RootEntity, unitOfWork);

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    var repository = RepositoryContext.GetCommandRepository(link.LinkedEntityType);

                    var linkedEntity = link.GetLinkedEntity();

                    link.SetForeignKey(RootEntity, linkedEntity);

                    repository.Save(linkedEntity, unitOfWork);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    var repository = RepositoryContext.GetCommandRepository(link.LinkedEntityType);

                    foreach (var linkedEntity in link.GetLinkedEntities())
                    {
                        link.SetForeignKey(RootEntity, linkedEntity);

                        repository.Save(linkedEntity, unitOfWork);
                    }
                }
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual void Delete(IUnitOfWork unitOfWork = null)
        {
            var rootRepository = RepositoryContext.GetCommandRepository(typeof(TEntity));

            rootRepository.Delete(RootEntity);

            // Assume delete cascade for the linked entities
        }
    }
}

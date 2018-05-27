using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public List<ICommandInheritanceEntityLink<TEntity>> InheritanceEntityLinks { get; set; }

        public List<ICommandSingleEntityLink<TEntity>> SingleEntityLinks { get; set; }

        public List<ICommandCollectionEntityLink<TEntity>> CollectionEntityLinks { get; set; }

        protected virtual bool RequiresUnitOfWork =>
            (InheritanceEntityLinks != null && InheritanceEntityLinks.Any()) ||
            (SingleEntityLinks != null && SingleEntityLinks.Any() && SingleEntityLinks.Select(l => l.GetLinkedEntity()).Any(e => e != null)) ||
            (CollectionEntityLinks != null && CollectionEntityLinks.Any() && CollectionEntityLinks.SelectMany(l => l.GetLinkedEntities()).Any());

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

                    repository.TransferEntities = () => new IEntity[] { RootEntity };

                    repository.Save(linkedEntity, unitOfWork);
                }
            }

            var rootRepository = RepositoryContext.GetCommandRepository(typeof(TEntity));

            rootRepository.Save(RootEntity, unitOfWork);

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    link.Save(RepositoryContext, unitOfWork, RootEntity);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    link.Save(RepositoryContext, unitOfWork, RootEntity);
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

        public virtual async Task SaveAsync(IUnitOfWork unitOfWork = null)
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

                    repository.TransferEntities = () => new IEntity[] { RootEntity };

                    await repository.SaveAsync(linkedEntity, unitOfWork);
                }
            }

            var rootRepository = RepositoryContext.GetCommandRepository(typeof(TEntity));

            rootRepository.Save(RootEntity, unitOfWork);

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    await link.SaveAsync(RepositoryContext, unitOfWork, RootEntity);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    await link.SaveAsync(RepositoryContext, unitOfWork, RootEntity);
                }
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }
    }
}

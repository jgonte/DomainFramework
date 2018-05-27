using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Threading.Tasks;
=======
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3

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

<<<<<<< HEAD
        public List<ICommandInheritanceEntityLink<TEntity>> InheritanceEntityLinks { get; set; }

        public List<ICommandSingleEntityLink<TEntity>> SingleEntityLinks { get; set; }

        public List<ICommandCollectionEntityLink<TEntity>> CollectionEntityLinks { get; set; }

        protected virtual bool RequiresUnitOfWork =>
            (InheritanceEntityLinks != null && InheritanceEntityLinks.Any()) ||
            (SingleEntityLinks != null && SingleEntityLinks.Any() && SingleEntityLinks.Select(l => l.GetLinkedEntity()).Any(e => e != null)) ||
            (CollectionEntityLinks != null && CollectionEntityLinks.Any() && CollectionEntityLinks.SelectMany(l => l.GetLinkedEntities()).Any());
=======
        public List<IInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        public List<ISingleEntityLink> SingleEntityLinks { get; set; }

        public List<ICollectionEntityLink> CollectionEntityLinks { get; set; }

        protected virtual bool RequiresUnitOfWork =>
            (InheritanceEntityLinks != null && InheritanceEntityLinks.Any()) ||
            (SingleEntityLinks != null && SingleEntityLinks.Any()) ||
            (CollectionEntityLinks != null && CollectionEntityLinks.Any());
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3

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

<<<<<<< HEAD
                    repository.TransferEntities = () => new IEntity[] { RootEntity };
=======
                    link.SetForeignKey(RootEntity, linkedEntity);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3

                    repository.Save(linkedEntity, unitOfWork);
                }
            }

            var rootRepository = RepositoryContext.GetCommandRepository(typeof(TEntity));

            rootRepository.Save(RootEntity, unitOfWork);

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
<<<<<<< HEAD
                    link.Save(RepositoryContext, unitOfWork, RootEntity);
=======
                    var repository = RepositoryContext.GetCommandRepository(link.LinkedEntityType);

                    var linkedEntity = link.GetLinkedEntity();

                    link.SetForeignKey(RootEntity, linkedEntity);

                    repository.Save(linkedEntity, unitOfWork);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
<<<<<<< HEAD
                    link.Save(RepositoryContext, unitOfWork, RootEntity);
=======
                    var repository = RepositoryContext.GetCommandRepository(link.LinkedEntityType);

                    foreach (var linkedEntity in link.GetLinkedEntities())
                    {
                        link.SetForeignKey(RootEntity, linkedEntity);

                        repository.Save(linkedEntity, unitOfWork);
                    }
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
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
<<<<<<< HEAD

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
=======
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}

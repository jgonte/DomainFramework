using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    [Obsolete]
    public class SaveEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        private EntityDependency[] _dependencies;

        private string _selector;

        public SaveEntityCommandOperation(TEntity entity, EntityDependency[] dependencies = null, string selector = null) : base(entity)
        {
            _dependencies = dependencies;

            _selector = selector;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            repository.Save(Entity, user, unitOfWork, _selector);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            await repository.SaveAsync(Entity, user, unitOfWork, _selector);
        }
    }
}

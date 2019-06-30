using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SaveEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        private IEntity[] _dependencies;

        public SaveEntityCommandOperation(TEntity entity, params IEntity[] dependencies) : base(entity)
        {
            _dependencies = dependencies;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            repository.Save(Entity, user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            await repository.SaveAsync(Entity, user, unitOfWork);
        }
    }
}

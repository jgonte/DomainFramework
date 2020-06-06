using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class UpdateEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        private EntityDependency[] _dependencies;

        private string _selector;

        public UpdateEntityCommandOperation(TEntity entity, EntityDependency[] dependencies = null, string selector = null) : base(entity)
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

            repository.Update(Entity, user, unitOfWork, _selector);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            await repository.UpdateAsync(Entity, user, unitOfWork, _selector);
        }
    }
}

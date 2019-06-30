using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class InsertEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        private IEntity[] _dependencies;

        public InsertEntityCommandOperation(TEntity entity, params IEntity[] dependencies) : base(entity)
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

            repository.Insert(Entity, user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            if (_dependencies?.Any() == true)
            {
                repository.Dependencies = () => _dependencies;
            }

            await repository.InsertAsync(Entity, user, unitOfWork);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class InsertEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        private EntityDependency[] _dependencies;

        private string _selector;

        public InsertEntityCommandOperation(TEntity entity, EntityDependency[] dependencies = null, string selector = null) : base(entity)
        {
            _dependencies = dependencies;

            _selector = selector;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            AddDependenciesToRepository(repositoryContext, repository);

            repository.Insert(Entity, user, unitOfWork, _selector);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            AddDependenciesToRepository(repositoryContext, repository);

            await repository.InsertAsync(Entity, user, unitOfWork, _selector);
        }

        private void AddDependenciesToRepository(IRepositoryContext repositoryContext, IEntityCommandRepository repository)
        {
            var dependencies = (_dependencies ?? Enumerable.Empty<EntityDependency>())
                .Union(
                    repositoryContext.Dependencies
                );

            if (dependencies.Any())
            {
                repository.Dependencies = () => dependencies;
            }
        }
    }
}

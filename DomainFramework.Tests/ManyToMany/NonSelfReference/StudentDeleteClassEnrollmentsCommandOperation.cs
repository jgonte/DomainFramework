using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    internal class StudentDeleteClassEnrollmentsCommandOperation : CommandOperation<ClassEnrollmentEntity>
    {
        public StudentDeleteClassEnrollmentsCommandOperation(ClassEnrollmentEntity entity) : base(entity)
        {
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(Entity.GetType());

            repository.DeleteCollection(Entity, user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(Entity.GetType());

            await repository.DeleteCollectionAsync(Entity, user, unitOfWork);
        }
    }
}
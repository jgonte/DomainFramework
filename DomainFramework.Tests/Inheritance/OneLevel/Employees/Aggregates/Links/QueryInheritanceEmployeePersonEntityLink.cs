using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class QueryInheritanceEmployeePersonEntityLink : QueryInheritanceEntityLink<int?, PersonEntity>
    {
        public override void PopulateEntity(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (PersonQueryRepository)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            LinkedEntity = (repository).GetById(derivedEntityId);
        }

        public override async Task PopulateEntityAsync(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (PersonQueryRepository)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            LinkedEntity = await (repository).GetByIdAsync(derivedEntityId);
        }
    }
}
using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class QueryInheritanceEmployeePersonEntityLink : QueryInheritanceEntityLink<int?, PersonEntity>
    {
        public override void PopulateEntity(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (PersonQueryRepository)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            BaseEntity = (repository).GetById(derivedEntityId);
        }

        public override async Task PopulateEntityAsync(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (PersonQueryRepository)repositoryContext.GetQueryRepository(typeof(PersonEntity));

            BaseEntity = await (repository).GetByIdAsync(derivedEntityId);
        }
    }
}
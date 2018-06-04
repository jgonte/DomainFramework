using DomainFramework.Core;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class QueryCollectionStudentEntityLink : QueryCollectionEntityLink<ClassEntity, StudentEntity>
    {
        public override void PopulateEntities(IRepositoryContext repositoryContext, ClassEntity entity)
        {
            var repository = (StudentQueryRepository)repositoryContext.GetQueryRepository(typeof(StudentEntity));

            LinkedEntities = repository.GetForClass(entity.Id).ToList();
        }

        public override Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, ClassEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
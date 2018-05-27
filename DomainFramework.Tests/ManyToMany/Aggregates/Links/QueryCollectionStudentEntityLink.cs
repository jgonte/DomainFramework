using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests
{
    class QueryCollectionStudentEntityLink : QueryCollectionEntityLink<ClassEntity, StudentEntity>
    {
        public override void PopulateEntities(IQueryRepository repository, ClassEntity entity)
        {
            LinkedEntities = ((StudentQueryRepository)repository).GetForClass(entity.Id).ToList();
        }
    }
}
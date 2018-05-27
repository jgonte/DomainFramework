using DataAccess;
using System;

namespace DomainFramework.Tests
{
    class ClassQueryRepository : Core.QueryRepository<ClassEntity, Guid?>
    {
        public override ClassEntity GetById(Guid? id)
        {
            var result = Query<ClassEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Get")
                .Parameters(
                    p => p.Name("classId").Value(id.Value)
                )
                .MapProperties(
                    m => m.Map(p => p.Id),//.Index(0),
                    m => m.Map(p => p.Name)//.Index(1)
                )
                .Execute();

            return result.Data;
        }
    }
}

using DataAccess;
using System;
using System.Threading.Tasks;
using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ClassQueryRepository : Core.QueryRepository<ClassEntity, Guid?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

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

        public override async Task<ClassEntity> GetByIdAsync(Guid? id)
        {
            var result = await Query<ClassEntity>
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
                .ExecuteAsync();

            return result.Data;
        }
    }
}

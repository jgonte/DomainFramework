using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class StudentQueryRepository : Core.QueryRepository<StudentEntity, Guid?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override StudentEntity GetById(Guid? id)
        {
            var result = Query<StudentEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Student_Get")
                .Parameters(
                    p => p.Name("studentId").Value(id.Value)
                )
                .Execute();

            result.Data.Id = id.Value;

            return result.Data;
        }

        public override async Task<StudentEntity> GetByIdAsync(Guid? id)
        {
            var result = await Query<StudentEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Student_Get")
                .Parameters(
                    p => p.Name("studentId").Value(id.Value)
                )
                .ExecuteAsync();

            result.Data.Id = id.Value;

            return result.Data;
        }

        public IEnumerable<StudentEntity> GetForClass(Guid? id)
        {
            var result = Query<StudentEntity>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_GetStudents")
                .Parameters(
                    p => p.Name("classId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }
    }
}

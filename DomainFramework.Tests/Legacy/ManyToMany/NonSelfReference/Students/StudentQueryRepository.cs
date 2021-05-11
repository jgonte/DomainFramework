using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class StudentQueryRepository : Core.EntityQueryRepository<StudentEntity, Guid?>
    {
        public override (int, IEnumerable<StudentEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override Task<(int, IEnumerable<StudentEntity>)> GetAsync(CollectionQueryParameters parameters)
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

        public async Task<IEnumerable<StudentEntity>> GetForClassAsync(Guid? id)
        {
            var result = await Query<StudentEntity>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_GetStudents")
                .Parameters(
                    p => p.Name("classId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}

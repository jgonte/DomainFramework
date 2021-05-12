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

            result.Record.Id = id.Value;

            return result.Record;
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

            result.Record.Id = id.Value;

            return result.Record;
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

            return result.Records;
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

            return result.Records;
        }
    }
}

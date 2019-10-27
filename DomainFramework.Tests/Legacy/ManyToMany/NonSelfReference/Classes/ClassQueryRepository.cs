﻿using DataAccess;
using System;
using System.Threading.Tasks;
using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ClassQueryRepository : Core.EntityQueryRepository<ClassEntity, Guid?>
    {
        public override (int, IEnumerable<ClassEntity>) Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        public override Task<(int, IEnumerable<ClassEntity>)> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        public override ClassEntity GetById(Guid? id, IAuthenticatedUser user)
        {
            var result = Query<ClassEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Get")
                .Parameters(
                    p => p.Name("classId").Value(id.Value)
                )
                .MapProperties(
                    m => m.Map<ClassEntity>(p => p.Id),//.Index(0),
                    m => m.Map<ClassEntity>(p => p.Name)//.Index(1)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<ClassEntity> GetByIdAsync(Guid? id, IAuthenticatedUser user)
        {
            var result = await Query<ClassEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Get")
                .Parameters(
                    p => p.Name("classId").Value(id.Value)
                )
                .MapProperties(
                    m => m.Map<ClassEntity>(p => p.Id),//.Index(0),
                    m => m.Map<ClassEntity>(p => p.Name)//.Index(1)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}

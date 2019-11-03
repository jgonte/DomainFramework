﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeeQueryRepository : EntityQueryRepository<EmployeeEntity, int?>
    {
        public override (int, IEnumerable<EmployeeEntity>) Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<EmployeeEntity>)> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override EmployeeEntity GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<EmployeeEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Get")
                .Parameters(
                    p => p.Name("employeeId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<EmployeeEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<EmployeeEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Get")
                .Parameters(
                    p => p.Name("employeeId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}
﻿using DomainFramework.Core;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests
{
    class ManagerEmployeesCommandAggregate : CommandAggregate<PersonEntity3>
    {
        private CollectionEntityLinkTransactedOperation<PersonEntity3, PersonEntity3> _employeesLink { get; set; }

        public IEnumerable<PersonEntity3> Employees => _employeesLink.LinkedEntities;

        public ManagerEmployeesCommandAggregate(RepositoryContext context, PersonEntity3 entity) : base(context, entity)
        {
            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity3>(entity, CommandOperationTypes.Save)
            );

            _employeesLink = new CollectionEntityLinkTransactedOperation<PersonEntity3, PersonEntity3>(entity);

            TransactedOperations.Enqueue(_employeesLink);
        }

        public void AddEmployee(PersonEntity3 employee)
        {
            _employeesLink.AddLinkedEntity(employee);
        }
    }
}
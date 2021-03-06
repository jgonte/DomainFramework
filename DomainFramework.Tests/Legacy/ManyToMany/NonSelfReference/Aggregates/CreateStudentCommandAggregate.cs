﻿using DomainFramework.Core;

namespace DomainFramework.Tests
{
    internal class CreateStudentCommandAggregate : CommandAggregate<StudentEntity>
    {
        public CreateStudentCommandAggregate(RepositoryContext context, StudentInputDto studentInputDto) : base(context)
        {
            RootEntity = new StudentEntity
            {
                FirstName = studentInputDto.FirstName
            };

            Enqueue(
                new InsertEntityCommandOperation<StudentEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}
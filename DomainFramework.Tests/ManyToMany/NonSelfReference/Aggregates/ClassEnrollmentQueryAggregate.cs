using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ClassEnrollmentQueryAggregate : QueryAggregate<Guid?, ClassEntity>
    {
        public IEnumerable<StudentEntity> Students => StudentLinks.LinkedEntities;

        public ClassEnrollmentQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<IQueryCollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(StudentLinks);
        }

        public QueryCollectionStudentEntityLink StudentLinks { get; set; } = new QueryCollectionStudentEntityLink();
    }
}
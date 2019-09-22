using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class StudentOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

    }
}
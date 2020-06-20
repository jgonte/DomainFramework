using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassOutputDto : IOutputDataTransferObject
    {
        public int ClassId { get; set; }

        public string Name { get; set; }

        public IEnumerable<StudentOutputDto> Students { get; set; }

    }
}
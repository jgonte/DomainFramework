using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IMultiValue
    {
        List<object> FieldValues { get; set; }
    }
}
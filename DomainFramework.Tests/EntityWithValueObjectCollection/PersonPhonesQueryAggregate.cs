using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonPhonesQueryAggregate : QueryAggregate<int?, PersonEntity4>
    {
        public QueryCollectionPhoneValueObjectLink PhonesLink { get; set; } = new QueryCollectionPhoneValueObjectLink();

        public List<Phone> Phones => PhonesLink.LinkedValueObjects;

        public PersonPhonesQueryAggregate() : this(null)
        {
        }

        public PersonPhonesQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            
        }
    }
}
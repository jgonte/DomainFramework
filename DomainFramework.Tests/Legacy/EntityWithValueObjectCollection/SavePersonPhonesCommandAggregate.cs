using DomainFramework.Core;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class SavePersonPhonesCommandAggregate : CommandAggregate<PersonEntity4>
    {
        public SavePersonPhonesCommandAggregate(DataAccess.RepositoryContext context, PersonEntity4 person, List<Phone> phones) : base(context)
        {
            RootEntity = person;

            Enqueue(
                new SaveEntityCommandOperation<PersonEntity4>(RootEntity)
            );

            this.ReplaceValueObjectsOperation<PersonEntity4, Phone, PhoneCommandRepository.RepositoryKey>(phones);
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
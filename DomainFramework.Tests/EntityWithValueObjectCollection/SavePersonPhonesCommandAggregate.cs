using DomainFramework.Core;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class SavePersonPhonesCommandAggregate : CommandAggregate<PersonEntity4>
    {
        private CollectionValueObjectLinkTransactedOperation<PersonEntity4, Phone, PhoneCommandRepository.RepositoryKey> _phonesLink;

        public SavePersonPhonesCommandAggregate(DataAccess.RepositoryContext context, string firstName, List<Phone> phones) : base(context, null)
        {
            RootEntity = new PersonEntity4
            {
                FirstName = firstName,
                Phones = phones
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity4>(RootEntity, CommandOperationTypes.Save)
            );

            _phonesLink = new CollectionValueObjectLinkTransactedOperation<PersonEntity4, Phone, PhoneCommandRepository.RepositoryKey>(RootEntity)
            {
                GetLinkedValueObjects = entity => entity.Phones
            };

            TransactedOperations.Enqueue(
                _phonesLink
            );
        }
    }
}
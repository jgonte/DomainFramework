using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class PersonPhonesCommandAggregate : CommandAggregate<PersonEntity4>
    {
        private CollectionValueObjectLinkTransactedOperation<PersonEntity4, Phone> _phonesLink;

        public IEnumerable<Phone> Pages => _phonesLink.LinkedValueObjects;

        public PersonPhonesCommandAggregate(DataAccess.RepositoryContext context, string firstName, List<Phone> phones) : base(context, null)
        {
            RootEntity = new PersonEntity4
            {
                FirstName = firstName,
                Phones = phones
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<PersonEntity4>(RootEntity)
            );

            _phonesLink = new CollectionValueObjectLinkTransactedOperation<PersonEntity4, Phone>(RootEntity);

            foreach (var phone in phones)
            {
                _phonesLink.AddLinkedValueObject(phone);
            }

            TransactedSaveOperations.Enqueue(
                _phonesLink
            );

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<PersonEntity4>(RootEntity)
            );
        }
    }
}
using DomainFramework.Core;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    internal class AddPhoneCommandAggregate : CommandAggregate<PersonEntity4>
    {
        public AddPhoneCommandAggregate(DataAccess.RepositoryContext context, PersonEntity4 personEntity, Phone phone) : base(context)
        {
            RootEntity = personEntity;

            var addPhone = new AddLinkedValueObjectCommandOperation<PersonEntity4, Phone, PhoneCommandRepository.RepositoryKey>(
                RootEntity,
                () => phone
            );

            Enqueue(addPhone);
        }
    }
}
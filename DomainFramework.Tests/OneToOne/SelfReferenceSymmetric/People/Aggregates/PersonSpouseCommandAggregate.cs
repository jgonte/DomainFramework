using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    class PersonSpouseCommandAggregate : CommandAggregate<PersonEntity2>
    {
        public CommandSingleSymmetricEntityLink<PersonEntity2, PersonEntity2> SpouseLink { get; set; } = new CommandSingleSymmetricEntityLink<PersonEntity2, PersonEntity2>();

        public PersonEntity2 Spouse => SpouseLink.LinkedEntity;

        public PersonSpouseCommandAggregate(RepositoryContext context, PersonEntity2 entity) : base(context, entity)
        {
            // Create the links to the single of entity links
            SingleEntityLinks = new List<ICommandSingleEntityLink<PersonEntity2>>();

            // Register the link to the single capital city
            SingleEntityLinks.Add(SpouseLink);
        }

        public void SetSpouse(PersonEntity2 person)
        {
            SpouseLink.SetEntity(person);
        }
    }
}
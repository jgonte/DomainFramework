using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.OneToOne
{
    class CountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CountryCapitalCityCommandAggregate(RepositoryContext context, CountryEntity entity) : base(context, entity)
        {
            // Create the links to the single of entity links
            SingleEntityLinks = new List<ICommandSingleEntityLink<CountryEntity>>();

            // Register the link to the single capital city
            SingleEntityLinks.Add(CapitalCityLink);
        }

        public CommandSingleEntityLink<CountryEntity, CapitalCityEntity> CapitalCityLink { get; set; } = new CommandSingleEntityLink<CountryEntity, CapitalCityEntity>();

        public CapitalCityEntity CapitalCity => CapitalCityLink.LinkedEntity;

        public void SetCapitalCity(CapitalCityEntity pageEntity)
        {
            CapitalCityLink.SetEntity(pageEntity);
        }

        public void Delete()
        {
            var repository = RepositoryContext.GetCommandRepository(RootEntity.GetType());

            repository.Delete(RootEntity);
        }
    }
}
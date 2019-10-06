using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CreateCountryCommandAggregate : CommandAggregate<Country>
    {
        public CreateCountryCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public CreateCountryCommandAggregate(CreateCountryInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(country, dependencies);
        }

        public override void Initialize(IInputDataTransferObject country, EntityDependency[] dependencies)
        {
            Initialize((CreateCountryInputDto)country, dependencies);
        }

        private void Initialize(CreateCountryInputDto country, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Country>(() => new CountryCommandRepository());

            RegisterCommandRepositoryFactory<CapitalCity>(() => new CapitalCityCommandRepository());

            RootEntity = new Country
            {
                Id = country.Id,
                Name = country.Name,
                IsActive = country.IsActive
            };

            Enqueue(new InsertEntityCommandOperation<Country>(RootEntity));

            if (country.CapitalCity != null)
            {
                var capitalCity = country.CapitalCity;

                var entityForCapitalCity = new CapitalCity
                {
                    Name = capitalCity.Name
                };

                Enqueue(new AddLinkedEntityCommandOperation<Country, CapitalCity>(RootEntity, () => entityForCapitalCity, "CapitalCity"));
            }
        }

    }
}
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryAddCapitalCityCommandAggregate : CommandAggregate<Country>
    {
        public CountryAddCapitalCityCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public CountryAddCapitalCityCommandAggregate(CountryAddCapitalCityInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(country, dependencies);
        }

        public override void Initialize(IInputDataTransferObject country, EntityDependency[] dependencies)
        {
            Initialize((CountryAddCapitalCityInputDto)country, dependencies);
        }

        private void Initialize(CountryAddCapitalCityInputDto country, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<CapitalCity>(() => new CapitalCityCommandRepository());

            RootEntity = new Country
            {
                Id = country.Id
            };

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
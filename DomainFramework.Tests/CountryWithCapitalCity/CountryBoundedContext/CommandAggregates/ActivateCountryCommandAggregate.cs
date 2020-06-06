using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class ActivateCountryCommandAggregate : CommandAggregate<Country>
    {
        public ActivateCountryCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public ActivateCountryCommandAggregate(IsActiveCountryInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(country, dependencies);
        }

        public override void Initialize(IInputDataTransferObject country, EntityDependency[] dependencies)
        {
            Initialize((IsActiveCountryInputDto)country, dependencies);
        }

        private void Initialize(IsActiveCountryInputDto country, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Country>(() => new CountryCommandRepository());

            RootEntity = new Country
            {
                Id = country.CountryCode
            };

            Enqueue(new UpdateEntityCommandOperation<Country>(RootEntity, dependencies, "Activate"));
        }

    }
}
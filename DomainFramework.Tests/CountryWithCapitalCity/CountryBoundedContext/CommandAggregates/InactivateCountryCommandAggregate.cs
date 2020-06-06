using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class InactivateCountryCommandAggregate : CommandAggregate<Country>
    {
        public InactivateCountryCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public InactivateCountryCommandAggregate(IsActiveCountryInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
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

            Enqueue(new UpdateEntityCommandOperation<Country>(RootEntity, dependencies, "Inactivate"));
        }

    }
}
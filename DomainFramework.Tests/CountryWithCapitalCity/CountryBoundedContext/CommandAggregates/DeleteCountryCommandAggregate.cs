using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class DeleteCountryCommandAggregate : CommandAggregate<Country>
    {
        public DeleteCountryCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public DeleteCountryCommandAggregate(DeleteCountryInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(country, dependencies);
        }

        public override void Initialize(IInputDataTransferObject country, EntityDependency[] dependencies)
        {
            Initialize((DeleteCountryInputDto)country, dependencies);
        }

        private void Initialize(DeleteCountryInputDto country, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Country>(() => new CountryCommandRepository());

            RootEntity = new Country
            {
                Id = country.Id
            };

            Enqueue(new DeleteEntityCommandOperation<Country>(RootEntity));
        }

    }
}
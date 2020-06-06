using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CreateCapitalCityCommandAggregate : CommandAggregate<CapitalCity>
    {
        public CreateCapitalCityCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public CreateCapitalCityCommandAggregate(CreateCapitalCityInputDto capitalCity, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(capitalCity, dependencies);
        }

        public override void Initialize(IInputDataTransferObject capitalCity, EntityDependency[] dependencies)
        {
            Initialize((CreateCapitalCityInputDto)capitalCity, dependencies);
        }

        private void Initialize(CreateCapitalCityInputDto capitalCity, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<CapitalCity>(() => new CapitalCityCommandRepository());

            var countryDependency = (Country)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new CapitalCity
            {
                Name = capitalCity.Name,
                CountryCode = (countryDependency != null) ? countryDependency.Id : capitalCity.CountryCode
            };

            Enqueue(new InsertEntityCommandOperation<CapitalCity>(RootEntity, dependencies));
        }

    }
}
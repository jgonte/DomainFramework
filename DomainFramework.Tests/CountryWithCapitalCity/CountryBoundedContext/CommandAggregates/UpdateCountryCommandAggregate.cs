using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class UpdateCountryCommandAggregate : CommandAggregate<Country>
    {
        public UpdateCountryCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
        }

        public UpdateCountryCommandAggregate(UpdateCountryInputDto country, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            Initialize(country, dependencies);
        }

        public override void Initialize(IInputDataTransferObject country, EntityDependency[] dependencies)
        {
            Initialize((UpdateCountryInputDto)country, dependencies);
        }

        private void Initialize(UpdateCountryInputDto country, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Country>(() => new CountryCommandRepository());

            RootEntity = new Country
            {
                Id = country.CountryCode,
                Name = country.Name,
                IsActive = country.IsActive
            };

            Enqueue(new UpdateEntityCommandOperation<Country>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Country>(RootEntity, "UnlinkCapitalCityFromCountry"));

            if (country.CapitalCity != null)
            {
                ILinkedAggregateCommandOperation operation;

                var capitalCity = country.CapitalCity;

                if (capitalCity is CreateCapitalCityInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Country, CreateCapitalCityCommandAggregate, CreateCapitalCityInputDto>(
                        RootEntity,
                        (CreateCapitalCityInputDto)capitalCity,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "CapitalCity"
                            }
                        }
                    );

                    Enqueue(operation);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

    }
}
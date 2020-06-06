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
            RegisterCommandRepositoryFactory<Country>(() => new CountryCommandRepository());

            RootEntity = new Country
            {
                Id = country.CountryCode
            };

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
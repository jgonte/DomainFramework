using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    [TestClass]
    public class CountryWithCapitalCityTests
    {
        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // Create the test database
            var script = File.ReadAllText(
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\CountryWithCapitalCity\Sql\CreateTestDatabase.sql"
            );

            ScriptRunner.Run(ConnectionManager.GetConnection("Master").ConnectionString, script);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        //[TestMethod]
        //public void Save_Country_Only_Tests()
        //{
        //    // Insert
        //    var createCommandAggregate = new CreateCountryCommandAggregate(new CreateCountryInputDto
        //    {
        //        Id = "us",
        //        Name = "United States"
        //    });

        //    createCommandAggregate.Save();

        //    var countryId = createCommandAggregate.RootEntity.Id;

        //    // Read
        //    var queryAggregate = new CountryQueryAggregate();

        //    var countryDto = queryAggregate.Get(countryId);

        //    Assert.AreEqual(countryId, countryDto.Id);

        //    Assert.AreEqual("United States", countryDto.Name);

        //    Assert.IsNull(countryDto.CapitalCity);

        //    // Update
        //    var updateCommandAggregate = new UpdateCountryCommandAggregate(new UpdateCountryInputDto
        //    {
        //        Id = countryId,
        //        Name = "United States of America"
        //    });

        //    updateCommandAggregate.Save();

        //    // Read
        //    queryAggregate = new CountryQueryAggregate();

        //    countryDto = queryAggregate.Get(countryId);

        //    Assert.AreEqual(countryId, countryDto.Id);

        //    Assert.AreEqual("United States of America", countryDto.Name);

        //    Assert.IsNull(countryDto.CapitalCity);

        //    // Delete
        //    //var deleteAggregate = new DeleteCountryCommandAggregate(new DeleteCountryInputDto
        //    //{
        //    //    Id = countryId.Value
        //    //});

        //    //deleteAggregate.Save();

        //    //countryDto = queryAggregate.Get(countryId);

        //    //Assert.IsNull(countryDto);
        //}

        [TestMethod]
        public void Save_Country_With_Capital_City_Tests()
        {
            // Insert
            var createCommandAggregate = new CreateCountryCommandAggregate(new CreateCountryInputDto
            {
                CountryCode = "jp",
                Name = "Japon",
                CapitalCity = new CreateCapitalCityInputDto
                {
                    Name = "Tokio"
                }
            });

            createCommandAggregate.Save();

            var countryCode = createCommandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetCountryByIdQueryAggregate();

            var countryDto = queryAggregate.Get(countryCode);

            Assert.AreEqual(countryCode, countryDto.Id);

            Assert.AreEqual("Japon", countryDto.Name);

            var capitalCityDto = countryDto.CapitalCity;

            Assert.AreEqual(countryCode, capitalCityDto.CountryCode);

            Assert.AreEqual("Tokio", capitalCityDto.Name);

            // Update
            var updateCommandAggregate = new UpdateCountryCommandAggregate(new UpdateCountryInputDto
            {
                CountryCode = countryCode,
                Name = "Japan",
                CapitalCity = new CreateCapitalCityInputDto
                {
                    Name = "Tokyo"
                }
            });

            updateCommandAggregate.Save();

            // Read
            queryAggregate = new GetCountryByIdQueryAggregate();

            countryDto = queryAggregate.Get(countryCode);

            Assert.AreEqual(countryCode, countryDto.Id);

            Assert.AreEqual("Japan", countryDto.Name);

            capitalCityDto = countryDto.CapitalCity;

            Assert.AreEqual(countryCode, capitalCityDto.CountryCode);

            Assert.AreEqual("Tokyo", capitalCityDto.Name);

            // Add another country to test getting a collection of countries
            createCommandAggregate = new CreateCountryCommandAggregate(new CreateCountryInputDto
            {
                CountryCode = "no",
                Name = "Norway",
                CapitalCity = new CreateCapitalCityInputDto
                {
                    Name = "Oslo"
                }
            });

            createCommandAggregate.Save();

            var getCountriesQueryAggregate = new GetCountriesQueryAggregate();

            var countryDtos = getCountriesQueryAggregate.Get(queryParameters: null, user: null);

            Assert.AreEqual(2, countryDtos.Item1);

            countryDto = countryDtos.Item2.ElementAt(0);

            Assert.AreEqual("jp", countryDto.Id);

            Assert.AreEqual("Japan", countryDto.Name);

            capitalCityDto = countryDto.CapitalCity;

            Assert.AreEqual("jp", capitalCityDto.CountryCode);

            Assert.AreEqual("Tokyo", capitalCityDto.Name);

            countryDto = countryDtos.Item2.ElementAt(1);

            Assert.AreEqual("no", countryDto.Id);

            Assert.AreEqual("Norway", countryDto.Name);

            capitalCityDto = countryDto.CapitalCity;

            Assert.AreEqual("no", capitalCityDto.CountryCode);

            Assert.AreEqual("Oslo", capitalCityDto.Name);

            // Inactivate a country
            var inactivateCountryAggregate = new InactivateCountryCommandAggregate(new IsActiveCountryInputDto { CountryCode = "jp" });

            inactivateCountryAggregate.Save();

            // Read the inactive country
            queryAggregate = new GetCountryByIdQueryAggregate();

            countryDto = queryAggregate.Get("jp");

            Assert.AreEqual("Japan", countryDto.Name);

            Assert.IsFalse(countryDto.IsActive);

            // Activate a country
            var activateCountryAggregate = new ActivateCountryCommandAggregate(new IsActiveCountryInputDto { CountryCode = "jp" });

            activateCountryAggregate.Save();

            // Read the active country
            queryAggregate = new GetCountryByIdQueryAggregate();

            countryDto = queryAggregate.Get("jp");

            Assert.AreEqual("Japan", countryDto.Name);

            Assert.IsTrue(countryDto.IsActive);

            // Delete
            //var deleteAggregate = new DeleteCountryCommandAggregate(new DeleteCountryInputDto
            //{
            //    Id = countryId.Value
            //});

            //deleteAggregate.Save();

            //countryDto = queryAggregate.Get(countryId);

            //Assert.IsNull(countryDto);


        }

    }
}

using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    [TestClass]
    public class PersonWithPhonesTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneEntityWithValueObjectCollectionTest.ConnectionString";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            // Test script executor (create database)
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("Master"),
@"
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'DomainFrameworkOneEntityWithValueObjectCollectionTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneEntityWithValueObjectCollectionTest
END
GO

CREATE DATABASE DomainFrameworkOneEntityWithValueObjectCollectionTest
GO

USE DomainFrameworkOneEntityWithValueObjectCollectionTest
GO

CREATE TABLE DomainFrameworkOneEntityWithValueObjectCollectionTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkOneEntityWithValueObjectCollectionTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)
GO

CREATE TABLE DomainFrameworkOneEntityWithValueObjectCollectionTest..Phone(
    [PersonId] INT NOT NULL,
    [Number] VARCHAR(15) NOT NULL,
    [PhoneType] INT NOT NULL
)

ALTER TABLE DomainFrameworkOneEntityWithValueObjectCollectionTest..Phone
ADD CONSTRAINT Phone_PK PRIMARY KEY (PersonId, Number)

ALTER TABLE DomainFrameworkOneEntityWithValueObjectCollectionTest..Phone
ADD CONSTRAINT Phone_Person_FK FOREIGN KEY (PersonId) REFERENCES DomainFrameworkOneEntityWithValueObjectCollectionTest..Person(PersonId);

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_Person_Create]
    @firstName VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PersonId] INT NOT NULL
    );

    INSERT INTO Person
    (
        [FirstName]
    )
    OUTPUT
        INSERTED.[PersonId]
    INTO @outputData
    VALUES
    (
        @firstName
    );

    SELECT
        [PersonId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Delete_Phones_For_Person]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Phone]
    WHERE [PersonId] = @personId

END;
GO

CREATE PROCEDURE [p_Phone_Create]
    @personId INT,
    @number VARCHAR(15),
    @phoneType INT
AS
BEGIN
    
    INSERT INTO Phone
    (
        [PersonId],
        [Number],
        [PhoneType]
    )
    VALUES
    (
        @personId,
        @number,
        @phoneType
    );

END;
GO

CREATE PROCEDURE [p_Person_Get]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PersonId] AS Id,
        [FirstName]
    FROM [Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_GetPhones]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PersonId],
        [Number],
        [PhoneType]
    FROM [Phone]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_Update]
    @personId INT,
    @firstName VARCHAR(50)
AS
BEGIN

    UPDATE Person
    SET
        [FirstName] = @firstName
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_Delete]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Person]
    WHERE [PersonId] = @personId

END;
GO

",
                        "^GO");
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
        [TestMethod]
        public void Person_With_Phones_Test()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity4>(() => new PersonCommandRepository5());

            context.RegisterCommandRepositoryFactory<PhoneCommandRepository.RepositoryKey>(() => new PhoneCommandRepository());

            context.RegisterQueryRepository<PersonEntity4>(new PersonQueryRepository5());

            context.RegisterQueryRepository<Person_PhonesQueryRepository.RepositoryKey>(new Person_PhonesQueryRepository());

            // We need an aggregate since saving the person and her phones need to happen within a transaction
            var personCommandAggregate = new SavePersonPhonesCommandAggregate(context, 
                firstName: "Mary",
                phones: new List<Phone>
                {
                    new Phone(number: "786-111-2233"),
                    new Phone(number: "786-444-2233", type: Phone.Types.Home),
                    new Phone(number: "954-444-1111", type: Phone.Types.Work)
                });

            personCommandAggregate.Save();

            var personEntity = personCommandAggregate.RootEntity;

            var id = personEntity.Id;

            var personQueryAggregate = new PersonPhonesQueryAggregate(context);

            // Read
            personQueryAggregate.Get(id, user: null);

            personEntity = personQueryAggregate.RootEntity;

            Assert.AreEqual(id, personEntity.Id);

            Assert.AreEqual("Mary", personEntity.FirstName);

            Assert.AreEqual(3, personEntity.Phones.Count);

            var phone = personEntity.Phones.ElementAt(0);

            Assert.AreEqual(new Phone(number: "786-111-2233", type: Phone.Types.Cell), phone);

            phone = personEntity.Phones.ElementAt(1);

            Assert.AreEqual(new Phone(number: "786-444-2233", type: Phone.Types.Home), phone);

            phone = personEntity.Phones.ElementAt(2);

            Assert.AreEqual(new Phone(number: "954-444-1111", type: Phone.Types.Work), phone);

            // Update
            personEntity = personCommandAggregate.RootEntity;

            personEntity.FirstName = "Mariah";

            // After updating this should be the only phone in the list
            personEntity.Phones = new List<Phone>
            {
                new Phone(number: "786-111-4433")
            };

            personCommandAggregate.Save();

            // Read changes
            personQueryAggregate.Get(id, user: null);

            personEntity = personQueryAggregate.RootEntity;

            Assert.AreEqual(id, personEntity.Id);

            Assert.AreEqual("Mariah", personEntity.FirstName);

            Assert.AreEqual(1, personEntity.Phones.Count); // The three previous phones must be replaced by only one

            phone = personEntity.Phones.ElementAt(0);

            Assert.AreEqual(new Phone(number: "786-111-4433", type: Phone.Types.Cell), phone);
        }
    }
}

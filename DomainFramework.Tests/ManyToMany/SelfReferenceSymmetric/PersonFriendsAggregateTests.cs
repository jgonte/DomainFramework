using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.ManyToMany.SelfReferenceSymmetric
{
    [TestClass]
    public class PersonFriendsAggregateTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkManyToManySelfReferenceSymmetricTest.ConnectionString";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use PersonInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static async Task MyPersonInitialize(TestContext testContext)
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
    WHERE Name = N'DomainFrameworkManyToManySelfReferenceSymmetricTest'
)
BEGIN
    DROP DATABASE DomainFrameworkManyToManySelfReferenceSymmetricTest
END
GO

CREATE DATABASE DomainFrameworkManyToManySelfReferenceSymmetricTest
GO

USE DomainFrameworkManyToManySelfReferenceSymmetricTest
GO

CREATE TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50)
)

ALTER TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)
GO

CREATE TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Friendship(
    [PersonId] INT NOT NULL,
    [FriendId] INT NOT NULL,
    [AcceptedDateTime] DATETIME NOT NULL
)

ALTER TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Friendship
ADD CONSTRAINT Friendship_PK PRIMARY KEY (PersonId, FriendId)

ALTER TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Friendship
ADD CONSTRAINT Friendship_Person_FK FOREIGN KEY (PersonId) REFERENCES DomainFrameworkManyToManySelfReferenceSymmetricTest..Person(PersonId);

ALTER TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Friendship
ADD CONSTRAINT Friendship_Friend_FK FOREIGN KEY (FriendId) REFERENCES DomainFrameworkManyToManySelfReferenceSymmetricTest..Person(PersonId);

ALTER TABLE DomainFrameworkManyToManySelfReferenceSymmetricTest..Friendship
ADD CONSTRAINT PersonId_Cannot_Be_Same_As_FriendId_CHK CHECK (PersonId <> FriendId);
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

CREATE PROCEDURE [p_Friendship_Create]
    @personId INT,
    @friendId INT,
    @acceptedDateTime DATETIME
AS
BEGIN

    INSERT INTO Friendship
    (
        [PersonId],
        [FriendId],
        [AcceptedDateTime]
    )
    VALUES
    (
        @personId,
        @friendId,
        @acceptedDateTime
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
    WHERE [PersonId] = @personId

END;
GO

CREATE PROCEDURE [p_Person_GetFriends]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        f.[PersonId] AS Id,
        f.[FirstName]
    FROM [Person] f
    INNER JOIN [Friendship] fs
        ON f.[PersonId] = fs.[FriendId]
    WHERE fs.[PersonId] = @personId

END;
GO

CREATE PROCEDURE [p_Friendship_Remove]
    @personId INT,
    @friendId INT
AS
BEGIN

    DELETE FROM Friendship
    WHERE [PersonId] = personId
    AND [FriendId] = @friendId;

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
        public async Task Person_Aggregate_With_Friends_Async_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository3());

            context.RegisterCommandRepositoryFactory<FriendshipEntity>(() => new FriendshipCommandRepository());

            // Suppose the person and the student do not exist by the time we enroll it so we need to create the person and student records in the database

            var entity = new PersonEntity
            {
                FirstName = "Jorge"
            };

            var commandAggregate = new PersonFriendsCommandAggregate(context, entity);

            commandAggregate.AddFriend(new PersonEntity
            {
                FirstName = "Sarah"
            },
            new DateTime(2017, 3, 12, 9, 16, 37));

            commandAggregate.AddFriend(new PersonEntity
            {
                FirstName = "Yana"
            },
            new DateTime(2017, 4, 12, 10, 26, 47));

            commandAggregate.AddFriend(new PersonEntity
            {
                FirstName = "Mark"
            },
            new DateTime(2017, 5, 14, 11, 24, 57));

            await commandAggregate.SaveAsync();

            // Read

            context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository3());

            var id = entity.Id; // Keep the generated id

            entity = new PersonEntity(); // New entity

            var queryAggregate = new PersonFriendsQueryAggregate(context);

            await queryAggregate.LoadAsync(id);

            entity = queryAggregate.RootEntity;

            Assert.AreEqual(id, entity.Id);

            Assert.AreEqual("Jorge", entity.FirstName);

            Assert.AreEqual(3, queryAggregate.Friends.Count());

            var student = queryAggregate.Friends.ElementAt(0);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Sarah", student.FirstName);

            student = queryAggregate.Friends.ElementAt(1);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Yana", student.FirstName);

            student = queryAggregate.Friends.ElementAt(2);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Mark", student.FirstName);
        }
    }
}

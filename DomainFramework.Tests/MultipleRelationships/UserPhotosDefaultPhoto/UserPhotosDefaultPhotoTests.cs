using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class UserPhotosDefaultPhotoTests
    {
        static readonly string connectionName = "SqlServerTest.UserPhotosDefaultPhoto.ConnectionString";

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
    WHERE Name = N'UserPhotosDefaultPhoto'
)
BEGIN
    DROP DATABASE UserPhotosDefaultPhoto
END
GO

CREATE DATABASE UserPhotosDefaultPhoto
GO

USE UserPhotosDefaultPhoto
GO

CREATE TABLE UserPhotosDefaultPhoto..[User](
    [UserId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [DefaultPhotoId] INT
)

ALTER TABLE UserPhotosDefaultPhoto..[User]
ADD CONSTRAINT User_PK PRIMARY KEY (UserId)
GO

CREATE TABLE UserPhotosDefaultPhoto..Photo(
    [PhotoId] INT NOT NULL IDENTITY,
    [Description] VARCHAR(50) NOT NULL,
    [UserId] INT NOT NULL
)

ALTER TABLE UserPhotosDefaultPhoto..Photo
ADD CONSTRAINT Photo_PK PRIMARY KEY (PhotoId)
GO

ALTER TABLE UserPhotosDefaultPhoto..Photo
ADD CONSTRAINT Photo_User_FK FOREIGN KEY (UserId) REFERENCES UserPhotosDefaultPhoto..[User](UserId);
GO

ALTER TABLE UserPhotosDefaultPhoto..[User]
ADD CONSTRAINT User_DefaultPhoto_FK FOREIGN KEY (DefaultPhotoId) REFERENCES UserPhotosDefaultPhoto..Photo(PhotoId);
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
    @"
CREATE PROCEDURE [p_User_Create]
    @name VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [UserId] INT NOT NULL
    );

    INSERT INTO [User]
    (
        [Name]
    )
    OUTPUT
        INSERTED.[UserId]
    INTO @outputData
    VALUES
    (
        @name
    );

    SELECT
        [UserId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Photo_Create]
    @description VARCHAR(50),
    @userId INT
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PhotoId] INT NOT NULL
    );

    INSERT INTO Photo
    (
        [Description],
        [UserId]
    )
    OUTPUT
        INSERTED.[PhotoId]
    INTO @outputData
    VALUES
    (
        @description,
        @userId
    );

    SELECT
        [PhotoId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_User_Get]
    @userId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Name]
    FROM [User]
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [p_Photo_Get]
    @photoId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Description],
        [UserId]
    FROM [Photo]
    WHERE [PhotoId] = @photoId

END;
GO

CREATE PROCEDURE [p_User_GetPhoto]
    @userId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PhotoId],
        [Description],
        [UserId]
    FROM [Photo]
    WHERE [UserId] = @userId

END;
GO

CREATE PROCEDURE [p_User_Update]
    @userId INT,
    @name VARCHAR(50),
    @defaultPhotoId INT
AS
BEGIN

    UPDATE [User]
    SET
        [Name] = @name,
        [DefaultPhotoId] = @defaultPhotoId
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [p_Photo_Update]
    @photoId INT,
    @description VARCHAR(50),
    @userId INT
AS
BEGIN

    UPDATE Photo
    SET
        [Description] = @description,
        [UserId] = @userId
    WHERE [PhotoId] = @photoId;

END;
GO

CREATE PROCEDURE [p_User_Delete]
    @userId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [User]
        WHERE [UserId] = @userId

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
        public void User_With_Several_Photos_And_With_A_Default_Photo_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<UserEntity>(() => new UserCommandRepository());

            context.RegisterCommandRepositoryFactory<PhotoEntity>(() => new PhotoCommandRepository());

            // Configure the user
            var commandAggregate = new UserCommandAggregate(context, new UserInputDto
            {
                Name = "alanbrito",
                Photos = new PhotoInputDto[]
                {
                    new PhotoInputDto
                    {
                        Description = "Photo 1",
                        IsDefault = true
                    },
                    new PhotoInputDto
                    {
                        Description = "Photo 2"
                    },
                    new PhotoInputDto
                    {
                        Description = "Photo 3"
                    }
                }
            });
            
            commandAggregate.Save();
        }
    }
}

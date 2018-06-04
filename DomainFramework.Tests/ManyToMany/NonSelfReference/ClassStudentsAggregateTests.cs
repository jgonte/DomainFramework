using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ClassStudentsAggregateTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkManyToManyTest.ConnectionString";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            // Test script executor (create database)
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("master"),
@"
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'DomainFrameworkManyToManyTest'
)
BEGIN
    DROP DATABASE DomainFrameworkManyToManyTest
END
GO

CREATE DATABASE DomainFrameworkManyToManyTest
GO

USE DomainFrameworkManyToManyTest
GO

CREATE TABLE DomainFrameworkManyToManyTest..Class(
    [ClassId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] VARCHAR(50)
)

ALTER TABLE DomainFrameworkManyToManyTest..Class
ADD CONSTRAINT Class_PK PRIMARY KEY (ClassId)
GO

CREATE TABLE DomainFrameworkManyToManyTest..Student(
    [StudentId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [FirstName] VARCHAR(50),
)

ALTER TABLE DomainFrameworkManyToManyTest..Student
ADD CONSTRAINT Student_PK PRIMARY KEY (StudentId)

CREATE TABLE DomainFrameworkManyToManyTest..ClassEnrollment(
    [ClassId] UNIQUEIDENTIFIER NOT NULL,
    [StudentId] UNIQUEIDENTIFIER NOT NULL,
    [StartedDateTime] DATETIME NOT NULL
)

ALTER TABLE DomainFrameworkManyToManyTest..ClassEnrollment
ADD CONSTRAINT ClassEnrollment_PK PRIMARY KEY (ClassId, StudentId)

ALTER TABLE DomainFrameworkManyToManyTest..ClassEnrollment
ADD CONSTRAINT ClassEnrollment_Class_FK FOREIGN KEY (ClassId) REFERENCES DomainFrameworkManyToManyTest..Class(ClassId);

ALTER TABLE DomainFrameworkManyToManyTest..ClassEnrollment
ADD CONSTRAINT ClassEnrollment_Student_FK FOREIGN KEY (StudentId) REFERENCES DomainFrameworkManyToManyTest..Student(StudentId);
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
@"
CREATE PROCEDURE [p_Class_Create]
    @name VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [ClassId] UNIQUEIDENTIFIER NOT NULL
    );

    INSERT INTO Class
    (
        [Name]
    )
    OUTPUT
        INSERTED.[ClassId]
    INTO @outputData
    VALUES
    (
        @name
    );

    SELECT
        [ClassId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Student_Create]
    @firstName VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [StudentId] UNIQUEIDENTIFIER NOT NULL
    );

    INSERT INTO Student
    (
        [FirstName]
    )
    OUTPUT
        INSERTED.[StudentId]
    INTO @outputData
    VALUES
    (
        @firstName
    );

    SELECT
        [StudentId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_ClassEnrollment_Create]
    @classId UNIQUEIDENTIFIER,
    @studentId UNIQUEIDENTIFIER,
    @startedDateTime DATETIME
AS
BEGIN

    INSERT INTO ClassEnrollment
    (
        [ClassId],
        [StudentId],
        [StartedDateTime]
    )
    VALUES
    (
        @classId,
        @studentId,
        @startedDateTime
    );

END;
GO

CREATE PROCEDURE [p_Class_Get]
    @classId UNIQUEIDENTIFIER
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [ClassId],
        [Name]
    FROM [Class]
    WHERE [ClassId] = @classId

END;
GO

CREATE PROCEDURE [p_Student_Get]
    @studentId UNIQUEIDENTIFIER
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [FirstName]
    FROM [Student]
    WHERE [StudentId] = @studentId

END;
GO

CREATE PROCEDURE [p_Class_GetStudents]
    @classId UNIQUEIDENTIFIER
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        s.[StudentId] AS Id,
        s.[FirstName]
    FROM [Student] s
    INNER JOIN [ClassEnrollment] ce
        ON s.[StudentId] = ce.[StudentId]
    WHERE [ClassId] = @classId

END;
GO

CREATE PROCEDURE [p_Class_Update]
    @classId UNIQUEIDENTIFIER,
    @name VARCHAR(50)
AS
BEGIN

    UPDATE Class
    SET
        [Name] = @name
    WHERE [ClassId] = @classId;

END;
GO

CREATE PROCEDURE [p_Student_Update]
    @studentId UNIQUEIDENTIFIER,
    @firstName VARCHAR(50)
AS
BEGIN

    UPDATE Student
    SET
        [FirstName] = @firstName
    WHERE [StudentId] = @studentId;

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
        public void Class_Aggregate_With_Students_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<ClassEntity>(() => new ClassCommandRepository());

            context.RegisterCommandRepositoryFactory<StudentEntity>(() => new StudentCommandRepository());

            context.RegisterCommandRepositoryFactory<ClassEnrollmentEntity>(() => new ClassEnrollmentCommandRepository());

            // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database

            var classEntity = new ClassEntity
            {
                Name = "Programming"
            };

            var classAggregate = new ClassEnrollmentCommandAggregate(context, classEntity);

            classAggregate.EnrollStudent(new StudentEntity
            {
                FirstName = "Sarah"
            }, 
            new DateTime(2017, 3, 12, 9, 16, 37));

            classAggregate.EnrollStudent(new StudentEntity
            {
                FirstName = "Yana"
            },
            new DateTime(2017, 4, 12, 10, 26, 47));

            classAggregate.EnrollStudent(new StudentEntity
            {
                FirstName = "Mark"
            },
            new DateTime(2017, 5, 14, 11, 24, 57));

            classAggregate.Save();

            // Read

            context.RegisterQueryRepository<ClassEntity>(new ClassQueryRepository());

            context.RegisterQueryRepository<StudentEntity>(new StudentQueryRepository());

            var id = classEntity.Id; // Keep the generated id

            classEntity = new ClassEntity(); // New entity

            var queryAggregate = new ClassEnrollmentQueryAggregate(context);

            queryAggregate.Load(id);

            classEntity = queryAggregate.RootEntity;

            Assert.AreEqual(id, classEntity.Id);

            Assert.AreEqual("Programming", classEntity.Name);

            Assert.AreEqual(3, queryAggregate.Students.Count());

            var student = queryAggregate.Students.ElementAt(0);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Sarah", student.FirstName);

            student = queryAggregate.Students.ElementAt(1);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Yana", student.FirstName);

            student = queryAggregate.Students.ElementAt(2);

            Assert.IsNotNull(student.Id);

            Assert.AreEqual("Mark", student.FirstName);
        }
    }
}

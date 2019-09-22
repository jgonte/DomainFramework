
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'ClassesWithStudents'
)
BEGIN
    DROP DATABASE [ClassesWithStudents]
END
GO

CREATE DATABASE [ClassesWithStudents]
GO

USE [ClassesWithStudents]
GO

CREATE SCHEMA [ClassBoundedContext];
GO

CREATE TABLE [ClassesWithStudents].[ClassBoundedContext].[Class]
(
    [ClassId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(100) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT Class_PK PRIMARY KEY ([ClassId])
);
GO

CREATE TABLE [ClassesWithStudents].[ClassBoundedContext].[Student]
(
    [StudentId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT Student_PK PRIMARY KEY ([StudentId])
);
GO

CREATE TABLE [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
(
    [ClassId] INT NOT NULL,
    [StudentId] INT NOT NULL,
    [Name] VARCHAR(25) NOT NULL,
    [StartedDateTime] DATETIME NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT ClassEnrollment_PK PRIMARY KEY ([ClassId], [StudentId])
);
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_DeleteStudents]
    @classId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [ClassId] = @classId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_Delete]
    @classId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[Class]
    WHERE [ClassId] = @classId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_Insert]
    @name VARCHAR(100),
    @createdBy INT
AS
BEGIN
    DECLARE @classOutputData TABLE
    (
        [ClassId] INT
    );

    INSERT INTO [ClassesWithStudents].[ClassBoundedContext].[Class]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[ClassId]
        INTO @classOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        [ClassId]
    FROM @classOutputData;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_Update]
    @classId INT,
    @name VARCHAR(100),
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[Class]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [ClassId] = @classId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_Get]
AS
BEGIN
    SELECT
        c.[ClassId] AS "Id",
        c.[Name] AS "Name"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Class] c;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_GetById]
    @classId INT
AS
BEGIN
    SELECT
        c.[ClassId] AS "Id",
        c.[Name] AS "Name"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Class] c
    WHERE c.[ClassId] = @classId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_DeleteStudents]
    @classId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [ClassId] = @classId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_DeleteClasses]
    @studentId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_Delete]
    @classId INT,
    @studentId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [ClassId] = @classId
    AND [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_Insert]
    @classId INT,
    @studentId INT,
    @name VARCHAR(25),
    @startedDateTime DATETIME,
    @createdBy INT
AS
BEGIN
    INSERT INTO [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    (
        [ClassId],
        [StudentId],
        [Name],
        [StartedDateTime],
        [CreatedBy]
    )
    VALUES
    (
        @classId,
        @studentId,
        @name,
        @startedDateTime,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_Update]
    @classId INT,
    @studentId INT,
    @name VARCHAR(25),
    @startedDateTime DATETIME,
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    SET
        [Name] = @name,
        [StartedDateTime] = @startedDateTime,
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [ClassId] = @classId
    AND [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_Get]
AS
BEGIN
    SELECT
        c.[ClassId] AS "Id.ClassId",
        c.[StudentId] AS "Id.StudentId",
        c.[Name] AS "Name",
        c.[StartedDateTime] AS "StartedDateTime"
    FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_GetById]
    @classId INT,
    @studentId INT
AS
BEGIN
    SELECT
        c.[ClassId] AS "Id.ClassId",
        c.[StudentId] AS "Id.StudentId",
        c.[Name] AS "Name",
        c.[StartedDateTime] AS "StartedDateTime"
    FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c
    WHERE c.[ClassId] = @classId
    AND c.[StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_DeleteClasses]
    @studentId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_Delete]
    @studentId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[Student]
    WHERE [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_Insert]
    @firstName VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @studentOutputData TABLE
    (
        [StudentId] INT
    );

    INSERT INTO [ClassesWithStudents].[ClassBoundedContext].[Student]
    (
        [FirstName],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[StudentId]
        INTO @studentOutputData
    VALUES
    (
        @firstName,
        @createdBy
    );

    SELECT
        [StudentId]
    FROM @studentOutputData;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_Update]
    @studentId INT,
    @firstName VARCHAR(50),
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[Student]
    SET
        [FirstName] = @firstName,
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_Get]
AS
BEGIN
    SELECT
        s.[StudentId] AS "Id",
        s.[FirstName] AS "FirstName"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Student] s;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_GetById]
    @studentId INT
AS
BEGIN
    SELECT
        s.[StudentId] AS "Id",
        s.[FirstName] AS "FirstName"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Student] s
    WHERE s.[StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_GetStudents_ForClassEnrollment]
    @classId INT
AS
BEGIN
    SELECT
        s.[StudentId] AS "Id",
        s.[FirstName] AS "FirstName"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Student] s
    INNER JOIN [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c
        ON s.[StudentId] = c.[StudentId]
    WHERE c.[ClassId] = @classId;

END;
GO


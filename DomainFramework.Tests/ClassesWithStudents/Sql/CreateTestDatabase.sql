
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
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Class_PK PRIMARY KEY ([ClassId])
);
GO

CREATE TABLE [ClassesWithStudents].[ClassBoundedContext].[Student]
(
    [StudentId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Student_PK PRIMARY KEY ([StudentId])
);
GO

CREATE TABLE [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
(
    [ClassId] INT NOT NULL,
    [StudentId] INT NOT NULL,
    [StartedDateTime] DATETIME NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
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
    @updatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[Class]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
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
    @startedDateTime DATETIME,
    @createdBy INT
AS
BEGIN
    INSERT INTO [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    (
        [ClassId],
        [StudentId],
        [StartedDateTime],
        [CreatedBy]
    )
    VALUES
    (
        @classId,
        @studentId,
        @startedDateTime,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClassEnrollment_Update]
    @classId INT,
    @studentId INT,
    @startedDateTime DATETIME,
    @updatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    SET
        [StartedDateTime] = @startedDateTime,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
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
    @updatedBy INT
AS
BEGIN
    UPDATE [ClassesWithStudents].[ClassBoundedContext].[Student]
    SET
        [FirstName] = @firstName,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
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


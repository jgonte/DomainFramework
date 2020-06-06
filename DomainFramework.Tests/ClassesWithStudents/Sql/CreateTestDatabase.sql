
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
    @updatedBy INT = NULL
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

CREATE PROCEDURE [ClassBoundedContext].[pClass_AddStudents]
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

CREATE PROCEDURE [ClassBoundedContext].[pClass_LinkStudents]
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

CREATE PROCEDURE [ClassBoundedContext].[pClass_UnlinkStudents]
    @classId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [ClassId] = @classId;

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
    @updatedBy INT = NULL
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
    @updatedBy INT = NULL
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

CREATE PROCEDURE [ClassBoundedContext].[pStudent_AddClasses]
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

CREATE PROCEDURE [ClassBoundedContext].[pStudent_LinkClasses]
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

CREATE PROCEDURE [ClassBoundedContext].[pStudent_UnlinkClasses]
    @studentId INT
AS
BEGIN
    DELETE FROM [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment]
    WHERE [StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[ClassId] AS "Id",
    c.[Name] AS "Name"',
        @from = N'[ClassesWithStudents].[ClassBoundedContext].[Class] c',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pClass_GetAll]
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

CREATE PROCEDURE [ClassBoundedContext].[pStudent_GetAllClasses]
    @studentId INT
AS
BEGIN
    SELECT
        c.[ClassId] AS "Id",
        c.[Name] AS "Name"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Class] c
    INNER JOIN [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c1
        ON c.[ClassId] = c1.[ClassId]
    WHERE c1.[StudentId] = @studentId;

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    s.[StudentId] AS "Id",
    s.[FirstName] AS "FirstName"',
        @from = N'[ClassesWithStudents].[ClassBoundedContext].[Student] s',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[pStudent_GetAll]
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

CREATE PROCEDURE [ClassBoundedContext].[pClass_GetAllStudents]
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

CREATE PROCEDURE [ClassBoundedContext].[Student_GetEnrolled]
    @classId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N'c.[ClassId] = @classId';
    END
    ELSE
    BEGIN
        SET @$filter = N'c.[ClassId] = @classId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    s.[StudentId] AS "Id",
    s.[FirstName] AS "FirstName"',
        @from = N'[ClassesWithStudents].[ClassBoundedContext].[Student] s
INNER JOIN [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c
    ON s.[StudentId] = c.[StudentId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ClassBoundedContext].[Student_GetNotEnrolled]
    @classId INT
AS
BEGIN
    SELECT
        s.[StudentId] AS "Id",
        s.[FirstName] AS "FirstName"
    FROM [ClassesWithStudents].[ClassBoundedContext].[Student] s
    LEFT OUTER JOIN [ClassesWithStudents].[ClassBoundedContext].[ClassEnrollment] c
        ON s.[StudentId] = c.[StudentId]
    WHERE c.[ClassId] <> @classId
    OR c.[ClassId] IS NULL;

END;
GO

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@from NVARCHAR(MAX),
	@count INT OUTPUT
AS
BEGIN

	DECLARE @sqlCommand NVARCHAR(MAX);
	DECLARE @paramDefinition NVARCHAR(100);

	SET @paramDefinition = N'@cnt INT OUTPUT'

	SET @sqlCommand = 
'
	SELECT
		 @cnt = COUNT(1)
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	SET @sqlCommand = @sqlCommand + 
'
	SELECT
	';

	IF ISNULL(@$select, '*') = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	IF @$orderby IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY ' + @$orderby;
	END
	ELSE
	BEGIN

	-- At least a dummy order by is required is $skip and $top are provided
		IF @$skip IS NOT NULL OR @$top IS NOT NULL
		BEGIN  
			SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY 1 ASC';
		END
	END

	IF @$skip IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	OFFSET ' + @$skip + ' ROWS';
	END

	IF @$top IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	FETCH NEXT ' + @$top + ' ROWS ONLY';
	END

	EXECUTE sp_executesql @sqlCommand, @paramDefinition, @cnt = @count OUTPUT

END;
GO



USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'CourseWithPreRequisites'
)
BEGIN
    DROP DATABASE [CourseWithPreRequisites]
END
GO

CREATE DATABASE [CourseWithPreRequisites]
GO

USE [CourseWithPreRequisites]
GO

CREATE SCHEMA [CourseBoundedContext];
GO

CREATE TABLE [CourseWithPreRequisites].[CourseBoundedContext].[Course]
(
    [CourseId] INT NOT NULL IDENTITY,
    [Description] NVARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Course_PK PRIMARY KEY ([CourseId])
);
GO

CREATE TABLE [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
(
    [RequiredCourseId] INT NOT NULL,
    [CourseId] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT PreRequisite_PK PRIMARY KEY ([RequiredCourseId], [CourseId])
);
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_Delete]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisites].[CourseBoundedContext].[Course]
    WHERE [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_Insert]
    @description NVARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @courseOutputData TABLE
    (
        [CourseId] INT
    );

    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[Course]
    (
        [Description],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[CourseId]
        INTO @courseOutputData
    VALUES
    (
        @description,
        @createdBy
    );

    SELECT
        [CourseId]
    FROM @courseOutputData;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_Update]
    @courseId INT,
    @description NVARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [CourseWithPreRequisites].[CourseBoundedContext].[Course]
    SET
        [Description] = @description,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddIsRequiredBy]
    @requiredCourseId INT,
    @courseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    (
        [RequiredCourseId],
        [CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requiredCourseId,
        @courseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddRequires]
    @requiredCourseId INT,
    @courseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    (
        [RequiredCourseId],
        [CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requiredCourseId,
        @courseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkIsRequiredBy]
    @requiredCourseId INT,
    @courseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    (
        [RequiredCourseId],
        [CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requiredCourseId,
        @courseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkRequires]
    @requiredCourseId INT,
    @courseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    (
        [RequiredCourseId],
        [CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requiredCourseId,
        @courseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkIsRequiredBy]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    WHERE [RequiredCourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkRequires]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    WHERE [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Delete]
    @requiredCourseId INT,
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    WHERE [RequiredCourseId] = @requiredCourseId
    AND [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Insert]
    @requiredCourseId INT,
    @courseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    (
        [RequiredCourseId],
        [CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requiredCourseId,
        @courseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Update]
    @requiredCourseId INT,
    @courseId INT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [RequiredCourseId] = @requiredCourseId
    AND [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_Get]
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
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisites].[CourseBoundedContext].[Course] c',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAll]
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisites].[CourseBoundedContext].[Course] c;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetById]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisites].[CourseBoundedContext].[Course] c
    WHERE c.[CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllIsRequiredBy]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisites].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite] p
        ON c.[CourseId] = p.[CourseId]
    WHERE p.[RequiredCourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllRequires]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisites].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite] p
        ON c.[CourseId] = p.[RequiredCourseId]
    WHERE p.[CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetIsRequiredBy]
    @courseId INT,
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
        SET @$filter = N'p.[RequiredCourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[RequiredCourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisites].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite] p
    ON c.[CourseId] = p.[CourseId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetRequires]
    @courseId INT,
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
        SET @$filter = N'p.[CourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[CourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisites].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisites].[CourseBoundedContext].[PreRequisite] p
    ON c.[CourseId] = p.[RequiredCourseId]',
        @count = @count OUTPUT

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


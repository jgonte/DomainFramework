
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'CourseWithPreRequisitesAndRelated'
)
BEGIN
    DROP DATABASE [CourseWithPreRequisitesAndRelated]
END
GO

CREATE DATABASE [CourseWithPreRequisitesAndRelated]
GO

USE [CourseWithPreRequisitesAndRelated]
GO

CREATE SCHEMA [CourseBoundedContext];
GO

CREATE TABLE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course]
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

CREATE TABLE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
(
    [Requires_CourseId] INT NOT NULL,
    [IsRequiredBy_CourseId] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT PreRequisite_PK PRIMARY KEY ([Requires_CourseId], [IsRequiredBy_CourseId])
);
GO

CREATE TABLE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
(
    [Relates_CourseId] INT NOT NULL,
    [IsRelatedTo_CourseId] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT CourseRelation_PK PRIMARY KEY ([Relates_CourseId], [IsRelatedTo_CourseId])
);
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_Delete]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course]
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

    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course]
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
    UPDATE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course]
    SET
        [Description] = @description,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddIsRelatedTo]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    (
        [Relates_CourseId],
        [IsRelatedTo_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @relates_CourseId,
        @isRelatedTo_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddIsRequiredBy]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    (
        [Requires_CourseId],
        [IsRequiredBy_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requires_CourseId,
        @isRequiredBy_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddRelates]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    (
        [Relates_CourseId],
        [IsRelatedTo_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @relates_CourseId,
        @isRelatedTo_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_AddRequires]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    (
        [Requires_CourseId],
        [IsRequiredBy_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requires_CourseId,
        @isRequiredBy_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkIsRelatedTo]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    (
        [Relates_CourseId],
        [IsRelatedTo_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @relates_CourseId,
        @isRelatedTo_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkIsRequiredBy]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    (
        [Requires_CourseId],
        [IsRequiredBy_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requires_CourseId,
        @isRequiredBy_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkRelates]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    (
        [Relates_CourseId],
        [IsRelatedTo_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @relates_CourseId,
        @isRelatedTo_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_LinkRequires]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    (
        [Requires_CourseId],
        [IsRequiredBy_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requires_CourseId,
        @isRequiredBy_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkIsRelatedTo]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    WHERE [Relates_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkIsRequiredBy]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    WHERE [Requires_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkRelates]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    WHERE [IsRelatedTo_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_UnlinkRequires]
    @courseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    WHERE [IsRequiredBy_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourseRelation_Delete]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    WHERE [Relates_CourseId] = @relates_CourseId
    AND [IsRelatedTo_CourseId] = @isRelatedTo_CourseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourseRelation_Insert]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    (
        [Relates_CourseId],
        [IsRelatedTo_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @relates_CourseId,
        @isRelatedTo_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourseRelation_Update]
    @relates_CourseId INT,
    @isRelatedTo_CourseId INT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [Relates_CourseId] = @relates_CourseId
    AND [IsRelatedTo_CourseId] = @isRelatedTo_CourseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Delete]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT
AS
BEGIN
    DELETE FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    WHERE [Requires_CourseId] = @requires_CourseId
    AND [IsRequiredBy_CourseId] = @isRequiredBy_CourseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Insert]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    (
        [Requires_CourseId],
        [IsRequiredBy_CourseId],
        [CreatedBy]
    )
    VALUES
    (
        @requires_CourseId,
        @isRequiredBy_CourseId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pPreRequisite_Update]
    @requires_CourseId INT,
    @isRequiredBy_CourseId INT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [Requires_CourseId] = @requires_CourseId
    AND [IsRequiredBy_CourseId] = @isRequiredBy_CourseId;

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
        @from = N'[CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAll]
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetById]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
    WHERE c.[CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllIsRelatedTo]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation] c1
        ON c.[CourseId] = c1.[IsRelatedTo_CourseId]
    WHERE c1.[Relates_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllIsRequiredBy]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite] p
        ON c.[CourseId] = p.[IsRequiredBy_CourseId]
    WHERE p.[Requires_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllRelates]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation] c1
        ON c.[CourseId] = c1.[Relates_CourseId]
    WHERE c1.[IsRelatedTo_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetAllRequires]
    @courseId INT
AS
BEGIN
    SELECT
        c.[CourseId] AS "Id",
        c.[Description] AS "Description"
    FROM [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
    INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite] p
        ON c.[CourseId] = p.[Requires_CourseId]
    WHERE p.[IsRequiredBy_CourseId] = @courseId;

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetIsRelatedTo]
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
        SET @$filter = N'c1.[Relates_CourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'c1.[Relates_CourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation] c1
    ON c.[CourseId] = c1.[IsRelatedTo_CourseId]',
        @count = @count OUTPUT

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
        SET @$filter = N'p.[Requires_CourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[Requires_CourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite] p
    ON c.[CourseId] = p.[IsRequiredBy_CourseId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [CourseBoundedContext].[pCourse_GetRelates]
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
        SET @$filter = N'c1.[IsRelatedTo_CourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'c1.[IsRelatedTo_CourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[CourseRelation] c1
    ON c.[CourseId] = c1.[Relates_CourseId]',
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
        SET @$filter = N'p.[IsRequiredBy_CourseId] = ' + CAST(@courseId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[IsRequiredBy_CourseId] = ' + CAST(@courseId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CourseId] AS "Id",
    c.[Description] AS "Description"',
        @from = N'[CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[Course] c
INNER JOIN [CourseWithPreRequisitesAndRelated].[CourseBoundedContext].[PreRequisite] p
    ON c.[CourseId] = p.[Requires_CourseId]',
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


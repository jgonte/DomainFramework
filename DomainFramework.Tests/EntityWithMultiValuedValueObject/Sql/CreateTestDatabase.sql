
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'EntityWithMultiValuedValueObject'
)
BEGIN
    DROP DATABASE [EntityWithMultiValuedValueObject]
END
GO

CREATE DATABASE [EntityWithMultiValuedValueObject]
GO

USE [EntityWithMultiValuedValueObject]
GO

CREATE TABLE [EntityWithMultiValuedValueObject]..[TestEntity]
(
    [TestEntityId] INT NOT NULL IDENTITY,
    [Text] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT TestEntity_PK PRIMARY KEY ([TestEntityId])
);
GO

CREATE TABLE [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1]
(
    [TestEntityId] INT NOT NULL,
    [DataType] INT NOT NULL,
    [Data] VARCHAR(200) NOT NULL
);
GO

ALTER TABLE [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1]
    ADD CONSTRAINT TestEntity_TypeValues1_FK FOREIGN KEY ([TestEntityId])
        REFERENCES [EntityWithMultiValuedValueObject]..[TestEntity] ([TestEntityId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [TestEntity_TypeValues1_FK_IX]
    ON [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1]
    (
        [TestEntityId]
    );
GO

CREATE PROCEDURE [pTestEntity_Delete]
    @testEntityId INT
AS
BEGIN
    DELETE FROM [EntityWithMultiValuedValueObject]..[TestEntity]
    WHERE [TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [pTestEntity_Insert]
    @text VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @testEntityOutputData TABLE
    (
        [TestEntityId] INT
    );

    INSERT INTO [EntityWithMultiValuedValueObject]..[TestEntity]
    (
        [Text],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[TestEntityId]
        INTO @testEntityOutputData
    VALUES
    (
        @text,
        @createdBy
    );

    SELECT
        [TestEntityId]
    FROM @testEntityOutputData;

END;
GO

CREATE PROCEDURE [pTestEntity_Update]
    @testEntityId INT,
    @text VARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [EntityWithMultiValuedValueObject]..[TestEntity]
    SET
        [Text] = @text,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [pTestEntity_AddTypeValues1]
    @testEntityId INT,
    @dataType INT,
    @data VARCHAR(200)
AS
BEGIN
    INSERT INTO [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1]
    (
        [TestEntityId],
        [DataType],
        [Data]
    )
    VALUES
    (
        @testEntityId,
        @dataType,
        @data
    );

END;
GO

CREATE PROCEDURE [pTestEntity_DeleteTypeValues1]
    @testEntityId INT
AS
BEGIN
    DELETE FROM [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1]
    WHERE [TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [pTestEntity_Get]
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
        @selectList = N'    t.[TestEntityId] AS "Id",
    t.[Text] AS "Text"',
        @from = N'[EntityWithMultiValuedValueObject]..[TestEntity] t',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [pTestEntity_GetAll]
AS
BEGIN
    SELECT
        t.[TestEntityId] AS "Id",
        t.[Text] AS "Text"
    FROM [EntityWithMultiValuedValueObject]..[TestEntity] t;

END;
GO

CREATE PROCEDURE [pTestEntity_GetById]
    @testEntityId INT
AS
BEGIN
    SELECT
        t.[TestEntityId] AS "Id",
        t.[Text] AS "Text"
    FROM [EntityWithMultiValuedValueObject]..[TestEntity] t
    WHERE t.[TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [pTestEntity_GetAllTypeValues1]
    @testEntityId INT
AS
BEGIN
    SELECT
        t.[DataType] AS "DataType",
        t.[Data] AS "Data"
    FROM [EntityWithMultiValuedValueObject]..[TestEntity_TypeValues1] t
    WHERE t.[TestEntityId] = @testEntityId;

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



USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'OrganizationPersonWithCommonEntities'
)
BEGIN
    DROP DATABASE [OrganizationPersonWithCommonEntities]
END
GO

CREATE DATABASE [OrganizationPersonWithCommonEntities]
GO

USE [OrganizationPersonWithCommonEntities]
GO

CREATE SCHEMA [OrganizationPersonBoundedContext];
GO

CREATE TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
(
    [AddressId] INT NOT NULL IDENTITY,
    [Street] VARCHAR(25) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [OrganizationId] INT,
    [PersonId] INT
    CONSTRAINT Address_PK PRIMARY KEY ([AddressId])
);
GO

CREATE TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
(
    [PhoneId] INT NOT NULL IDENTITY,
    [Number] VARCHAR(15) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [OrganizationId] INT,
    [PersonId] INT
    CONSTRAINT Phone_PK PRIMARY KEY ([PhoneId])
);
GO

CREATE TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization]
(
    [OrganizationId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Organization_PK PRIMARY KEY ([OrganizationId])
);
GO

CREATE TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    ADD CONSTRAINT Organization_Address_FK FOREIGN KEY ([OrganizationId])
        REFERENCES [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization] ([OrganizationId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Organization_Address_FK_IX]
    ON [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    (
        [OrganizationId]
    );
GO

ALTER TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    ADD CONSTRAINT Organization_Phones_FK FOREIGN KEY ([OrganizationId])
        REFERENCES [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization] ([OrganizationId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Organization_Phones_FK_IX]
    ON [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    (
        [OrganizationId]
    );
GO

ALTER TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    ADD CONSTRAINT Person_Address_FK FOREIGN KEY ([PersonId])
        REFERENCES [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Address_FK_IX]
    ON [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    (
        [PersonId]
    );
GO

ALTER TABLE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    ADD CONSTRAINT Person_Phones_FK FOREIGN KEY ([PersonId])
        REFERENCES [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Phones_FK_IX]
    ON [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    (
        [PersonId]
    );
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_Delete]
    @addressId INT
AS
BEGIN
    DELETE FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_Insert]
    @street VARCHAR(25),
    @createdBy INT,
    @organizationId INT = NULL,
    @personId INT = NULL
AS
BEGIN
    DECLARE @addressOutputData TABLE
    (
        [AddressId] INT
    );

    INSERT INTO [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    (
        [Street],
        [CreatedBy],
        [OrganizationId],
        [PersonId]
    )
    OUTPUT
        INSERTED.[AddressId]
        INTO @addressOutputData
    VALUES
    (
        @street,
        @createdBy,
        @organizationId,
        @personId
    );

    SELECT
        [AddressId]
    FROM @addressOutputData;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_Update]
    @addressId INT,
    @street VARCHAR(25),
    @updatedBy INT = NULL,
    @organizationId INT = NULL,
    @personId INT = NULL
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    SET
        [Street] = @street,
        [UpdatedBy] = @updatedBy,
        [OrganizationId] = @organizationId,
        [PersonId] = @personId,
        [UpdatedDateTime] = GETDATE()
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_Delete]
    @organizationId INT
AS
BEGIN
    DELETE FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_Insert]
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @organizationOutputData TABLE
    (
        [OrganizationId] INT
    );

    INSERT INTO [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[OrganizationId]
        INTO @organizationOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        [OrganizationId]
    FROM @organizationOutputData;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_Update]
    @organizationId INT,
    @name VARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_UnlinkAddress]
    @organizationId INT
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    SET
        [OrganizationId] = NULL
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_UnlinkPhones]
    @organizationId INT
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    SET
        [OrganizationId] = NULL
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_UnlinkAddress]
    @personId INT
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address]
    SET
        [PersonId] = NULL
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_UnlinkPhones]
    @personId INT
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    SET
        [PersonId] = NULL
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_Delete]
    @phoneId INT
AS
BEGIN
    DELETE FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    WHERE [PhoneId] = @phoneId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_Insert]
    @number VARCHAR(15),
    @createdBy INT,
    @organizationId INT = NULL,
    @personId INT = NULL
AS
BEGIN
    DECLARE @phoneOutputData TABLE
    (
        [PhoneId] INT
    );

    INSERT INTO [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    (
        [Number],
        [CreatedBy],
        [OrganizationId],
        [PersonId]
    )
    OUTPUT
        INSERTED.[PhoneId]
        INTO @phoneOutputData
    VALUES
    (
        @number,
        @createdBy,
        @organizationId,
        @personId
    );

    SELECT
        [PhoneId]
    FROM @phoneOutputData;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_Update]
    @phoneId INT,
    @number VARCHAR(15),
    @updatedBy INT = NULL,
    @organizationId INT = NULL,
    @personId INT = NULL
AS
BEGIN
    UPDATE [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone]
    SET
        [Number] = @number,
        [UpdatedBy] = @updatedBy,
        [OrganizationId] = @organizationId,
        [PersonId] = @personId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PhoneId] = @phoneId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_Get]
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
        @selectList = N'    a.[AddressId] AS "Id",
    a.[Street] AS "Street",
    a.[OrganizationId] AS "OrganizationId",
    a.[PersonId] AS "PersonId"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address] a',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_GetAll]
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street",
        a.[OrganizationId] AS "OrganizationId",
        a.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address] a;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pAddress_GetById]
    @addressId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street",
        a.[OrganizationId] AS "OrganizationId",
        a.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address] a
    WHERE a.[AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_GetAddress]
    @organizationId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street",
        a.[OrganizationId] AS "OrganizationId",
        a.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address] a
    WHERE a.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_GetAddress]
    @personId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street",
        a.[OrganizationId] AS "OrganizationId",
        a.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Address] a
    WHERE a.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_Get]
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
        @selectList = N'    o.[OrganizationId] AS "Id",
    o.[Name] AS "Name"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization] o',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_GetAll]
AS
BEGIN
    SELECT
        o.[OrganizationId] AS "Id",
        o.[Name] AS "Name"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization] o;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_GetById]
    @organizationId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS "Id",
        o.[Name] AS "Name"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Organization] o
    WHERE o.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_Get]
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
        @selectList = N'    p.[PersonId] AS "Id",
    p.[Name] AS "Name"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_GetAll]
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person] p;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Person] p
    WHERE p.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_Get]
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
        @selectList = N'    p.[PhoneId] AS "Id",
    p.[Number] AS "Number",
    p.[OrganizationId] AS "OrganizationId",
    p.[PersonId] AS "PersonId"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_GetAll]
AS
BEGIN
    SELECT
        p.[PhoneId] AS "Id",
        p.[Number] AS "Number",
        p.[OrganizationId] AS "OrganizationId",
        p.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPhone_GetById]
    @phoneId INT
AS
BEGIN
    SELECT
        p.[PhoneId] AS "Id",
        p.[Number] AS "Number",
        p.[OrganizationId] AS "OrganizationId",
        p.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p
    WHERE p.[PhoneId] = @phoneId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_GetAllPhones]
    @organizationId INT
AS
BEGIN
    SELECT
        p.[PhoneId] AS "Id",
        p.[Number] AS "Number",
        p.[OrganizationId] AS "OrganizationId",
        p.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p
    WHERE p.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_GetAllPhones]
    @personId INT
AS
BEGIN
    SELECT
        p.[PhoneId] AS "Id",
        p.[Number] AS "Number",
        p.[OrganizationId] AS "OrganizationId",
        p.[PersonId] AS "PersonId"
    FROM [OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p
    WHERE p.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pOrganization_GetPhones]
    @organizationId INT,
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
        SET @$filter = N'p.[OrganizationId] = @organizationId';
    END
    ELSE
    BEGIN
        SET @$filter = N'p.[OrganizationId] = @organizationId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    p.[PhoneId] AS "Id",
    p.[Number] AS "Number",
    p.[OrganizationId] AS "OrganizationId",
    p.[PersonId] AS "PersonId"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [OrganizationPersonBoundedContext].[pPerson_GetPhones]
    @personId INT,
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
        SET @$filter = N'p.[PersonId] = @personId';
    END
    ELSE
    BEGIN
        SET @$filter = N'p.[PersonId] = @personId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    p.[PhoneId] AS "Id",
    p.[Number] AS "Number",
    p.[OrganizationId] AS "OrganizationId",
    p.[PersonId] AS "PersonId"',
        @from = N'[OrganizationPersonWithCommonEntities].[OrganizationPersonBoundedContext].[Phone] p',
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


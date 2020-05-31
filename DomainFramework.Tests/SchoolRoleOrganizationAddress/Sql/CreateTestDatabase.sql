
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'SchoolRoleOrganizationAddress'
)
BEGIN
    DROP DATABASE [SchoolRoleOrganizationAddress]
END
GO

CREATE DATABASE [SchoolRoleOrganizationAddress]
GO

USE [SchoolRoleOrganizationAddress]
GO

CREATE SCHEMA [SchoolBoundedContext];
GO

CREATE TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address]
(
    [AddressId] INT NOT NULL IDENTITY,
    [Street] VARCHAR(50) NOT NULL
    CONSTRAINT Address_PK PRIMARY KEY ([AddressId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
(
    [OrganizationId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [Phone_Number] VARCHAR(10) NOT NULL,
    [AddressId] INT
    CONSTRAINT Organization_PK PRIMARY KEY ([OrganizationId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
(
    [RoleId] INT NOT NULL IDENTITY,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Role_PK PRIMARY KEY ([RoleId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
(
    [SchoolId] INT NOT NULL,
    [IsCharter] BIT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT School_PK PRIMARY KEY ([SchoolId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
(
    [OrganizationId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT OrganizationRole_PK PRIMARY KEY ([OrganizationId], [RoleId])
);
GO

ALTER TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
    ADD CONSTRAINT School_Role_IFK FOREIGN KEY ([SchoolId])
        REFERENCES [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] ([RoleId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [School_Role_IFK_IX]
    ON [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
    (
        [SchoolId]
    );
GO

ALTER TABLE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    ADD CONSTRAINT Address_Organizations_FK FOREIGN KEY ([AddressId])
        REFERENCES [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address] ([AddressId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Address_Organizations_FK_IX]
    ON [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    (
        [AddressId]
    );
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_Delete]
    @addressId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address]
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_Insert]
    @street VARCHAR(50)
AS
BEGIN
    DECLARE @addressOutputData TABLE
    (
        [AddressId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address]
    (
        [Street]
    )
    OUTPUT
        INSERTED.[AddressId]
        INTO @addressOutputData
    VALUES
    (
        @street
    );

    SELECT
        [AddressId]
    FROM @addressOutputData;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_Update]
    @addressId INT,
    @street VARCHAR(50)
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address]
    SET
        [Street] = @street
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_Delete]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @phone_Number VARCHAR(10),
    @addressId INT = NULL
AS
BEGIN
    DECLARE @organizationOutputData TABLE
    (
        [OrganizationId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    (
        [Name],
        [CreatedBy],
        [Phone_Number],
        [AddressId]
    )
    OUTPUT
        INSERTED.[OrganizationId]
        INTO @organizationOutputData
    VALUES
    (
        @name,
        @createdBy,
        @phone_Number,
        @addressId
    );

    SELECT
        [OrganizationId]
    FROM @organizationOutputData;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_Update]
    @organizationId INT,
    @name VARCHAR(50),
    @updatedBy INT = NULL,
    @phone_Number VARCHAR(10),
    @addressId INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [Phone_Number] = @phone_Number,
        [AddressId] = @addressId,
        [UpdatedDateTime] = GETDATE()
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_AddAddress]
    @organizationId INT,
    @addressId INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    SET
        [AddressId] = @addressId
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_LinkAddress]
    @organizationId INT,
    @addressId INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    SET
        [AddressId] = @addressId
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_UnlinkAddress]
    @organizationId INT
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization]
    SET
        [AddressId] = NULL
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganizationRole_Delete]
    @organizationId INT,
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    WHERE [OrganizationId] = @organizationId
    AND [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganizationRole_Insert]
    @organizationId INT,
    @roleId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    (
        [OrganizationId],
        [RoleId],
        [CreatedBy]
    )
    VALUES
    (
        @organizationId,
        @roleId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganizationRole_Update]
    @organizationId INT,
    @roleId INT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [OrganizationId] = @organizationId
    AND [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_Delete]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_Insert]
    @createdBy INT
AS
BEGIN
    DECLARE @roleOutputData TABLE
    (
        [RoleId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
    (
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[RoleId]
        INTO @roleOutputData
    VALUES
    (
        @createdBy
    );

    SELECT
        [RoleId]
    FROM @roleOutputData;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_Update]
    @roleId INT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_AddOrganization]
    @organizationId INT,
    @roleId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    (
        [OrganizationId],
        [RoleId],
        [CreatedBy]
    )
    VALUES
    (
        @organizationId,
        @roleId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_LinkOrganization]
    @organizationId INT,
    @roleId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    (
        [OrganizationId],
        [RoleId],
        [CreatedBy]
    )
    VALUES
    (
        @organizationId,
        @roleId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_UnlinkOrganization]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_Delete]
    @schoolId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
    WHERE [SchoolId] = @schoolId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_Insert]
    @isCharter BIT,
    @createdBy INT
AS
BEGIN
    DECLARE @roleId INT;

    DECLARE @roleOutputData TABLE
    (
        [RoleId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
    (
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[RoleId]
        INTO @roleOutputData
    VALUES
    (
        @createdBy
    );

    SELECT
        @roleId = [RoleId]
    FROM @roleOutputData;

    INSERT INTO [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
    (
        [SchoolId],
        [IsCharter],
        [CreatedBy]
    )
    VALUES
    (
        @roleId,
        @isCharter,
        @createdBy
    );

    SELECT
        [RoleId]
    FROM @roleOutputData;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_Update]
    @schoolId INT,
    @isCharter BIT,
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role]
    SET
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [RoleId] = @schoolId;

    UPDATE [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School]
    SET
        [IsCharter] = @isCharter,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [SchoolId] = @schoolId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_Get]
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
    a.[Street] AS "Street"',
        @from = N'[SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address] a',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_GetAll]
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address] a;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pAddress_GetById]
    @addressId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address] a
    WHERE a.[AddressId] = @addressId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_GetAddress]
    @organizationId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS "Id",
        a.[Street] AS "Street"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Address] a
    INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization] o
        ON a.[AddressId] = o.[AddressId]
    WHERE o.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_Get]
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
    o.[Name] AS "Name",
    o.[Phone_Number] AS "Phone.Number",
    o.[AddressId] AS "AddressId"',
        @from = N'[SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization] o',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_GetAll]
AS
BEGIN
    SELECT
        o.[OrganizationId] AS "Id",
        o.[Name] AS "Name",
        o.[Phone_Number] AS "Phone.Number",
        o.[AddressId] AS "AddressId"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization] o;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pOrganization_GetById]
    @organizationId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS "Id",
        o.[Name] AS "Name",
        o.[Phone_Number] AS "Phone.Number",
        o.[AddressId] AS "AddressId"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization] o
    WHERE o.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_GetOrganization]
    @roleId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS "Id",
        o.[Name] AS "Name",
        o.[Phone_Number] AS "Phone.Number",
        o.[AddressId] AS "AddressId"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Organization] o
    INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[OrganizationRole] o1
        ON o.[OrganizationId] = o1.[OrganizationId]
    WHERE o1.[RoleId] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_Get]
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
        @selectList = N'    _q_.[Id] AS "Id",
    _q_.[IsCharter] AS "IsCharter",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        s.[SchoolId] AS "Id",
        s.[IsCharter] AS "IsCharter",
        1 AS "_EntityType_"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
    INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
        ON s.[SchoolId] = r.[RoleId]
    UNION ALL
    (
        SELECT
            r.[RoleId] AS "Id",
            NULL AS "IsCharter",
            2 AS "_EntityType_"
        FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
        LEFT OUTER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
            ON s.[SchoolId] = r.[RoleId]
        WHERE s.[SchoolId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[IsCharter] AS "IsCharter",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            s.[SchoolId] AS "Id",
            s.[IsCharter] AS "IsCharter",
            1 AS "_EntityType_"
        FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
        INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
            ON s.[SchoolId] = r.[RoleId]
        UNION ALL
        (
            SELECT
                r.[RoleId] AS "Id",
                NULL AS "IsCharter",
                2 AS "_EntityType_"
            FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
            LEFT OUTER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
                ON s.[SchoolId] = r.[RoleId]
            WHERE s.[SchoolId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pRole_GetById]
    @roleId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[IsCharter] AS "IsCharter",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            s.[SchoolId] AS "Id",
            s.[IsCharter] AS "IsCharter",
            1 AS "_EntityType_"
        FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
        INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
            ON s.[SchoolId] = r.[RoleId]
        UNION ALL
        (
            SELECT
                r.[RoleId] AS "Id",
                NULL AS "IsCharter",
                2 AS "_EntityType_"
            FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
            LEFT OUTER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
                ON s.[SchoolId] = r.[RoleId]
            WHERE s.[SchoolId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @roleId;

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_Get]
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
        @selectList = N'    s.[SchoolId] AS "Id",
    s.[IsCharter] AS "IsCharter"',
        @from = N'[SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
    ON s.[SchoolId] = r.[RoleId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_GetAll]
AS
BEGIN
    SELECT
        s.[SchoolId] AS "Id",
        s.[IsCharter] AS "IsCharter"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
    INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
        ON s.[SchoolId] = r.[RoleId];

END;
GO

CREATE PROCEDURE [SchoolBoundedContext].[pSchool_GetById]
    @schoolId INT
AS
BEGIN
    SELECT
        s.[SchoolId] AS "Id",
        s.[IsCharter] AS "IsCharter"
    FROM [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[School] s
    INNER JOIN [SchoolRoleOrganizationAddress].[SchoolBoundedContext].[Role] r
        ON s.[SchoolId] = r.[RoleId]
    WHERE s.[SchoolId] = @schoolId;

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


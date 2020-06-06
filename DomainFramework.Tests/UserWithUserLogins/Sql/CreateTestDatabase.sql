
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'UserWithUserLogins'
)
BEGIN
    DROP DATABASE [UserWithUserLogins]
END
GO

CREATE DATABASE [UserWithUserLogins]
GO

USE [UserWithUserLogins]
GO

CREATE SCHEMA [UserBoundedContext];
GO

CREATE TABLE [UserWithUserLogins].[UserBoundedContext].[User]
(
    [UserId] INT NOT NULL IDENTITY,
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [NormalizedEmail] NVARCHAR(256) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT User_PK PRIMARY KEY ([UserId])
);
GO

CREATE UNIQUE INDEX [NormalizedUserNameIndex]
    ON [UserWithUserLogins].[UserBoundedContext].[User]
    (
        [NormalizedUserName]
    );
GO

CREATE INDEX [NormalizedEmailIndex]
    ON [UserWithUserLogins].[UserBoundedContext].[User]
    (
        [NormalizedEmail]
    );
GO

CREATE TABLE [UserWithUserLogins].[UserBoundedContext].[User_UserLogins]
(
    [UserId] INT NOT NULL,
    [Provider] VARCHAR(128) NOT NULL,
    [UserKey] VARCHAR(128) NOT NULL
);
GO

ALTER TABLE [UserWithUserLogins].[UserBoundedContext].[User_UserLogins]
    ADD CONSTRAINT User_UserLogins_FK FOREIGN KEY ([UserId])
        REFERENCES [UserWithUserLogins].[UserBoundedContext].[User] ([UserId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [User_UserLogins_FK_IX]
    ON [UserWithUserLogins].[UserBoundedContext].[User_UserLogins]
    (
        [UserId]
    );
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Delete]
    @userId INT
AS
BEGIN

    EXECUTE [dbo].[SetUserContext] @userId;

    DELETE FROM [UserWithUserLogins].[UserBoundedContext].[User]
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Insert]
    @userName NVARCHAR(256),
    @normalizedUserName NVARCHAR(256),
    @email NVARCHAR(256),
    @normalizedEmail NVARCHAR(256),
    @createdBy INT
AS
BEGIN
    DECLARE @userOutputData TABLE
    (
        [UserId] INT
    );

    INSERT INTO [UserWithUserLogins].[UserBoundedContext].[User]
    (
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[UserId]
        INTO @userOutputData
    VALUES
    (
        @userName,
        @normalizedUserName,
        @email,
        @normalizedEmail,
        @createdBy
    );

    SELECT
        [UserId]
    FROM @userOutputData;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Update]
    @userId INT,
    @userName NVARCHAR(256),
    @normalizedUserName NVARCHAR(256),
    @email NVARCHAR(256),
    @normalizedEmail NVARCHAR(256),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [UserWithUserLogins].[UserBoundedContext].[User]
    SET
        [UserName] = @userName,
        [NormalizedUserName] = @normalizedUserName,
        [Email] = @email,
        [NormalizedEmail] = @normalizedEmail,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_AddUserLogins]
    @userId INT,
    @provider VARCHAR(128),
    @userKey VARCHAR(128)
AS
BEGIN
    INSERT INTO [UserWithUserLogins].[UserBoundedContext].[User_UserLogins]
    (
        [UserId],
        [Provider],
        [UserKey]
    )
    VALUES
    (
        @userId,
        @provider,
        @userKey
    );

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_DeleteUserLogins]
    @userId INT
AS
BEGIN

    EXECUTE [dbo].[SetUserContext] @userId;

    DELETE FROM [UserWithUserLogins].[UserBoundedContext].[User_UserLogins]
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Get]
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
        @selectList = N'    u.[UserId] AS "Id",
    u.[UserName] AS "UserName",
    u.[NormalizedUserName] AS "NormalizedUserName",
    u.[Email] AS "Email",
    u.[NormalizedEmail] AS "NormalizedEmail"',
        @from = N'[UserWithUserLogins].[UserBoundedContext].[User] u',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetAll]
AS
BEGIN
    SELECT
        u.[UserId] AS "Id",
        u.[UserName] AS "UserName",
        u.[NormalizedUserName] AS "NormalizedUserName",
        u.[Email] AS "Email",
        u.[NormalizedEmail] AS "NormalizedEmail"
    FROM [UserWithUserLogins].[UserBoundedContext].[User] u;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetById]
    @userId INT
AS
BEGIN
    SELECT
        u.[UserId] AS "Id",
        u.[UserName] AS "UserName",
        u.[NormalizedUserName] AS "NormalizedUserName",
        u.[Email] AS "Email",
        u.[NormalizedEmail] AS "NormalizedEmail"
    FROM [UserWithUserLogins].[UserBoundedContext].[User] u
    WHERE u.[UserId] = @userId;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetByNormalizedEmail]
    @email NVARCHAR(256)
AS
BEGIN
    SELECT
        u.[UserId] AS "Id",
        u.[UserName] AS "UserName",
        u.[NormalizedUserName] AS "NormalizedUserName",
        u.[Email] AS "Email",
        u.[NormalizedEmail] AS "NormalizedEmail"
    FROM [UserWithUserLogins].[UserBoundedContext].[User] u
    WHERE u.[NormalizedEmail] = UPPER(@email);

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetByUserLogin]
    @provider VARCHAR(128),
    @userKey VARCHAR(128)
AS
BEGIN
    SELECT
        u.[UserId] AS "Id",
        u.[UserName] AS "UserName",
        u.[NormalizedUserName] AS "NormalizedUserName",
        u.[Email] AS "Email",
        u.[NormalizedEmail] AS "NormalizedEmail"
    FROM [UserWithUserLogins].[UserBoundedContext].[User] u
    INNER JOIN [UserWithUserLogins].[UserBoundedContext].[User_UserLogins] ul
        ON u.[UserId] = ul.[UserId]
    WHERE ul.[Provider] = @provider
    AND ul.[UserKey] = @userKey;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetUserLogins]
    @userId INT,
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
        SET @$filter = N'u.[UserId] = @userId';
    END
    ELSE
    BEGIN
        SET @$filter = N'u.[UserId] = @userId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    u.[Provider] AS "Provider",
    u.[UserKey] AS "UserKey"',
        @from = N'[UserWithUserLogins].[UserBoundedContext].[User_UserLogins] u',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetAllUserLogins]
    @userId INT
AS
BEGIN
    SELECT
        u.[Provider] AS "Provider",
        u.[UserKey] AS "UserKey"
    FROM [UserWithUserLogins].[UserBoundedContext].[User_UserLogins] u
    WHERE u.[UserId] = @userId;

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


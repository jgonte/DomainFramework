
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'RegisterUser'
)
BEGIN
    DROP DATABASE [RegisterUser]
END
GO

CREATE DATABASE [RegisterUser]
GO

USE [RegisterUser]
GO

CREATE SCHEMA [UserBoundedContext];
GO

CREATE TABLE [RegisterUser].[UserBoundedContext].[User]
(
    [UserId] INT NOT NULL IDENTITY,
    [SubjectId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordSalt] NVARCHAR(50) NOT NULL,
    [PasswordHash] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL
    CONSTRAINT User_PK PRIMARY KEY ([UserId])
);
GO

CREATE UNIQUE INDEX [UsernameIndex]
    ON [RegisterUser].[UserBoundedContext].[User]
    (
        [Username]
    );
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Delete]
    @userId INT
AS
BEGIN

    EXECUTE [dbo].[SetUserContext] @userId;

    DELETE FROM [RegisterUser].[UserBoundedContext].[User]
    WHERE [UserId] = @userId;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_Insert]
    @username NVARCHAR(50),
    @passwordSalt NVARCHAR(50),
    @passwordHash NVARCHAR(50),
    @email NVARCHAR(256)
AS
BEGIN
    DECLARE @userOutputData TABLE
    (
        [UserId] INT,
        [SubjectId] UNIQUEIDENTIFIER
    );

    INSERT INTO [RegisterUser].[UserBoundedContext].[User]
    (
        [Username],
        [PasswordSalt],
        [PasswordHash],
        [Email]
    )
    OUTPUT
        INSERTED.[UserId],
        INSERTED.[SubjectId]
        INTO @userOutputData
    VALUES
    (
        @username,
        @passwordSalt,
        @passwordHash,
        @email
    );

    SELECT
        [UserId],
        [SubjectId]
    FROM @userOutputData;

END;
GO

CREATE PROCEDURE [UserBoundedContext].[pUser_GetUserByUserName]
    @username NVARCHAR(50)
AS
BEGIN
    SELECT
        u.[UserId] AS "Id",
        u.[SubjectId] AS "SubjectId",
        u.[Username] AS "Username",
        u.[PasswordSalt] AS "PasswordSalt",
        u.[PasswordHash] AS "PasswordHash",
        u.[Email] AS "Email"
    FROM [RegisterUser].[UserBoundedContext].[User] u
    WHERE u.[Username] = @username;

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


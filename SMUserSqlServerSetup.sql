Use SocialMedia
GO

--Create a user in the local database
IF NOT EXISTS(SELECT *
              FROM sys.server_principals sp
              WHERE sp.name = 'SMUser')
    BEGIN
        CREATE LOGIN SMUser WITH PASSWORD =N'SmPA$$w0rd', DEFAULT_DATABASE = SocialMedia
    END

IF NOT EXISTS(SELECT *
              FROM sys.database_principals dp
              WHERE dp.name = 'SMUser')
    BEGIN
        EXEC sp_adduser 'SMUser', 'SMUser', 'db_owner';
    END
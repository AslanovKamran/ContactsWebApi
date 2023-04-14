--GO
--USE master

--GO
--CREATE DATABASE ContactsTestDb

--GO
--USE ContactsTestDb

--GO
--CREATE TABLE Roles
--(
--[Id] INT PRIMARY KEY IDENTITY,
--[Name] NVARCHAR(100) UNIQUE NOT NULL,
--)

----Initital Roles
--GO
--INSERT INTO Roles VALUES (N'Admin');
--INSERT INTO Roles VALUES (N'User');


--GO
--CREATE TABLE Users
--(
--[Id] INT PRIMARY KEY IDENTITY,
--[Login] NVARCHAR(100) UNIQUE NOT NULL,
--[Password] NVARCHAR(100) NOT NULL,
--[RoleId] INT FOREIGN KEY REFERENCES Roles(ID) NOT NULL,
--[Salt] NVARCHAR(255) UNIQUE NOT NULL,
--)

--GO
--CREATE TABLE Contacts
--(
--[Id] INT PRIMARY KEY IDENTITY,
--[Name] NVARCHAR(100) NOT NULL,
--[Surname] NVARCHAR(100) NOT NULL,
--[Phone] NVARCHAR(255),
--[Email] NVARCHAR(255) NOT NULL,
--[IsFavorite] BIT
--)

 --GO
 --CREATE TABLE RefreshTokens
 --(
 --[Id] INT PRIMARY KEY IDENTITY,
 --[Token] NVARCHAR(255) NOT NULL,
 --[Expires] DATETIME NOT NULL,
 --[UserId] INT REFERENCES Users(Id) NOT NULL 
 --)


----STORED PROCEDURES



----Insert a new entity into the Contacts and retrieve its props
--GO
--CREATE PROC AddContact @Name NVARCHAR(100), @Surname NVARCHAR(100), @Phone NVARCHAR(max), @Email NVARCHAR(max), @IsFavorite BIT
--AS
--BEGIN
--INSERT INTO Contacts 
--OUTPUT inserted.Id, inserted.Name,inserted.Surname,inserted.Phone,inserted.Email,inserted.IsFavorite
--VALUES(@Name,@Surname,@Phone,@Email,@IsFavorite)
--END


----Retrieve each entity from the Contacts
--GO
--CREATE PROC GetAllContacts 
--AS
--BEGIN
--SELECT * FROM Contacts
--END

----Retrieve paginated entities from the Contacts
--GO
--CREATE PROC GetAllContactsPaginates  @Skip INT, @Take INT
--AS
--BEGIN
--SELECT * FROM Contacts
--ORDER BY Contacts.Id
--OFFSET @Skip ROWS
--FETCH NEXT @Take ROWS ONLY
--END

----Get Contacts Count
--GO
--CREATE PROC GetContactsCount
--AS
--BEGIN
--SELECT COUNT(*) FROM Contacts
--END


----Retrieve specific entity from the Contacts by its Id
--GO
--CREATE PROC GetSingleContact @Id INT 
--AS
--BEGIN
--SELECT * FROM Contacts WHERE Contacts.Id = @Id
--END


----Update a specific entity in the Contacts and retrieve its props
--GO
--CREATE PROC UpdateContact @Id INT, @Name NVARCHAR(100),@Surname NVARCHAR(100), @Phone NVARCHAR(max), @Email NVARCHAR(100), @IsFavorite BIT
--AS
--BEGIN
--UPDATE Contacts SET
--Contacts.Name = @Name, Contacts.Surname = @Surname, Contacts.Phone = @Phone, Contacts.Email = @Email, Contacts.IsFavorite = @IsFavorite
--OUTPUT inserted.Id, inserted.Name, inserted.Surname, inserted.Phone,inserted.Email,inserted.IsFavorite 
--WHERE Contacts.Id = @Id
--END


----Delete a specific entity from the Contacts by its Id
--GO
--CREATE PROC DeleteContact @Id INT
--AS
--BEGIN
--DELETE FROM Contacts WHERE Contacts.Id = @Id
--END




----Insert a new entity into Users and retrieve Id of the inserted entity
--GO
--CREATE PROC AddUser @Login NVARCHAR(100), @Password NVARCHAR(100),@RoleId INT, @Salt NVARCHAR(255)
--AS 
--BEGIN
--INSERT INTO Users (Login,Password,RoleId,Salt)
--OUTPUT inserted.Id
--VALUES (@Login,@Password,@RoleId,@Salt)
--END

----Retrieve each entity from the Users, joined with its role from the Roles
--GO
--CREATE PROC GetAllUsers
--AS
--BEGIN
--SELECT u.*, r.* FROM Users u
--JOIN Roles r on u.RoleId = r.Id
--END


----Retrieve a specific entity by its id from the Users, joined with its role from the Roles
--GO
--CREATE PROC GetSingleUser @Id INT
--AS
--BEGIN
--SELECT u.*, r.* FROM Users u
--JOIN Roles r on u.RoleId = r.Id
--WHERE u.Id = @Id
--END


----Retrieve a specific entity by its login from the Users, joined with its role from the Roles
--GO
--CREATE PROC  GetUserByLogin @Login NVARCHAR(100)
--AS
--BEGIN
--select u.*, r.* FROM Users as u
--JOIN Roles as r ON r.Id = u.RoleId 
--where u.Login = @Login COLLATE Latin1_General_CS_AS
--END




----Insert a new entity into RefreshTokens
-- GO 
-- CREATE PROC AddRefreshToken @Token NVARCHAR(255), @Expires DATETIME, @UserId INT
-- AS
-- BEGIN
-- INSERT INTO RefreshTokens VALUES(@Token,@Expires, @UserId)
-- END

----Retrieve a specific entity by its id from the RefreshTokens, joined with Users and their Roles
-- GO
-- CREATE PROC GetRefresTokenByToken @Token NVARCHAR(255)
-- AS
-- BEGIN
-- SELECT rt.*, us.*,rl.* FROM RefreshTokens AS rt
-- JOIN Users AS us ON us.Id = rt.UserId
-- JOIN Roles AS rl ON rl.Id = us.RoleId
-- WHERE rt.Token = @Token
-- END

----Remove a specific entity from RefreshTokens by Token
-- GO
-- CREATE PROC RemoveOldRefreshToken @Token NVARCHAR(255)
-- AS
-- BEGIN
-- DELETE FROM RefreshTokens WHERE RefreshTokens.Token = @Token
-- END

----Remove all User's RefreshTokens by their Id 
--GO
--CREATE PROC DeleteUserRefreshTokens @UserId INT
--AS
--BEGIN
--DELETE FROM RefreshTokens WHERE UserId = @UserId
--END

--Change a User Password by their Login 
--GO
--CREATE PROC  ChangeUserPassword @Login NVARCHAR(100), @Password NVARCHAR(100)
--AS
--BEGIN
--UPDATE [Users] SET [Users].Password = @Password WHERE
--[Users].Login= @Login COLLATE Latin1_General_CS_AS
--END



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

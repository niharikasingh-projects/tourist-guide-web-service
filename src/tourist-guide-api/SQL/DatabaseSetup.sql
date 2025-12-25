-- =============================================
-- Tourist Guide Database Setup Script
-- Description: Creates the TouristGuideDb database and all tables
-- =============================================

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TouristGuideDb')
BEGIN
    CREATE DATABASE TouristGuideDb;
    PRINT 'Database TouristGuideDb created successfully.';
END
ELSE
BEGIN
    PRINT 'Database TouristGuideDb already exists.';
END
GO

USE TouristGuideDb;
GO

-- Create Users Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'tourist',
        PhoneNumber NVARCHAR(15) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT CK_Users_Role CHECK (Role IN ('tourist', 'guide'))
    );
    PRINT 'Table Users created successfully.';
END
GO

ALTER TABLE [Users]
ADD [Location] NVARCHAR(100) NULL;
GO

ALTER TABLE [Users]
ADD [Languages] NVARCHAR(500) NULL;
GO

ALTER TABLE [Users]
ADD [Certifications] NVARCHAR(500) NULL;
GO

ALTER TABLE [Users]
ADD [ProfileImageUrl] NVARCHAR(500) NULL;
GO


-- Create TouristAttractions Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TouristAttractions')
BEGIN
    CREATE TABLE TouristAttractions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        Location NVARCHAR(200) NOT NULL,
        ImageUrl NVARCHAR(500) NULL,
        Category NVARCHAR(100) NULL,
        Rating DECIMAL(3, 2) NOT NULL DEFAULT 0,
        EntryFee DECIMAL(10, 2) NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT CK_TouristAttractions_Rating CHECK (Rating >= 0 AND Rating <= 5)
    );
    PRINT 'Table TouristAttractions created successfully.';
END
GO

-- Create GuideProfiles Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'GuideProfiles')
BEGIN
    CREATE TABLE GuideProfiles (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        AttractionId INT NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
        PhoneNumber NVARCHAR(15) NOT NULL,
        Experience INT NOT NULL,
        Languages NVARCHAR(MAX) NOT NULL,
        Bio NVARCHAR(500) NULL,
        Rating DECIMAL(3, 2) NOT NULL DEFAULT 0,
        PricePerHour DECIMAL(10, 2) NOT NULL,
        Availability NVARCHAR(MAX) NOT NULL,
        ProfileImageUrl NVARCHAR(500) NULL,
        IsAvailable BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_GuideProfiles_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_GuideProfiles_Attractions FOREIGN KEY (AttractionId) REFERENCES TouristAttractions(Id),
        CONSTRAINT UQ_GuideProfiles_UserAttraction UNIQUE (UserId, AttractionId),
        CONSTRAINT CK_GuideProfiles_Rating CHECK (Rating >= 0 AND Rating <= 5)
    );
    PRINT 'Table GuideProfiles created successfully.';
END
GO

-- Create Bookings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bookings')
BEGIN
    CREATE TABLE Bookings (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        GuideId INT NOT NULL,
        AttractionId INT NOT NULL,
        BookingDate DATETIME2 NOT NULL,
        TimeFrom NVARCHAR(10) NOT NULL,
        TimeTo NVARCHAR(10) NOT NULL,
        NumberOfPeople INT NOT NULL DEFAULT 1,
        TotalAmount DECIMAL(10, 2) NOT NULL,
        TaxAmount DECIMAL(10, 2) NOT NULL,
        GrandTotal DECIMAL(10, 2) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'pending',
        TouristName NVARCHAR(100) NOT NULL,
        TouristEmail NVARCHAR(150) NOT NULL,
        TouristPhone NVARCHAR(15) NOT NULL,
        SpecialRequests NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_Bookings_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_Bookings_Guides FOREIGN KEY (GuideId) REFERENCES GuideProfiles(Id),
        CONSTRAINT FK_Bookings_Attractions FOREIGN KEY (AttractionId) REFERENCES TouristAttractions(Id),
        CONSTRAINT CK_Bookings_Status CHECK (Status IN ('pending', 'confirmed', 'completed', 'cancelled'))
    );
    PRINT 'Table Bookings created successfully.';
END
GO

-- Create Payments Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        Id INT PRIMARY KEY IDENTITY(1,1),
        BookingId INT NOT NULL,
        Amount DECIMAL(10, 2) NOT NULL,
        PaymentMethod NVARCHAR(50) NOT NULL,
        TransactionId NVARCHAR(50) NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'pending',
        UpiId NVARCHAR(100) NULL,
        CardNumber NVARCHAR(20) NULL,
        CardHolderName NVARCHAR(100) NULL,
        PaymentDate DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Payments_Bookings FOREIGN KEY (BookingId) REFERENCES Bookings(Id) ON DELETE CASCADE,
        CONSTRAINT CK_Payments_Status CHECK (Status IN ('pending', 'completed', 'failed')),
        CONSTRAINT CK_Payments_Method CHECK (PaymentMethod IN ('UPI', 'CreditCard', 'PayLater'))
    );
    PRINT 'Table Payments created successfully.';
END
GO

-- Create Indexes for Better Performance
CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);
CREATE NONCLUSTERED INDEX IX_GuideProfiles_UserId ON GuideProfiles(UserId);
CREATE NONCLUSTERED INDEX IX_GuideProfiles_AttractionId ON GuideProfiles(AttractionId);
CREATE NONCLUSTERED INDEX IX_Bookings_UserId ON Bookings(UserId);
CREATE NONCLUSTERED INDEX IX_Bookings_GuideId ON Bookings(GuideId);
CREATE NONCLUSTERED INDEX IX_Bookings_BookingDate ON Bookings(BookingDate);
CREATE NONCLUSTERED INDEX IX_Payments_BookingId ON Payments(BookingId);
GO

PRINT '============================================='
PRINT 'Database setup completed successfully!'
PRINT '============================================='

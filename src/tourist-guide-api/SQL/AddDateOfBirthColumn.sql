-- =============================================
-- Add DateOfBirth Column to Users Table
-- Description: Adds DateOfBirth column to store user's date of birth
-- =============================================

USE TouristGuideDb;
GO

-- Check if column doesn't exist before adding
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE Name = N'DateOfBirth' 
    AND Object_ID = Object_ID(N'Users')
)
BEGIN
    -- Add DateOfBirth column
    ALTER TABLE Users
    ADD DateOfBirth DATETIME2 NULL;
    
    PRINT 'DateOfBirth column added successfully to Users table.';
END
ELSE
BEGIN
    PRINT 'DateOfBirth column already exists in Users table.';
END
GO

PRINT '============================================='
PRINT 'DateOfBirth column setup completed!'
PRINT '============================================='

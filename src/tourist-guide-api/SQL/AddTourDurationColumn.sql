-- =============================================
-- Add TourDuration Column to GuideProfiles Table
-- Description: Adds TourDuration column to store tour duration in hours
-- =============================================

USE TouristGuideDb;
GO

-- Check if column doesn't exist before adding
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE Name = N'TourDuration' 
    AND Object_ID = Object_ID(N'GuideProfiles')
)
BEGIN
    -- Add TourDuration column
    ALTER TABLE GuideProfiles
    ADD TourDuration INT NOT NULL DEFAULT 2;
    
    PRINT 'TourDuration column added successfully to GuideProfiles table.';
END
ELSE
BEGIN
    PRINT 'TourDuration column already exists in GuideProfiles table.';
END
GO

-- Update existing records with default tour duration of 2-3 hours based on guide experience
UPDATE GuideProfiles
SET TourDuration = CASE 
    WHEN Experience >= 10 THEN 3
    WHEN Experience >= 5 THEN 2
    ELSE 2
END
WHERE TourDuration = 2; -- Only update records with default value
GO

PRINT '============================================='
PRINT 'TourDuration column setup completed!'
PRINT 'Default values set based on guide experience.'
PRINT '============================================='

-- =============================================
-- Tourist Guide Database - Mock Data Insertion Script
-- Description: Inserts sample data for testing
-- =============================================

USE TouristGuideDb;
GO

-- Insert Users (Tourists and Guides)
SET IDENTITY_INSERT Users ON;

-- Tourists (passwords hashed with BCrypt for 'password123')
INSERT INTO Users (Id, Name, Email, PasswordHash, Role, PhoneNumber, CreatedAt) VALUES
(1, 'John Smith', 'john@example.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'tourist', '9876543210', GETUTCDATE()),
(2, 'Sarah Johnson', 'sarah@example.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'tourist', '9876543211', GETUTCDATE()),
(3, 'Mike Brown', 'mike@example.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'tourist', '9876543212', GETUTCDATE());

-- Guides (passwords hashed with BCrypt for 'password123')
INSERT INTO Users (Id, Name, Email, PasswordHash, Role, PhoneNumber, CreatedAt) VALUES
(4, 'Rajesh Kumar', 'rajesh@guide.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'guide', '9876543213', GETUTCDATE()),
(5, 'Priya Sharma', 'priya@guide.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'guide', '9876543214', GETUTCDATE()),
(6, 'Amit Patel', 'amit@guide.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'guide', '9876543215', GETUTCDATE()),
(7, 'Deepika Singh', 'deepika@guide.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'guide', '9876543216', GETUTCDATE()),
(8, 'Vikram Malhotra', 'vikram@guide.com', '$2a$11$eKGvZ8uh3.sVCQBVF3X0rO4mN8KmQZ5xLqF9YJXWZxzN5kP.jPXNW', 'guide', '9876543217', GETUTCDATE());

SET IDENTITY_INSERT Users OFF;
GO

-- Insert Tourist Attractions
SET IDENTITY_INSERT TouristAttractions ON;

INSERT INTO TouristAttractions (Id, Name, Description, Location, ImageUrl, Category, Rating, EntryFee, IsActive, CreatedAt) VALUES
(1, 'Taj Mahal', 'The iconic white marble mausoleum built by Mughal Emperor Shah Jahan in memory of his wife Mumtaz Mahal. A UNESCO World Heritage Site and one of the Seven Wonders of the World.', 'Agra', 'https://images.unsplash.com/photo-1564507592333-c60657eea523', 'Historical Monument', 4.8, 50.00, 1, GETUTCDATE()),
(2, 'Red Fort', 'A historic fort in the city of Delhi that served as the main residence of the Mughal Emperors. Built in 1648, it is a UNESCO World Heritage Site.', 'Delhi', 'https://images.unsplash.com/photo-1587135941948-670b381f08ce', 'Historical Monument', 4.5, 35.00, 1, GETUTCDATE()),
(3, 'Gateway of India', 'An arch-monument built during the 20th century in Mumbai. It was erected to commemorate the landing of King George V and Queen Mary at Apollo Bunder.', 'Mumbai', 'https://images.unsplash.com/photo-1570168007204-dfb528c6958f', 'Monument', 4.3, 0.00, 1, GETUTCDATE()),
(4, 'Hawa Mahal', 'The Palace of Winds is a palace in Jaipur, India. It is constructed of red and pink sandstone and is a prime example of Rajputana architecture.', 'Jaipur', 'https://images.unsplash.com/photo-1599661046289-e31897846e41', 'Palace', 4.6, 50.00, 1, GETUTCDATE()),
(5, 'Golden Temple', 'A Gurdwara located in the city of Amritsar, Punjab. It is the holiest Gurdwara and the most prominent center of worship for Sikhs worldwide.', 'Amritsar', 'https://images.unsplash.com/photo-1609416917060-5d8f51c8d39f', 'Religious Site', 4.9, 0.00, 1, GETUTCDATE()),
(6, 'Qutub Minar', 'A minaret and "victory tower" that forms part of the Qutb complex, a UNESCO World Heritage Site in the Mehrauli area of Delhi.', 'Delhi', 'https://images.unsplash.com/photo-1587474260584-136574528ed5', 'Historical Monument', 4.4, 30.00, 1, GETUTCDATE());

SET IDENTITY_INSERT TouristAttractions OFF;
GO

-- Insert Guide Profiles
SET IDENTITY_INSERT GuideProfiles ON;

INSERT INTO GuideProfiles (Id, UserId, AttractionId, FullName, Email, PhoneNumber, Experience, TourDuration, Languages, Bio, Rating, PricePerHour, ProfileImageUrl, IsAvailable, CreatedAt) VALUES
(1, 4, 1, 'Rajesh Kumar', 'rajesh@guide.com', '9876543213', 10, 3, 'English, Hindi, Urdu', 'Expert guide with 10 years of experience at Taj Mahal. Specialized in Mughal history and architecture.', 4.8, 500.00, 'https://randomuser.me/api/portraits/men/1.jpg', 1, GETUTCDATE()),
(2, 5, 1, 'Priya Sharma', 'priya@guide.com', '9876543214', 7, 2, 'English, Hindi', 'Passionate about Indian heritage with 7 years of guiding experience. Known for engaging storytelling.', 4.7, 450.00, 'https://randomuser.me/api/portraits/women/1.jpg', 1, GETUTCDATE()),
(3, 6, 2, 'Amit Patel', 'amit@guide.com', '9876543215', 12, 4, 'English, Hindi, French', 'Senior guide at Red Fort with extensive knowledge of Delhi history and Mughal dynasty.', 4.9, 550.00, 'https://randomuser.me/api/portraits/men/2.jpg', 1, GETUTCDATE()),
(4, 7, 3, 'Deepika Singh', 'deepika@guide.com', '9876543216', 5, 2, 'English, Hindi, Marathi', 'Energetic guide with deep knowledge of Mumbai landmarks and colonial history.', 4.5, 400.00, 'https://randomuser.me/api/portraits/women/2.jpg', 1, GETUTCDATE()),
(5, 8, 4, 'Vikram Malhotra', 'vikram@guide.com', '9876543217', 8, 3, 'English, Hindi, Rajasthani', 'Specialist in Rajasthani culture and architecture with 8 years of experience.', 4.6, 475.00, 'https://randomuser.me/api/portraits/men/3.jpg', 1, GETUTCDATE());

SET IDENTITY_INSERT GuideProfiles OFF;
GO

-- Insert Guide Available Dates
SET IDENTITY_INSERT GuideAvailableDates ON;

-- Rajesh Kumar - Available for next 60 days (Guide 1)
INSERT INTO GuideAvailableDates (Id, GuideProfileId, FromDate, ToDate, CreatedAt) VALUES
(1, 1, GETUTCDATE(), DATEADD(DAY, 60, GETUTCDATE()), GETUTCDATE());

-- Priya Sharma - Available for next 60 days (Guide 2)
INSERT INTO GuideAvailableDates (Id, GuideProfileId, FromDate, ToDate, CreatedAt) VALUES
(2, 2, GETUTCDATE(), DATEADD(DAY, 60, GETUTCDATE()), GETUTCDATE());

-- Amit Patel - Available for next 60 days (Guide 3)
INSERT INTO GuideAvailableDates (Id, GuideProfileId, FromDate, ToDate, CreatedAt) VALUES
(3, 3, GETUTCDATE(), DATEADD(DAY, 60, GETUTCDATE()), GETUTCDATE());

-- Deepika Singh - Available for next 60 days (Guide 4)
INSERT INTO GuideAvailableDates (Id, GuideProfileId, FromDate, ToDate, CreatedAt) VALUES
(4, 4, GETUTCDATE(), DATEADD(DAY, 60, GETUTCDATE()), GETUTCDATE());

-- Vikram Malhotra - Available for next 60 days (Guide 5)
INSERT INTO GuideAvailableDates (Id, GuideProfileId, FromDate, ToDate, CreatedAt) VALUES
(5, 5, GETUTCDATE(), DATEADD(DAY, 60, GETUTCDATE()), GETUTCDATE());

SET IDENTITY_INSERT GuideAvailableDates OFF;
GO

-- Insert Sample Bookings
SET IDENTITY_INSERT Bookings ON;

INSERT INTO Bookings (Id, UserId, GuideId, AttractionId, BookingDate, TimeFrom, TimeTo, NumberOfPeople, TotalAmount, TaxAmount, GrandTotal, Status, TouristName, TouristEmail, TouristPhone, SpecialRequests, CreatedAt) VALUES
(1, 1, 1, 1, DATEADD(DAY, 5, GETUTCDATE()), '10:00', '13:00', 2, 3000.00, 840.00, 3840.00, 'confirmed', 'John Smith', 'john@example.com', '9876543210', 'Need wheelchair accessibility', GETUTCDATE()),
(2, 2, 3, 2, DATEADD(DAY, 3, GETUTCDATE()), '09:00', '12:00', 1, 1650.00, 462.00, 2112.00, 'confirmed', 'Sarah Johnson', 'sarah@example.com', '9876543211', NULL, GETUTCDATE()),
(3, 1, 2, 1, DATEADD(DAY, -2, GETUTCDATE()), '14:00', '17:00', 3, 4050.00, 1134.00, 5184.00, 'completed', 'John Smith', 'john@example.com', '9876543210', 'Photography allowed?', GETUTCDATE()),
(4, 3, 4, 3, DATEADD(DAY, 1, GETUTCDATE()), '11:00', '14:00', 2, 2400.00, 672.00, 3072.00, 'pending', 'Mike Brown', 'mike@example.com', '9876543212', 'First time visitor', GETUTCDATE());

SET IDENTITY_INSERT Bookings OFF;
GO

-- Insert Sample Payments
SET IDENTITY_INSERT Payments ON;

INSERT INTO Payments (Id, BookingId, Amount, PaymentMethod, TransactionId, Status, UpiId, CardNumber, CardHolderName, PaymentDate, CreatedAt) VALUES
(1, 1, 3840.00, 'UPI', 'TXN1234567890', 'completed', 'john@upi', NULL, NULL, GETUTCDATE(), GETUTCDATE()),
(2, 2, 2112.00, 'CreditCard', 'TXN0987654321', 'completed', NULL, '****1234', 'Sarah Johnson', GETUTCDATE(), GETUTCDATE()),
(3, 3, 5184.00, 'CreditCard', 'TXN1122334455', 'completed', NULL, '****5678', 'John Smith', DATEADD(DAY, -2, GETUTCDATE()), GETUTCDATE()),
(4, 4, 3072.00, 'PayLater', NULL, 'pending', NULL, NULL, NULL, NULL, GETUTCDATE());

SET IDENTITY_INSERT Payments OFF;
GO

PRINT '============================================='
PRINT 'Mock data inserted successfully!'
PRINT 'Test Credentials:'
PRINT 'Tourist - Email: john@example.com, Password: password123'
PRINT 'Guide - Email: rajesh@guide.com, Password: password123'
PRINT '============================================='

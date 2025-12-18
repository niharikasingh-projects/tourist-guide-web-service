# Tourist Guide Web API

ASP.NET Core 9.0 Web API for Tourist Guide Application with SQL Server backend.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access (Tourist/Guide)
- **Tourist Attractions Management**: CRUD operations for attractions
- **Guide Profiles**: Guide registration and profile management
- **Booking System**: Time-based booking with availability checking
- **Payment Processing**: Support for UPI, Credit Card, and Pay Later options
- **Tax Calculation**: Automatic tax calculation (14% CGST + 14% SGST)

## Technology Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- SQL Server
- JWT Authentication
- BCrypt for password hashing
- Swagger/OpenAPI

## Prerequisites

- .NET SDK 9.0 or later
- SQL Server (LocalDB or Server)
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Database Setup

Run the SQL scripts in order:

```bash
# 1. Create database and tables
sqlcmd -S localhost -i SQL/DatabaseSetup.sql

# 2. Insert mock data
sqlcmd -S localhost -i SQL/SeedData.sql
```

Alternatively, use SQL Server Management Studio to execute the scripts.

### 2. Configuration

Update `appsettings.json` with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TouristGuideDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Build and Run

```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the API
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000
- Swagger UI: https://localhost:5001/swagger

## API Endpoints

### Authentication
- `POST /api/auth/signup` - Register new user
- `POST /api/auth/signin` - User login

### Attractions
- `GET /api/attractions` - Get all attractions
- `GET /api/attractions/{id}` - Get attraction by ID
- `GET /api/attractions/search?location={location}` - Search attractions
- `POST /api/attractions` - Create attraction (Guide only)
- `PUT /api/attractions/{id}` - Update attraction (Guide only)
- `DELETE /api/attractions/{id}` - Delete attraction (Guide only)

### Guides
- `GET /api/guides/attraction/{attractionId}` - Get guides by attraction
- `GET /api/guides/{id}` - Get guide by ID
- `GET /api/guides/profile` - Get current guide's profile (Guide only)
- `POST /api/guides/profile` - Create guide profile (Guide only)
- `PUT /api/guides/profile` - Update guide profile (Guide only)
- `GET /api/guides/{id}/availability` - Check guide availability

### Bookings
- `POST /api/bookings` - Create booking (Tourist only)
- `GET /api/bookings/{id}` - Get booking by ID
- `GET /api/bookings/my-bookings` - Get current user's bookings (Tourist only)
- `GET /api/bookings/guide-bookings?guideId={id}` - Get guide's bookings (Guide only)
- `GET /api/bookings/guide-bookings/categorize?guideId={id}&selectedDate={date}` - Categorize guide bookings
- `PATCH /api/bookings/{id}/status` - Update booking status

### Payments
- `POST /api/payments/process` - Process payment
- `GET /api/payments/booking/{bookingId}` - Get payment by booking ID

## Test Credentials

### Tourist Account
- Email: `john@example.com`
- Password: `password123`

### Guide Account
- Email: `rajesh@guide.com`
- Password: `password123`

### Test Credit Card
- Card Number: `1111 1111 1111 1111`
- Any expiry date and CVV

## Database Schema

### Tables
1. **Users** - User accounts (tourists and guides)
2. **TouristAttractions** - Tourist attraction information
3. **GuideProfiles** - Guide profile details linked to attractions
4. **Bookings** - Booking records with time slots
5. **Payments** - Payment transaction records

## CORS Configuration

The API is configured to accept requests from:
- Angular Frontend: `http://localhost:4200`

Update `Program.cs` to add additional origins if needed.

## Entity Framework Migrations

To create migrations for database changes:

```bash
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## Business Logic

### Tax Calculation
- CGST: 14%
- SGST: 14%
- Total Tax: 28%

### Booking Time Slots
- Available hours: 9:00 AM to 6:00 PM (18:00)
- Format: 24-hour time (e.g., "09:00", "14:00")

### Booking Status Flow
1. `pending` - Initial booking created
2. `confirmed` - Payment completed or confirmed by guide
3. `completed` - Service completed
4. `cancelled` - Booking cancelled

### Payment Methods
1. **UPI** - Instant payment with UPI ID
2. **CreditCard** - Card payment with validation
3. **PayLater** - Deferred payment option

## Project Structure

```
TouristGuide.Api/
├── Controllers/        # API Controllers
├── Data/              # DbContext and configurations
├── DTOs/              # Data Transfer Objects
├── Models/            # Entity models
├── Services/          # Business logic services
├── SQL/               # Database scripts
├── Program.cs         # Application entry point
└── appsettings.json   # Configuration
```

## Security

- Passwords are hashed using BCrypt
- JWT tokens expire after 7 days
- Role-based authorization on sensitive endpoints
- SQL injection protection via Entity Framework parameterization

## Notes

- The connection string uses `TrustServerCertificate=True` for local development
- For production, configure proper SSL certificates
- Update the JWT secret key in production
- Consider implementing rate limiting for production use

## License

This project is for educational purposes.

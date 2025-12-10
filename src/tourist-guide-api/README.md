# Tourist Guide API - .NET 8 Backend

This is the backend API service for the Tourist Guide Web Application. It provides RESTful endpoints for managing tourist attractions, tour guides, bookings, user authentication, and user profiles.

## Technology Stack

- **Framework**: ASP.NET Core Web API (.NET 8)
- **Database**: SQLite with Entity Framework Core 8
- **Documentation**: Swagger/OpenAPI
- **CORS**: Configured for Angular frontend (localhost:4200)

## Project Structure

```
tourist-guide-api/
├── Controllers/          # API Controllers
│   ├── AttractionsController.cs
│   ├── GuidesController.cs
│   ├── BookingsController.cs
│   ├── AuthController.cs
│   └── ProfileController.cs
├── Models/              # Entity Models
│   ├── TouristAttraction.cs
│   ├── Guide.cs
│   ├── Booking.cs
│   ├── User.cs
│   └── UserProfile.cs
├── DTOs/                # Data Transfer Objects
│   ├── AuthDtos.cs
│   ├── BookingDtos.cs
│   └── GuideDtos.cs
├── Data/                # Database Context and Initializer
│   ├── TouristGuideDbContext.cs
│   └── DbInitializer.cs
├── Services/            # Business Logic Layer
│   ├── IAttractionService.cs & AttractionService.cs
│   ├── IGuideService.cs & GuideService.cs
│   ├── IBookingService.cs & BookingService.cs
│   ├── IAuthService.cs & AuthService.cs
│   └── IProfileService.cs & ProfileService.cs
├── Program.cs           # Application Entry Point
├── appsettings.json     # Configuration
└── TouristGuide.API.csproj
```

## Prerequisites

- .NET 8 SDK or later ([Download](https://dotnet.microsoft.com/download))
- Visual Studio Code, Visual Studio 2022, or any C# IDE

## Getting Started

### 1. Install Dependencies

```bash
cd d:\Project\TouristGuide\tourist-guide-api
dotnet restore
```

### 2. Run the Application

```bash
dotnet run
```

The API will start on:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### 3. View API Documentation

Once the application is running, open Swagger UI:
```
http://localhost:5000/swagger
```

## Database

The application uses **SQLite** with automatic database initialization on startup.

- **Database File**: `touristguide.db` (created automatically in the project root)
- **Seed Data**: Includes 20+ tourist attractions and 7 sample guides
- **Schema**: 5 tables (TouristAttractions, Guides, Bookings, Users, UserProfiles)

### Database Structure

**TouristAttractions**
- Id, Name, Location, City, Country, Description, ImageUrl, Category, Rating

**Guides**
- Id, Name, ImageUrl, Languages (CSV), Availability, HourlyRate, Rating, ExperienceYears, Specialties (CSV), Bio, AvailableDates (JSON), Contact, Email, PhoneNumber, AttractionIds (CSV)

**Bookings**
- Id, AttractionId, AttractionName, GuideId, GuideName, GuideContact, GuideEmail, CustomerName, CustomerContact, CustomerEmail, Amount, BookingDate, Status, FromDate, ToDate

**Users**
- Id, Email, Password, Name, CreatedAt

**UserProfiles**
- Id, Email, Name, DateOfBirth, PhoneNumber, Country

## API Endpoints

### Attractions

- `GET /api/attractions` - Get all attractions
- `GET /api/attractions/{id}` - Get attraction by ID
- `GET /api/attractions/search?location={location}&fromDate={date}&toDate={date}` - Search attractions

### Guides

- `GET /api/guides/by-attraction/{attractionId}?fromDate={date}&toDate={date}` - Get guides for an attraction
- `GET /api/guides/{guideId}` - Get guide by ID

### Bookings

- `POST /api/bookings` - Create a new booking
- `GET /api/bookings/{id}` - Get booking by ID
- `GET /api/bookings/by-customer/{email}` - Get all bookings for a customer
- `PUT /api/bookings/{id}/cancel` - Cancel a booking

### Authentication

- `POST /api/auth/signin` - User sign-in
- `POST /api/auth/signup` - User registration

### Profile

- `GET /api/profile/{email}` - Get user profile
- `POST /api/profile` - Create/Update user profile
- `DELETE /api/profile/{email}` - Delete user profile

## Sample API Requests

### Create Booking
```json
POST /api/bookings
Content-Type: application/json

{
  "attractionId": "pa-1",
  "attractionName": "Eiffel Tower Tour",
  "guideId": "g-pa-1",
  "guideName": "Pierre Dubois",
  "guideContact": "+33 1 42 86 93 50",
  "guideEmail": "pierre.dubois@parisguides.fr",
  "customerName": "John Doe",
  "customerContact": "+1 555-1234",
  "customerEmail": "john.doe@example.com",
  "amount": 105.00,
  "fromDate": "2025-12-15",
  "toDate": "2025-12-15"
}
```

### Sign Up
```json
POST /api/auth/signup
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123",
  "name": "John Doe"
}
```

## Configuration

### CORS Policy

The API is configured to accept requests from the Angular frontend:
- **Allowed Origin**: `http://localhost:4200`
- **Allowed Methods**: GET, POST, PUT, DELETE
- **Allowed Headers**: All headers

To change the allowed origin, update `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Change this
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### Connection String

The SQLite connection string is in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=touristguide.db"
  }
}
```

## Development

### Build the Project

```bash
dotnet build
```

### Run Tests (if added)

```bash
dotnet test
```

### Clean Build Artifacts

```bash
dotnet clean
```

## Security Notes

⚠️ **IMPORTANT**: This is a development version with basic authentication.

**For Production, you MUST**:
1. Hash passwords using BCrypt or ASP.NET Core Identity
2. Implement JWT tokens for authentication
3. Add input validation and sanitization
4. Enable HTTPS only
5. Configure proper CORS origins
6. Add rate limiting
7. Implement proper logging and monitoring

## Troubleshooting

### Port Already in Use

If port 5000 is already in use, change it in `Program.cs` or use:

```bash
dotnet run --urls="http://localhost:5050"
```

### Database Issues

Delete the database file and restart the application:

```bash
rm touristguide.db
dotnet run
```

The database will be recreated with seed data.

### CORS Errors

Ensure the Angular app is running on `http://localhost:4200` or update the CORS policy in `Program.cs`.

## Integration with Angular Frontend

The Angular frontend expects the API to run on `http://localhost:5000`. This is configured in:

```typescript
// src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};
```

### Running Both Applications

1. Start the backend API (Terminal 1):
```bash
cd d:\Project\TouristGuide\tourist-guide-api
dotnet run
```

2. Start the Angular frontend (Terminal 2):
```bash
cd d:\Project\TouristGuide\tourist-guide-web-app
npm start
```

The Angular app will be available at `http://localhost:4200` and will communicate with the API at `http://localhost:5000`.

## License

This project is part of the Tourist Guide Web Application.

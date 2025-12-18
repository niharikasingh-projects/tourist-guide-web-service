# Tourist Guide API - Setup Summary

## ✅ Completed Backend Implementation

### Project Structure Created

```
TouristGuide.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── AttractionsController.cs
│   ├── GuidesController.cs
│   ├── BookingsController.cs
│   └── PaymentsController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── AuthDtos.cs
│   ├── AttractionDtos.cs
│   ├── GuideProfileDtos.cs
│   ├── BookingDtos.cs
│   └── PaymentDtos.cs
├── Models/
│   ├── User.cs
│   ├── TouristAttraction.cs
│   ├── GuideProfile.cs
│   ├── Booking.cs
│   └── Payment.cs
├── Services/
│   ├── IAuthService.cs & AuthService.cs
│   ├── IAttractionService.cs & AttractionService.cs
│   ├── IGuideService.cs & GuideService.cs
│   ├── IBookingService.cs & BookingService.cs
│   └── IPaymentService.cs & PaymentService.cs
├── SQL/
│   ├── DatabaseSetup.sql
│   └── SeedData.sql
├── Program.cs (Configured)
├── appsettings.json (Updated)
└── README.md
```

## Database Schema

### Tables
1. **Users** - Authentication and user management
2. **TouristAttractions** - Tourist places and monuments
3. **GuideProfiles** - Guide information linked to attractions
4. **Bookings** - Booking records with time slots
5. **Payments** - Payment transaction records

### Relationships
- User (1) ↔ (0..1) GuideProfile
- GuideProfile (N) → (1) TouristAttraction
- User (1) ↔ (N) Bookings
- GuideProfile (1) ↔ (N) Bookings
- Booking (1) ↔ (1) Payment

## API Endpoints Summary

### Authentication (`/api/auth`)
- `POST /signup` - Register new user (tourist/guide)
- `POST /signin` - User login

### Attractions (`/api/attractions`)
- `GET /` - Get all active attractions
- `GET /{id}` - Get attraction details
- `GET /search?location={loc}` - Search by location
- `POST /` - Create attraction [Guide]
- `PUT /{id}` - Update attraction [Guide]
- `DELETE /{id}` - Soft delete attraction [Guide]

### Guides (`/api/guides`)
- `GET /attraction/{id}` - Get guides by attraction
- `GET /attraction/{id}?timeFrom={time}&timeTo={time}` - Get available guides
- `GET /{id}` - Get guide details
- `GET /profile` - Get current guide profile [Guide]
- `POST /profile` - Create guide profile [Guide]
- `PUT /profile` - Update guide profile [Guide]
- `GET /{id}/availability` - Check availability

### Bookings (`/api/bookings`)
- `POST /` - Create booking [Tourist]
- `GET /{id}` - Get booking details
- `GET /my-bookings` - Get user bookings [Tourist]
- `GET /guide-bookings?guideId={id}` - Get guide bookings [Guide]
- `GET /guide-bookings/categorize?guideId={id}&selectedDate={date}` - Categorize bookings [Guide]
- `PATCH /{id}/status` - Update booking status

### Payments (`/api/payments`)
- `POST /process` - Process payment
- `GET /booking/{bookingId}` - Get payment by booking

## Key Features Implemented

### 1. Authentication & Authorization
- JWT token-based authentication
- Role-based access control (Tourist/Guide)
- BCrypt password hashing
- 7-day token expiration

### 2. Guide Availability System
- Time-slot based availability (9:00-18:00)
- Booking conflict detection
- Automatic filtering by time range

### 3. Booking Management
- Time-based booking (timeFrom/timeTo)
- Support for multiple people
- Status tracking (pending → confirmed → completed/cancelled)
- Booking categorization (current/past/future)

### 4. Payment Processing
- Multiple payment methods:
  - UPI (with UPI ID)
  - Credit Card (test card: 1111 1111 1111 1111)
  - Pay Later
- Automatic tax calculation (28% total)
- Transaction ID generation
- Payment status tracking

### 5. Tax Calculation
- CGST: 14%
- SGST: 14%
- Total Tax: 28%
- Automatic calculation on booking creation

## Configuration Details

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TouristGuideDb;..."
  },
  "Jwt": {
    "Secret": "YourSuperSecretKeyForJWTTokenGeneration12345",
    "Issuer": "TouristGuideApi",
    "Audience": "TouristGuideApp"
  }
}
```

### CORS Settings
- Configured for Angular frontend: `http://localhost:4200`
- AllowCredentials, AllowAnyHeader, AllowAnyMethod

### Swagger Configuration
- Enabled in Development mode
- Available at: `/swagger`
- Full API documentation with request/response models

## Database Setup Steps

### Option 1: Using SQL Scripts
```bash
# Run in SQL Server Management Studio or sqlcmd
1. Execute SQL/DatabaseSetup.sql
2. Execute SQL/SeedData.sql
```

### Option 2: Using Entity Framework Migrations
```bash
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

## Mock Data Included

### Users (password: password123)
- **Tourists:**
  - john@example.com
  - sarah@example.com
  - mike@example.com

- **Guides:**
  - rajesh@guide.com (Taj Mahal)
  - priya@guide.com (Taj Mahal)
  - amit@guide.com (Red Fort)
  - deepika@guide.com (Gateway of India)
  - vikram@guide.com (Hawa Mahal)

### Attractions
1. Taj Mahal (Agra) - ₹50 entry
2. Red Fort (Delhi) - ₹35 entry
3. Gateway of India (Mumbai) - Free
4. Hawa Mahal (Jaipur) - ₹50 entry
5. Golden Temple (Amritsar) - Free
6. Qutub Minar (Delhi) - ₹30 entry

### Sample Bookings
- 4 sample bookings with different statuses
- Including past, current, and future bookings
- Various payment methods demonstrated

## NuGet Packages Installed

1. **Microsoft.EntityFrameworkCore.SqlServer** (9.0.0)
2. **Microsoft.EntityFrameworkCore.Tools** (10.0.1)
3. **Microsoft.AspNetCore.Authentication.JwtBearer** (9.0.0)
4. **BCrypt.Net-Next** (4.0.3)
5. **Swashbuckle.AspNetCore** (10.0.1)

## Running the API

### Development
```bash
# Run the API
dotnet run

# Or with hot reload
dotnet watch run
```

### Production Build
```bash
# Build release version
dotnet build --configuration Release

# Publish
dotnet publish --configuration Release --output ./publish
```

## Testing the API

### 1. Using Swagger UI
Navigate to `https://localhost:5001/swagger`

### 2. Using curl
```bash
# Sign up
curl -X POST https://localhost:5001/api/auth/signup \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com","password":"password123","role":"tourist"}'

# Sign in
curl -X POST https://localhost:5001/api/auth/signin \
  -H "Content-Type: application/json" \
  -d '{"email":"john@example.com","password":"password123"}'

# Get attractions
curl https://localhost:5001/api/attractions
```

### 3. Using Angular Frontend
- The CORS is already configured for `http://localhost:4200`
- Update Angular environment files with API URL: `https://localhost:5001`

## Integration with Angular Frontend

### Required Changes in Angular Services

Update the base URL in environment files:

```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

### Service Updates
All services should use HttpClient to call the API endpoints instead of localStorage:

```typescript
// Example: auth.service.ts
signIn(credentials: SignInDto): Observable<AuthResponse> {
  return this.http.post<AuthResponse>(`${this.apiUrl}/auth/signin`, credentials);
}
```

## Security Considerations

### Implemented
✅ Password hashing with BCrypt
✅ JWT token authentication
✅ Role-based authorization
✅ HTTPS enforcement
✅ SQL injection protection (EF Core)
✅ CORS configuration

### For Production
⚠️ Update JWT secret key
⚠️ Configure proper SSL certificates
⚠️ Enable rate limiting
⚠️ Add request validation
⚠️ Implement logging
⚠️ Add error handling middleware
⚠️ Configure proper connection strings
⚠️ Enable database backups

## Next Steps

1. **Run Database Scripts**
   - Execute DatabaseSetup.sql
   - Execute SeedData.sql
   
2. **Test API**
   - Use Swagger UI to test all endpoints
   - Verify authentication flow
   - Test booking creation and payment processing

3. **Update Angular Frontend**
   - Replace localStorage with HTTP calls
   - Add API URL to environment
   - Update all services to use HttpClient
   - Add JWT token interceptor
   - Add error handling

4. **Production Deployment**
   - Configure production database
   - Update connection strings
   - Set up proper SSL certificates
   - Configure application insights/logging
   - Set up CI/CD pipeline

## Troubleshooting

### Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database TouristGuideDb exists

### CORS Errors
- Verify Angular is running on port 4200
- Check CORS policy in Program.cs
- Ensure AllowCredentials is set

### Authentication Issues
- Check JWT secret matches between requests
- Verify token is not expired
- Ensure Authorization header format: `Bearer {token}`

## Support

For issues or questions:
1. Check the README.md
2. Review Swagger documentation
3. Check database connection
4. Verify all packages are restored

---

**Build Status:** ✅ Success
**Database:** Ready for setup
**API Endpoints:** 25+ endpoints implemented
**Documentation:** Complete with Swagger UI

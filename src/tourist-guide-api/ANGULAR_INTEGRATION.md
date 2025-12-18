# Angular Frontend Integration Guide

## Quick Reference for Updating Angular Services

### 1. Environment Configuration

Create/Update environment files:

**src/environments/environment.ts**
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

**src/environments/environment.prod.ts**
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api.com/api'
};
```

### 2. Create HTTP Interceptor for JWT

**src/app/interceptors/auth.interceptor.ts**
```typescript
import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('auth_token');
  
  if (token) {
    const cloned = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(cloned);
  }
  
  return next(req);
};
```

**app.config.ts**
```typescript
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    // ... other providers
  ]
};
```

### 3. Update Services

#### AuthService

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../environments/environment';

export interface SignUpDto {
  name: string;
  email: string;
  password: string;
  role: string;
  phoneNumber?: string;
}

export interface SignInDto {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: number;
    name: string;
    email: string;
    role: string;
    phoneNumber?: string;
  };
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  signUp(data: SignUpDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/signup`, data).pipe(
      tap(response => {
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('current_user', JSON.stringify(response.user));
      })
    );
  }

  signIn(data: SignInDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/signin`, data).pipe(
      tap(response => {
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('current_user', JSON.stringify(response.user));
      })
    );
  }

  signOut(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('current_user');
  }

  getCurrentUser() {
    const user = localStorage.getItem('current_user');
    return user ? JSON.parse(user) : null;
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('auth_token');
  }
}
```

#### AttractionService

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Attraction {
  id: number;
  name: string;
  description: string;
  location: string;
  imageUrl?: string;
  category?: string;
  rating: number;
  entryFee: number;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class AttractionService {
  private apiUrl = `${environment.apiUrl}/attractions`;

  constructor(private http: HttpClient) {}

  getAllAttractions(): Observable<Attraction[]> {
    return this.http.get<Attraction[]>(this.apiUrl);
  }

  getAttractionById(id: number): Observable<Attraction> {
    return this.http.get<Attraction>(`${this.apiUrl}/${id}`);
  }

  searchAttractions(location: string): Observable<Attraction[]> {
    return this.http.get<Attraction[]>(`${this.apiUrl}/search?location=${location}`);
  }

  createAttraction(data: any): Observable<Attraction> {
    return this.http.post<Attraction>(this.apiUrl, data);
  }

  updateAttraction(id: number, data: any): Observable<Attraction> {
    return this.http.put<Attraction>(`${this.apiUrl}/${id}`, data);
  }

  deleteAttraction(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

#### GuideService

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface GuideProfile {
  id: number;
  userId: number;
  attractionId: number;
  attractionName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  experience: number;
  languages: string;
  bio?: string;
  rating: number;
  pricePerHour: number;
  availability: string;
  profileImageUrl?: string;
  isAvailable: boolean;
}

@Injectable({ providedIn: 'root' })
export class GuideService {
  private apiUrl = `${environment.apiUrl}/guides`;

  constructor(private http: HttpClient) {}

  getGuidesByAttraction(
    attractionId: number,
    timeFrom?: string,
    timeTo?: string
  ): Observable<GuideProfile[]> {
    let params = new HttpParams();
    if (timeFrom) params = params.set('timeFrom', timeFrom);
    if (timeTo) params = params.set('timeTo', timeTo);

    return this.http.get<GuideProfile[]>(
      `${this.apiUrl}/attraction/${attractionId}`,
      { params }
    );
  }

  getGuideById(id: number): Observable<GuideProfile> {
    return this.http.get<GuideProfile>(`${this.apiUrl}/${id}`);
  }

  getMyProfile(): Observable<GuideProfile> {
    return this.http.get<GuideProfile>(`${this.apiUrl}/profile`);
  }

  createProfile(data: any): Observable<GuideProfile> {
    return this.http.post<GuideProfile>(`${this.apiUrl}/profile`, data);
  }

  updateProfile(data: any): Observable<GuideProfile> {
    return this.http.put<GuideProfile>(`${this.apiUrl}/profile`, data);
  }

  checkAvailability(
    guideId: number,
    date: Date,
    timeFrom: string,
    timeTo: string
  ): Observable<{ isAvailable: boolean }> {
    const params = new HttpParams()
      .set('date', date.toISOString())
      .set('timeFrom', timeFrom)
      .set('timeTo', timeTo);

    return this.http.get<{ isAvailable: boolean }>(
      `${this.apiUrl}/${guideId}/availability`,
      { params }
    );
  }
}
```

#### BookingService

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Booking {
  id: number;
  userId: number;
  guideId: number;
  attractionId: number;
  attractionName: string;
  guideName: string;
  bookingDate: Date;
  timeFrom: string;
  timeTo: string;
  numberOfPeople: number;
  totalAmount: number;
  taxAmount: number;
  grandTotal: number;
  status: string;
  touristName: string;
  touristEmail: string;
  touristPhone: string;
  specialRequests?: string;
  createdAt: Date;
}

export interface CreateBookingDto {
  guideId: number;
  attractionId: number;
  bookingDate: Date;
  timeFrom: string;
  timeTo: string;
  numberOfPeople: number;
  touristName: string;
  touristEmail: string;
  touristPhone: string;
  specialRequests?: string;
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private apiUrl = `${environment.apiUrl}/bookings`;

  constructor(private http: HttpClient) {}

  createBooking(data: CreateBookingDto): Observable<Booking> {
    return this.http.post<Booking>(this.apiUrl, data);
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/${id}`);
  }

  getMyBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/my-bookings`);
  }

  getGuideBookings(guideId: number): Observable<Booking[]> {
    const params = new HttpParams().set('guideId', guideId.toString());
    return this.http.get<Booking[]>(`${this.apiUrl}/guide-bookings`, { params });
  }

  categorizeGuideBookings(guideId: number, selectedDate: Date): Observable<{
    current: Booking[];
    past: Booking[];
    future: Booking[];
  }> {
    const params = new HttpParams()
      .set('guideId', guideId.toString())
      .set('selectedDate', selectedDate.toISOString());

    return this.http.get<any>(`${this.apiUrl}/guide-bookings/categorize`, { params });
  }

  updateBookingStatus(id: number, status: string): Observable<Booking> {
    return this.http.patch<Booking>(`${this.apiUrl}/${id}/status`, { status });
  }
}
```

#### PaymentService

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Payment {
  id: number;
  bookingId: number;
  amount: number;
  paymentMethod: string;
  transactionId?: string;
  status: string;
  paymentDate?: Date;
}

export interface ProcessPaymentDto {
  bookingId: number;
  paymentMethod: string;
  upiId?: string;
  cardNumber?: string;
  cardHolderName?: string;
  expiryDate?: string;
  cvv?: string;
}

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private apiUrl = `${environment.apiUrl}/payments`;

  constructor(private http: HttpClient) {}

  processPayment(data: ProcessPaymentDto): Observable<Payment> {
    return this.http.post<Payment>(`${this.apiUrl}/process`, data);
  }

  getPaymentByBookingId(bookingId: number): Observable<Payment> {
    return this.http.get<Payment>(`${this.apiUrl}/booking/${bookingId}`);
  }
}
```

### 4. Create Auth Guard

**src/app/guards/auth.guard.ts**
```typescript
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  router.navigate(['/signin']);
  return false;
};
```

### 5. Update Routing

```typescript
import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'signin', component: SignInComponent },
  { path: 'signup', component: SignUpComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: 'my-bookings',
    component: MyBookingsComponent,
    canActivate: [authGuard]
  },
  // ... other routes
];
```

### 6. Migration Checklist

- [ ] Install HttpClient in app.config.ts
- [ ] Create environment files with API URL
- [ ] Create auth interceptor
- [ ] Update all services to use HttpClient
- [ ] Remove localStorage mock data logic
- [ ] Create auth guard
- [ ] Update routing with guards
- [ ] Test authentication flow
- [ ] Test all API endpoints
- [ ] Handle error responses
- [ ] Add loading indicators
- [ ] Update error messages

### 7. Error Handling Example

```typescript
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

// In your service
createBooking(data: CreateBookingDto): Observable<Booking> {
  return this.http.post<Booking>(this.apiUrl, data).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An error occurred';
      
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.status === 0) {
        errorMessage = 'Unable to connect to server';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized. Please log in again.';
      } else if (error.status === 403) {
        errorMessage = 'You do not have permission to perform this action';
      }
      
      return throwError(() => new Error(errorMessage));
    })
  );
}

// In your component
this.bookingService.createBooking(data).subscribe({
  next: (booking) => {
    // Success
    this.router.navigate(['/payment', booking.id]);
  },
  error: (error) => {
    // Handle error
    this.errorMessage = error.message;
  }
});
```

### 8. Testing Locally

1. **Start the .NET API:**
   ```bash
   cd d:\Project\TouristGuide\tourist-guide-api
   dotnet run
   ```

2. **Start Angular:**
   ```bash
   cd d:\Project\TouristGuide\tourist-guide
   ng serve
   ```

3. **Access:**
   - Frontend: http://localhost:4200
   - API: https://localhost:5001
   - Swagger: https://localhost:5001/swagger

### 9. Common Issues & Solutions

**CORS Error:**
- Ensure API is running on https://localhost:5001
- Verify CORS policy in Program.cs includes http://localhost:4200

**401 Unauthorized:**
- Check token is being sent in Authorization header
- Verify token is not expired
- Ensure auth interceptor is registered

**404 Not Found:**
- Verify API URL in environment.ts
- Check endpoint paths match API routes
- Ensure API is running

**SSL Certificate Error:**
- Trust the development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```

---

This guide covers all the essential steps to integrate your Angular frontend with the .NET Web API backend!

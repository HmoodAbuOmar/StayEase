# StayEasy-HotelMangmentSystem 🏨

A hotel management system built with ASP.NET Core Web API using **N-Tier Architecture**.  
The project supports hotel and room management, reservations, reviews, authentication, and checkout flow with clear separation between **Admin** and **User** operations.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Roles](#roles)
- [Implemented Modules](#implemented-modules)
- [API Endpoints](#api-endpoints)
- [Getting Started](#getting-started)
- [Configuration](#configuration)

---

## Overview

**StayEasy-HotelMangmentSystem** is a backend API for managing hotel operations.
It provides:

- User registration and login
- Email confirmation and password recovery
- Hotel and room management
- Room type management
- Reservation creation and update flow
- Hotel reviews
- Checkout endpoints for payment flow
- In-memory caching using `IMemoryCache`
- Role-based access for **Admin** and **User**

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend Framework | ASP.NET Core Web API |
| Architecture | N-Tier Architecture (DAL / BLL / PL) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | ASP.NET Identity + JWT + Refresh Token |
| Caching | IMemoryCache |
| Object Mapping | Mapster |
| API Documentation | Swagger / OpenAPI |

---

## Architecture

```text
StayEasy.DAL   → Models, DbContext, Repositories, Migrations
StayEasy.BLL   → Services, DTOs, Business Logic
StayEasy.PL    → Controllers, Program.cs, API Configuration
```

---

## Roles

```text
Admin → manages hotels, rooms, room types, reservations, and hotel reviews view
User  → browses hotels and rooms, creates reservations, writes reviews, and performs checkout
```

---

## Implemented Modules

### 🔐 Account & Identity
- User registration
- User login
- Email confirmation
- Forget password
- Reset password
- Refresh token

### 🏨 Hotels
- Users can browse available hotels
- Admin can create, update, delete, and retrieve hotels

### 🚪 Rooms
- Users can browse rooms
- Admin can create, update, delete, and retrieve rooms
- Admin can search room details by room number

### 🛏️ Room Types
- Admin can manage room types

### 📅 Reservations
- Users can create reservations
- Users can view, update, and cancel their reservations
- Admin can view all reservations and reservation details
- Admin can confirm or cancel reservations

### ⭐ Reviews
- Users can create, update, and delete hotel reviews
- Admin can view reviews for a specific hotel

### 💳 Checkouts
- User checkout endpoint
- Success callback endpoint
- Cancel callback endpoint

### ⚡ Caching
- `IMemoryCache` is used to improve performance for frequently requested data

---

## API Endpoints

### Account
```http
POST   /api/Identity/Account/Register
POST   /api/Identity/Account/Login
GET    /api/Identity/Account/ConfirmEmail
POST   /api/Identity/Account/ForgetPassword
PATCH  /api/Identity/Account/ResetPassword
PATCH  /api/Identity/Account/RefreshToken
```

### Checkouts
```http
POST   /api/User/Checkouts
GET    /api/User/Checkouts/success
GET    /api/User/Checkouts/cancel
```

### Hotels
```http
GET    /api/user/Hotels
GET    /api/Admin/Hotels
POST   /api/Admin/Hotels
GET    /api/Admin/Hotels/{id}
PATCH  /api/Admin/Hotels/{id}
DELETE /api/Admin/Hotels/{id}
```

### Reservations
```http
POST   /api/user/Reservations
GET    /api/user/Reservations
DELETE /api/user/Reservations/{id}
PATCH  /api/user/Reservations/{id}
GET    /api/Admin/Reservations
GET    /api/Admin/Reservations/{id}
PATCH  /api/Admin/Reservations/Confirm/{id}
PATCH  /api/Admin/Reservations/Cancel/{id}
```

### Reviews
```http
POST   /api/User/Reviews/hotel/{hotelId}
PUT    /api/User/Reviews/{reviewId}
DELETE /api/User/Reviews/{reviewId}
GET    /api/Admin/Reviews/hotel/{hotelId}
```

### Rooms
```http
GET    /api/user/Rooms
GET    /api/Admin/Rooms
POST   /api/Admin/Rooms
GET    /api/Admin/Rooms/{id}
PUT    /api/Admin/Rooms/{id}
DELETE /api/Admin/Rooms/{id}
GET    /api/Admin/Rooms/RoomNumber/{roomNumber}
```

### RoomTypes
```http
GET    /api/admin/RoomTypes
POST   /api/admin/RoomTypes
GET    /api/admin/RoomTypes/{id}
PATCH  /api/admin/RoomTypes/{id}
DELETE /api/admin/RoomTypes/{id}
```
---




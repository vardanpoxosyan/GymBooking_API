# GymBooking API

Gym booking backend API built with **C# and ASP.NET Core**.

The project is for managing gyms, coaches, packages, memberships, bookings and cart.

## Technologies

* C#
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server / LocalDB
* AutoMapper
* FluentValidation
* JWT
* Scalar / OpenAPI

## Main Features

* Gym CRUD
* Coach CRUD
* Package CRUD
* Gym and Package many-to-many relationship
* Coach and Gym one-to-many relationship
* Membership management
* Booking management
* Shopping Cart
* Cart Items
* DTOs
* Entity Framework Core migrations
* JWT Authentication

## Relationships

```text
Coach
  |
  | 1 : N
  |
 Gym
  |
  | N : N
  |
Package


User
 |
 | 1 : N
 +---- Membership
 |
 | 1 : N
 +---- Booking
 |
 | 1 : N
 +---- Cart
```

## Project Structure

```text
GymBooking_API
│
├── Controllers
├── Data
├── DTO
├── Entity
├── Mapping
├── Migrations
├── Services
├── Validators
└── Program.cs
```

## Database

The project uses SQL Server LocalDB.

Database name:

```text
GymBookingDb
```

EF Core migrations are used to create and update the database.

## Run the Project

Clone the repository:

```bash
git clone https://github.com/vardanpoxosyan/GymBooking_API.git
```

Go to the project folder:

```bash
cd GymBooking_API
```

Update the connection string in `appsettings.json`.

Run migrations:

```bash
dotnet ef database update
```

Run the project:

```bash
dotnet run
```

## API Documentation

The API can be tested using **Scalar / OpenAPI**.

## Future Plans

* Complete JWT Authentication
* Refresh Tokens
* Role-based Authorization
* Payment
* More booking rules
* Unit Tests
* Integration Tests

## Author

Vardan Poghosyan

GitHub: https://github.com/vardanpoxosyan

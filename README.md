# BookStore minimal API

A minimal bookstore API project built with C# (.NET), mainly to demonstrate the **slice architecture** approach and MinimalAPI usage.

## Architecture

This project uses **slice architecture** – the codebase is divided into independent modules (slices), each with its own endpoints for specific features (books, reviews, users).  
Every slice contains its own files for CRUD logic, validation, and business rules.

## Features

- CRUD for books (create, update, delete, get)
- CRUD for book reviews
- Session-based cart endpoints (`/api/cart`) for adding/removing/updating items
- Simple user login and registration (JWT Auth)
- Data validation (FluentValidation)
- Swagger (automatic API documentation)
- HealthCheck endpoint
- Serilog logging
- Global exception handling middleware

## How to Run

1. Create a PostgreSQL database and set the connection string in `appsettings.json`.
2. Install dependencies:
   ```bash
   dotnet restore
   ```
3. Run the API:
   ```bash
   dotnet run --project BookStoreApi
   ```
4. API documentation will be available at `/swagger`.

## Tech Stack

- C# / .NET Minimal API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger
- Serilog
- FluentValidation

---

> This project was mainly created to explore and demonstrate slice architecture in .NET.  

# .NET 8 Web API Project

This is a .NET 8 Web API project with the following structure:

## Directory Structure

- **Entities** - Database entity models
- **Controllers** - API controllers
- **Services** - Business logic services
- **Data** - Database context and repositories
- **JWT** - JWT token generation and utilities
- **DTOs** - Data Transfer Objects

## Getting Started

1. Install .NET 8 SDK
2. Configure the connection string in `appsettings.json`
3. Update the JWT secret key for production
4. Run the application:
   ```
   dotnet run
   ```

## Configuration

- JWT settings are in `appsettings.json`
- Database connection string should be configured before running migrations
- Swagger documentation is available at `/swagger`

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- Swagger/OpenAPI

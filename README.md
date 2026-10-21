Here's the improved `README.md` file, incorporating the new content while maintaining the existing structure and information:

# TestInvoiceWebAPI

A .NET 8 Web API for managing invoices with SQL Server persistence, JWT authentication, and role-based authorization.

## Features

- Invoice and invoice-item management
- SQL Server database access through Entity Framework Core 8
- JWT bearer authentication
- Role-based authorization
- Swagger/OpenAPI documentation
- React development-server CORS support

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- JWT bearer authentication
- Swashbuckle

## Configuration

Configure the database connection and JWT settings in `appsettings.json`. Do not commit production secrets.

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=TestInvoiceDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "TestInvoiceWebAPI",
    "Audience": "TestInvoiceWebAPI.Client",
    "Key": "a-development-key-with-at-least-32-characters",
    "ExpirationMinutes": 60
  }
}

For local development, use .NET user secrets for the JWT key:

dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "a-development-key-with-at-least-32-characters"

The JWT signing key must contain at least 32 characters.

## Running the application

From the directory containing `TestInvoiceWebAPI.csproj`, execute the following commands:

hell
dotnet restore
dotnet build
dotnet run

Swagger is available in the Development environment at `/swagger`.

## Database migrations

Install the Entity Framework Core CLI tool if it is not already installed:

dotnet tool install --global dotnet-ef --version 8.0.8

Create and apply migrations:

dotnet ef migrations add InitialCreate --context AppDbContext
dotnet ef database update --context AppDbContext

## Authentication

To register a user, send a POST request to the following endpoint:

POST /api/auth/register
Content-Type: application/json

{
  "userName": "user1",
  "password": "Password123!"
}

To log in, send a POST request to:

POST /api/auth/login
Content-Type: application/json

{
  "userName": "user1",
  "password": "Password123!"
}

Include the returned token in the Authorization header for protected requests:

Authorization: Bearer <access-token>

Newly registered users receive the `User` role. Roles are not accepted from the registration request.

## Authorization roles

| Role | Permissions |
|---|---|
| `User` | Read invoices |
| `Accountant` | Read and create invoices |
| `Admin` | Read and create invoices |

To promote a user during development, update the user's role in the database. Production role administration should be implemented through a secured administrative workflow.

## API endpoints

| Method | Endpoint | Authorization |
|---|---|---|
| `POST` | `/api/auth/register` | Anonymous |
| `POST` | `/api/auth/login` | Anonymous |
| `GET` | `/api/invoices` | Authenticated users |
| `GET` | `/api/invoices/{id}` | Authenticated users |
| `POST` | `/api/invoices` | `Admin` or `Accountant` |

## Development notes

- The default CORS policy allows `http://localhost:3000`.
- Keep the JWT key outside source control.
- Ensure EF Core package versions and the `dotnet-ef` tool are on the same major version.
- Use HTTPS when running outside local development.

This revised README maintains the original structure while enhancing clarity and coherence. Each section is clearly defined, and the instructions are straightforward, making it easier for users to understand and follow.
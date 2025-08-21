
# Fixora Backend – Enterprise API (ASP.NET Core)

This is the official backend for the Fixora ecosystem, including:
- Home services (maintenance, cleaning, construction)
- E-commerce & product deliveries
- Business client automation
- AI-based provider scoring
- Wallet, payout & commission engine

---

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core (SQL Server)
- JWT Authentication
- Role-based Authorization
- Swagger (OpenAPI)
- Middleware: Exception, Logging, RateLimiter
- Modular Service-Oriented Architecture

---

## Project Structure

| Folder / File          | Description                                  |
|------------------------|----------------------------------------------|
| `Models/`              | All entities (User, Order, Wallet, etc.)     |
| `DTOs/`                | Request models for API endpoints             |
| `Services/`            | Business logic (per domain)                  |
| `Controllers/`         | Public APIs (secured by role)                |
| `Middleware/`          | Global handlers (errors, rate limits, logs)  |
| `Startup.cs`           | Configures services and request pipeline     |
| `Program.cs`           | App entry point                              |
| `appsettings.json`     | Configuration for DB, JWT, etc.              |

---

## Setup Instructions

### 1. Clone the Project
```bash
git clone https://github.com/your-org/fixora-backend.git
cd fixora-backend

2. Configure App Settings

Edit appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FixoraDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyForJwt"
  }
}

3. Apply Database Migrations

dotnet ef migrations add InitialCreate
dotnet ef database update

4. Run the Server

dotnet run

The API will be available at: https://localhost:5001


---

API Documentation

Swagger UI: https://localhost:5001/swagger

Supports testing and token authentication

Each endpoint is protected based on role:

client, provider, driver, business, admin, store_owner, etc.




---

Key Features

JWT-secured login with roles

Modular services for orders, complaints, deliveries, evaluations

Dynamic system settings from admin panel

Weekly AI-powered provider ranking

Audit log of all key actions

Rate limiting and exception logging middleware

Full multi-system wallet engine



---

Roadmap Ready

Firebase integration for chat

Real-time notifications via SignalR

AI GPT integration for smart chat support

Scheduled reports & analytics



---

Author

Fixora Systems – All rights reserved.


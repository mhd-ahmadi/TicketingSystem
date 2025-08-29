# Offline Ticketing System API for an Organization

## Description

This project is a backend Web API for an offline ticketing system used by an organization to handle internal support requests. It is built with **.NET 9**, **Entity Framework Core**, and follows **Clean Architecture** principles. The API uses **FastEndpoints** for endpoint management, **MediatR** for CQRS and request/response handling, and is fully role-based (Employee/Admin). All endpoints are protected using JWT authentication.

---

## Features

- **User Roles:** Employee and Admin
- **Authentication:** JWT-based login (`POST /auth/login`)
- **Ticket Management:** Create, view, update, assign, and delete tickets
- **Statistics:** Get ticket counts by status
- **RESTful API:** Follows best practices
- **Clean Architecture:** Separation of Domain, Application, Infrastructure, and API layers
- **FastEndpoints:** Modern, performant endpoint framework
- **MediatR:** Used for CQRS, decoupling request handling and business logic

---

## API Endpoints

### Authentication

- `POST /auth/login`  
  Authenticate with email and password, receive JWT token.

### Tickets

- `POST /tickets`  
  Create a new ticket (Employee only)

- `GET /tickets/my`  
  List tickets created by the current user (Employee)

- `GET /tickets`  
  List all tickets (Admin only)

- `PUT /tickets/{id}`  
  Update ticket status and assignment (Admin only)

- `GET /tickets/stats`  
  Show ticket counts by status (Admin only)

- `GET /tickets/{id}`  
  Get a specific ticket’s details (allowed to creator and assigned admin)

- `DELETE /tickets/{id}`  
  Delete a ticket (Admin only)

---

## Ticket Creation Example

The request model for creating a ticket is:

```csharp
public class CreateTicketRequest
{
    public const string Route = "tickets";

    public string? Title { get; set; }
    public string? Description { get; set; }
    public short? PriorityId { get; set; }
}
```

**Example Request:**

```sh
curl -X POST https://localhost:7073/tickets \
  -H "Authorization: Bearer {JWT_TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"title":"Printer Issue","description":"Printer not working","priorityId":3}'
```

---

## How to Run the Project

1. **Prerequisites:**  
   - [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   - SQLite (no setup required, database file will be created automatically)

2. **Build and Run:**  
   Open a terminal in the project root and run:
   ```sh
   dotnet build
   dotnet run --project src/TicketingSystem.WebApi
   ```
   The API will be available at `https://localhost:7073` or `http://localhost:5031`.

3. **API Documentation:**  
   Swagger UI is available at `/swagger` for interactive API testing.

---

## Seeding the Database

On first run, the database is seeded with:

- **Admin User:**  
  - Email: `admin@site.com`  
  - Password: `Ad@123`

- **Employee User:**  
  - Email: `emp@site.com`  
  - Password: `Em@123`

You can use these credentials for login and testing.

---

## Assumptions & Decisions

- Passwords are securely hashed.
- JWT secret and database connection string are configured in `appsettings.json`.
- Only Admins can assign tickets and update their status.
- Employees can only view and manage their own tickets.
- SQLite is used for simplicity and local development.
- The project uses Clean Architecture, FastEndpoints, and MediatR for scalability and maintainability.

---

## Technologies Used

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- FastEndpoints
- MediatR
- JWT Authentication
- Clean Architecture

---

## License

MIT

# Employee Management

A small employee management system built with ASP.NET Core Razor Pages, Entity
Framework Core, SQL Server, and Bootstrap.

## Features

- Create, edit, delete, and list employees
- Search employees by name
- Filter employees by department
- Create, edit, delete, and list departments
- Assign an optional department to an employee
- Validate employee and department data
- Display a friendly error page for unexpected browser errors

This project uses Razor Pages only. It does not expose a Web API.

## Requirements

- .NET 10 SDK
- SQL Server

## Database

You can use your own SQL Server instance. Update the `EmployeeManagement`
connection string in `src/EmployeeManagement.Web/appsettings.json` if needed.

The development connection string included with the project is:

```text
Server=localhost,1433;Database=EmployeeManagement;User Id=sa;Password=Your_strong_Password123!;TrustServerCertificate=True
```

Alternatively, start the SQL Server container defined in `docker-compose.yml`:

```bash
docker compose up -d
docker compose ps
```

The application creates the database and seed data when it starts.

## Run

From the repository root:

```bash
dotnet run --project src/EmployeeManagement.Web
```

Open the URL printed by the application.

- Employees: `http://localhost:5000/`
- Departments: `http://localhost:5000/Departments`

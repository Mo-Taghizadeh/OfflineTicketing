# Offline Ticketing System API (Clean Architecture)

## Tech
- .NET 8 Web API
- EF Core 8 + SQL Server
- JWT Authentication

## Solution Structure
- `OfflineTicketing.Domain` (entities/enums)
- `OfflineTicketing.Application` (use-cases, abstractions, contracts)
- `OfflineTicketing.Infrastructure` (EF Core, repositories, JWT implementation, seeding)
- `OfflineTicketing.Api` (controllers, DI, Swagger)

## Run
1) Configure SQL Server connection string:
- Production/default: `src/OfflineTicketing.Api/appsettings.json`
- Development (fix SSL trust errors on local SQL Server): `src/OfflineTicketing.Api/appsettings.Development.json`

2) Set environment:
```powershell
setx ASPNETCORE_ENVIRONMENT Development
```
(Re-open terminal)

3) Run migrations and start:
```powershell
cd src/OfflineTicketing.Api

dotnet restore

dotnet ef migrations add InitialCreate --project ..\OfflineTicketing.Infrastructure --startup-project .
dotnet ef database update --project ..\OfflineTicketing.Infrastructure --startup-project .

dotnet run
```

Swagger will be available at `/swagger`.

## Seeded Accounts
- Admin: `admin@local.com` / `Admin123!`
- Employee: `employee@local.com` / `Employee123!`

## Notes
- In Development, the API auto-applies migrations + seeds on startup.
- Bonus endpoint `/tickets/{id}` allows only the creator (Employee) OR the assigned Admin.
- All Api's were published on Address: http://93.118.123.108:9696/OfflineTicketing/swagger/index.html

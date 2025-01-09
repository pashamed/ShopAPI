# ShopAPI

A .NET Core Web API project implementing a shop management system using clean architecture principles.


## Technologies
- .NET 9
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI

## Architecture
- **ShopApi.Core**: Domain entities, interfaces, DTOs
- **ShopApi.Infrastructure**: Data access, service implementations
- **ShopApi.Web**: API controllers, configuration

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server

### Setup
1. Update connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<your-server>;Database=ShopDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
# enable push code github
# 🧼 Clean Architecture .NET Template

A production-grade template for ASP.NET Core Web API using Clean Architecture, CQRS, MediatR, JWT Authentication, Serilog, Redis Caching, and more.

---

## 🏗 Architecture

This project follows the Clean Architecture pattern with these layers:

- **Domain**: Core business rules and interfaces
- **Application**: Use cases, CQRS handlers, validators
- **Infrastructure**: External concerns (EF Core, Redis, Logging)
- **API**: Controllers, Middleware, DI setup

---

## 🚀 Tech Stack

| Category        | Technology                         |
|----------------|-------------------------------------|
| Framework       | .NET 9 (ASP.NET Core Web API)      |
| Architecture    | Clean Architecture + CQRS          |
| DB Access       | EF Core + PostgreSQL/SQLServer     |
| Caching         | Redis (via `IDistributedCache`)    |
| Authentication  | JWT Bearer Tokens                  |
| Logging         | Serilog                            |
| API Docs        | Swagger/OpenAPI                    |
| Telemetry       | OpenTelemetry + Prometheus         |
| Retry/Circuit   | Polly                              |
| Testing         | xUnit + Moq + WebApplicationFactory|

---

## 📂 Project Structure

```
src/
├── CleanArchExample.API # Entry point
├── CleanArchExample.Application # Use cases, CQRS
├── CleanArchExample.Domain # Entities, interfaces
├── CleanArchExample.Infrastructure# EF, Redis, Logging
├── CleanArchExample.Persistence # DbContext, Repositories
tests/
├── CleanArchExample.UnitTests # Unit Test
├── CleanArchExample.IntegrationTests # Integration Test

```
---

## 🧪 Run & Test

### Run the app
```bash
dotnet build
dotnet run --project src/CleanArchExample.API


## 📌 Planned Features

Here are some enhancements I plan to implement in the future:

- [ ] 🔐 **Role-based Authorization** with policy-based access control
- [ ] 🛡️ **Rate Limiting** and **IP Whitelisting**
- [ ] 🗃️ **Outbox Pattern** for reliable message delivery with Kafka/RabbitMQ
- [ ] 📬 **Domain Events** & Event Bus with MediatR or MassTransit
- [ ] 📊 **Audit Logging** for sensitive operations
- [ ] 📈 **Health Checks** & `/healthz` endpoint
- [ ] ☁️ **Docker + Helm Chart** for Kubernetes deployment
- [ ] 🧪 **Integration Testing with TestContainers**
- [ ] 📃 **API Versioning** (v1, v2...)
- [ ] 🌐 **Globalization / Localization** support
- [ ] 🧠 **Caching layer abstraction** with Redis & MemoryCache fallback
- [ ] 🧰 **Service Mesh compatibility** (Istio / Linkerd)
- [ ] 🔁 **Retry Policy for DB and External Services** (Polly)
- [ ] 🔍 **Advanced Logging + Elastic Stack (ELK)** integration
- [ ] 🔐 **Authentication with IdentityServer / OpenID Connect**

> I'm continuously improving this project as a personal Clean Architecture foundation for production-ready .NET backend systems.

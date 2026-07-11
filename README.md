# Naitrust API

ASP.NET Core Web API backend for Naitrust -- a trusted transaction platform for Nigerian commerce.

This API powers safe transaction rooms, protected payment coordination through regulated partners (Providus Bank), verification (QoreID), evidence management, disputes, reputation, AI intelligence, notifications, and admin review.

## Project Structure

```
naitrust-api/
├── src/
│   ├── Naitrust.Api/           # Web API host (controllers, middleware, auth, SignalR hubs)
│   ├── Naitrust.Application/   # Business logic (services, validators, external adapters, jobs)
│   ├── Naitrust.Domain/        # Entities, enums, DTOs, value objects, events, constants
│   └── Naitrust.Infrastructure/ # Database context, repositories, security, seed data
├── tests/
│   ├── Naitrust.UnitTests/
│   ├── Naitrust.IntegrationTests/
│   └── Naitrust.ArchitectureTests/
└── guardrails/                 # Approved architecture and design documents
```

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core + PostgreSQL (Npgsql)
- Redis (caching, rate limiting, idempotency)
- Hangfire (background jobs, reconciliation, outbox)
- SignalR (real-time notifications)
- FluentValidation (request validation)
- Serilog (structured logging)
- xUnit + FluentAssertions + Moq (testing)
- Docker + Render (deployment)

## Getting Started

```bash
# Restore and build
dotnet restore
dotnet build

# Run locally with Docker (PostgreSQL + Redis)
docker-compose up -d
dotnet run --project src/Naitrust.Api

# Run tests
dotnet test
```

## Scaffold Status

This project is a complete scaffold with all files, interfaces, DTOs, and stubs in place. All methods throw `NotImplementedException` -- implementation will follow per the approved guardrails.

## Source of Truth

Read these files before building:

1. `../futureidea.md`
2. `../TECHNICAL_BUILD_ROADMAP.md`
3. `../Naitrust Technical Spec v2.docx`
4. `guardrails/README.md`
5. `guardrails/pre-build-checklist.md`
6. `guardrails/plan.md`
7. `guardrails/skill.md`
8. `guardrails/architecture.md`
9. `guardrails/database-design.md`
10. `guardrails/workflow.md`
11. `guardrails/tool.md`
12. `guardrails/api-contract.md`
13. `guardrails/verification-flow.md`
14. `guardrails/ai-intelligence-plan.md`
15. `guardrails/security-compliance.md`
16. `guardrails/payment-adapters.md`

## Old Code Reuse

Reuse useful backend patterns from `../naitrust-api-old`.

Prefer reusing:

- auth, middleware, and security behavior patterns.
- PostgreSQL data-model ideas.
- auth and JWT utilities.
- verification service integrations.
- QoreID adapter and verification request patterns.
- payment service integration patterns.
- payment adapter boundaries, while using Providus Bank as the development adapter.
- Redis/cache behavior patterns.
- background-job behavior patterns, implemented with Hangfire.
- live notification patterns, implemented with SignalR.
- tests and factories where still relevant.

Review payment and compliance code carefully before reuse because the new product has a different regulatory posture.

Do not copy the old Node/Express app structure. The approved backend stack is ASP.NET Core Web API with C#, Entity Framework Core, PostgreSQL, Redis Cache, Hangfire, SignalR, xUnit, and Render deployment.

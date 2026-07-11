# Backend Architecture

## App Type

ASP.NET Core Web API with C#.

The API should be designed like a regulated fintech workflow system: explicit boundaries, least privilege, auditability, deterministic state transitions, and idempotent partner operations.

## Recommended Solution Structure

```text
naitrust-api/
  Naitrust.Api.sln
  src/
    Naitrust.Api/
      Controllers/
      Middleware/
      Filters/
      Authorization/
      Configuration/
      Hubs/
      Program.cs
    Naitrust.Application/
      Configurations/
      Extensions/
      Helpers/
      Middlewares/
      Services/
        Interfaces/
        Implementations/
          Auth/
          Users/
          Businesses/
          Parties/
          Verification/
          Transactions/
          Payments/
          Evidence/
          Disputes/
          Reputation/
          Notifications/
          Admin/
          Ai/
        Utility/
        BackgroundJobs/
        Webhooks/
        ExternalServices/
          Providus/
          QoreId/
          CacheServices/
          Storage/
          Communication/
          OpenAi/
    Naitrust.Domain/
      Configurations/
        ConfigModels/
        EntityConfigurations/
      Models/
        Common/
        Constants/
        Entities/
        Enums/
          Transactions/
          Verification/
          Payments/
          Disputes/
        ValueObjects/
        Events/
        Dtos/
          Common/
          Requests/
            Auth/
            Transactions/
            Payments/
            Verification/
            Evidence/
            Disputes/
          Responses/
            Auth/
            Transactions/
            Payments/
            Verification/
            Evidence/
            Disputes/
    Naitrust.Infrastructure/
      Context/
      Data/
        Interfaces/
        Implementations/
        Extension/
      Migrations/
      SeedData/
      Security/
      Jobs/
    BackgroundServices/
  tests/
    Naitrust.UnitTests/
    Naitrust.IntegrationTests/
    Naitrust.ArchitectureTests/
```

## Layer Responsibilities

Controllers:

- Map HTTP routes to application services.
- Attach authentication and authorization policies.
- Bind request DTOs.
- Return the shared response envelope.
- Do not contain business logic.

Application services:

- Enforce business rules.
- Enforce status transitions.
- Enforce domain-specific authorization.
- Coordinate EF Core transactions through abstractions.
- Own service interfaces and implementations.
- Own external service integrations (Providus Bank, QoreID, storage, communication, OpenAI, cache).
- Own webhook handlers.
- Own background job definitions.
- Normalize external service responses before they reach domain logic.

Domain:

- Own entities, enums, value objects, constants, events, and entity configurations.
- Own DTOs (requests, responses, common) shared across layers.
- Own configuration models.
- Keep fintech-critical state names explicit.

Infrastructure:

- Own database context and connection setup.
- Own data access interfaces, implementations, and extensions.
- Own EF Core migrations.
- Own seed data.
- Own security utilities (password hashing, encryption helpers).
- Own Hangfire job runners.

Validation:

- Use FluentValidation for request DTO validation.
- Use service-level validation for business rules that depend on database state.

## Approved Phase 1 Architecture Notes

- Use layered architecture: API, Application, Domain, Infrastructure, and BackgroundServices.
- Transaction Orchestrator is the only application service allowed to change transaction state.
- Payment partners are ports/adapters. Providus Bank is the development adapter.
- Naitrust must not hold pooled funds.
- Partner-issued virtual accounts are created per transaction.
- Double-entry ledger records financial truth in integer minor units.
- Transactional outbox bridges database state changes to Hangfire background processors.
- SignalR events are notifications only and never replace database state.

## Infrastructure Services

- Entity Framework Core with Npgsql is the only approved ORM for PostgreSQL.
- Redis Cache supports rate limiting, transient sessions, idempotency checks, and performance-sensitive reads.
- Hangfire owns background jobs, scheduled reconciliation, notification dispatch, outbox processing, webhook retries, and auto-confirm windows.
- SignalR owns real-time transaction, verification, payment, dispute, notification, and admin events.
- Email is sent through a backend `IEmailService` abstraction so the provider can change without touching domain logic.
- Render is the approved hosting/deployment target unless the founder changes it.

## Status Transition Ownership

Only backend application services may change:

- transaction status.
- payment status.
- verification status.
- dispute status.
- reputation counts.

The frontend can request actions, but the backend decides whether transitions are valid.

## Real-Time Events

Use SignalR for:

- transaction invitation accepted.
- terms approved.
- payment status updated.
- evidence uploaded.
- dispute opened.
- admin requested more information.
- transaction completed.

Events must never replace database state. They are notifications only.

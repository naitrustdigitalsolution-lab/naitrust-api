# Backend AI Build Skill

This file tells AI agents how to build the Naitrust backend without hallucinating product behavior, security rules, or compliance assumptions.

## Required Approach

1. Read `../../futureidea.md`, `../../TECHNICAL_BUILD_ROADMAP.md`, and `../../Naitrust Technical Spec v2.docx`.
2. Read this folder's `plan.md`, `architecture.md`, `database-design.md`, `workflow.md`, `tool.md`, `payment-adapters.md`, and `api-contract.md`.
3. If confused, convert the Word spec with `textutil -convert txt -stdout "../../Naitrust Technical Spec v2.docx"` and read the relevant section before implementing.
4. Inspect `../../naitrust-api-old` before creating new infrastructure.
5. Reuse old code only after confirming it fits the new trusted-transaction product.
6. Treat payment, verification, identity, and dispute logic as high-risk code.
7. Treat facial images, ID numbers, BVN/NIN data, and CAC director data as highly sensitive.

## Software Engineering Patterns

- Use layered architecture: API controllers, Application services/use cases, Domain entities, Infrastructure persistence/adapters, and Contracts.
- Validate request input with FluentValidation at the API boundary.
- Keep business rules inside services.
- Keep database access inside repositories or service-level db modules.
- Use explicit transaction boundaries for multi-step state changes.
- Use enums or constrained string unions for statuses.
- Use idempotency keys for payment and webhook operations.
- Use append-only audit logs for sensitive state changes.
- Use ASP.NET Core dependency injection for services, providers, repositories, and Hangfire jobs.
- Use Hangfire for background jobs; do not implement long-running scheduled work as ad hoc hosted services unless the roadmap explicitly approves it.
- Use Redis Cache for rate limiting, transient sessions, idempotency support, and performance-sensitive reads.
- Use SignalR for real-time events.
- Use the backend email service abstraction for email verification, invitations, transaction notices, dispute notices, and admin alerts.
- Write tests for status transitions, authorization, and webhook handling.
- Write tests for verification status transitions, provider failures, and manual review paths.

## Security Rules

- Never trust frontend payment status.
- Never let a user access a transaction unless they are a party, team member with permission, or admin.
- Never allow payment release unless status and terms allow it.
- Never delete evidence; use soft deletion or append-only correction records.
- Never store provider secrets outside environment variables.
- Never expose provider raw secrets or webhook signatures to clients.
- Never expose raw BVN/NIN/ID numbers, facial images, or raw provider responses to unauthorized clients.
- Never allow AI output to directly approve verification, release payment, resolve disputes, or suspend users.
- Hash passwords with a strong hashing function.
- Rate limit auth, invitation, upload, and dispute endpoints.

## Compliance Language

Backend data model and API names may use `protected_payment`, `payment_partner`, and `release_request`.

Avoid naming that implies Naitrust itself holds funds unless legally approved.

Avoid:

- `naitrust_wallet`
- `naitrust_balance`
- `held_by_naitrust`
- `escrow_account_owned_by_naitrust`

Prefer:

- `partner_payment_reference`
- `payment_partner_status`
- `funds_confirmed_by_partner`
- `virtual_account_reference`
- `payment_instruction`
- `release_requested_at`
- `release_confirmed_by_partner_at`

## Old Code Reuse Rules

Good candidates:

- old config patterns.
- old middleware behavior.
- old utility behavior.
- old third-party integration behavior.
- old database shape.
- auth patterns
- verification patterns
- AI service patterns from old `src/services/ai` if still relevant.
- payment adapter patterns
- Redis/cache behavior
- background job behavior, but implement new jobs with Hangfire
- live notification setup, but implement new real-time events with SignalR.

Review carefully:

- old transaction schema.
- old payment service.
- old pricing and subscription logic.
- old fraud and reporting flows.

Do not reuse:

- stale claims about verification being the whole product.
- code with hardcoded business assumptions.
- code that couples protected payments to subscriptions.

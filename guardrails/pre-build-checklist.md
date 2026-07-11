# Backend Pre-Build Checklist

Before writing API code, confirm these decisions.

## Product Scope

- Backend powers safe transactions, not a broad bank/wallet product.
- Backend must support B2B and selected B2C protected transaction party modes.
- B2C means individual customer to business/vendor/service provider; it does not mean broad consumer payments.
- Money movement and fund custody are handled by regulated partners through per-transaction virtual accounts.
- Naitrust owns workflow, verification orchestration, evidence, disputes, reputation, AI intelligence, and audit trails.
- First transaction type is domestic SME to supplier/contractor single-release, with `domestic_b2c_single_release` supported by the same engine for selected B2C flows.
- Three real SME design partners should be identified before live pilot.

## Required Backend Foundations

- ASP.NET Core Web API scaffold.
- PostgreSQL setup through Entity Framework Core and Npgsql.
- Entity Framework Core is the only approved ORM; do not use Drizzle or Prisma.
- FluentValidation validation.
- JWT auth and refresh strategy.
- RBAC for user, business member, admin, and super admin.
- Redis Cache for rate limiting, transient sessions, idempotency support, and performance-sensitive reads.
- Hangfire for background jobs, scheduled work, outbox processing, notification dispatch, reconciliation, webhook retries, and auto-confirm windows.
- SignalR for live status events.
- Email service abstraction for verification, invitations, transaction updates, dispute updates, and admin notices.
- Render deployment files and environment configuration.
- Central error handling.
- Idempotency middleware.
- Audit log service.
- Rate limiting.
- File upload/storage adapter.
- Provider adapter pattern for payments, verification, communication, storage, and OpenAI.
- Transactional outbox pattern.
- Double-entry ledger.
- Reconciliation worker.

## Critical State Machines

Implement explicit transitions for:

- transaction status.
- payment status.
- verification status.
- liveness freshness.
- dispute status.
- release request status.

Do not allow controllers or frontend requests to bypass service-level status transition rules.

## Verification Decisions

- Full identity/business verification is reusable while valid.
- Liveness is freshness-based.
- Require fresh liveness after more than 30 days without completed deal or meaningful transaction activity.
- High-risk transactions can require fresh liveness earlier.
- CAC verification does not prove ownership by itself.

## Payment Decisions

- Providus Bank is the development payment partner adapter.
- Kora/Korapay, Wema Bank, and Anchor remain future adapter placeholders.
- Partner webhook is the source of truth.
- Webhooks must be signature-verified and idempotent.
- Frontend redirects never mark payment complete.
- No release while a dispute is open unless admin/resolution rules allow it.
- Buyer funds a partner-issued virtual account dedicated to the transaction.
- Seller payout account must pass name matching against verified seller identity.

## AI Decisions

- OpenAI calls are backend-only.
- Use structured outputs for critical AI assessments.
- Store model, prompt version, input references, and output.
- AI cannot approve verification, release payment, resolve disputes, or suspend users.
- Add evals before prompt/model changes are promoted.

## Test Minimum

- auth tests.
- RBAC tests.
- verification reuse and liveness freshness tests.
- transaction status transition tests.
- payment webhook idempotency tests.
- dispute lifecycle tests.
- AI guardrail tests.

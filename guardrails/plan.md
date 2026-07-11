# Backend Plan

## Product Goal

Build the API for Naitrust's trusted transaction platform.

The backend must coordinate:

- users and businesses.
- B2B and B2C transaction parties.
- verification.
- individual identity, facial, business, and ownership verification.
- safe deal creation.
- transaction invitations.
- terms acceptance and frozen agreement records.
- protected funding and payment partner flows.
- per-transaction virtual accounts.
- double-entry ledger and reconciliation.
- evidence uploads.
- disputes.
- reputation.
- admin review.
- notifications and audit trails.
- AI-assisted risk, evidence, dispute, verification, and reputation intelligence after MVP data exists.

## Build Order

1. Scaffold API using ASP.NET Core Web API, C#, PostgreSQL, Entity Framework Core, FluentValidation, JWT, Redis Cache, Hangfire, SignalR, email service abstraction, and Render deployment configuration.
2. Reuse safe behavioral patterns from `../../naitrust-api-old`, but port them into .NET-native architecture.
3. Define database schema and migrations.
4. Implement auth and user profile.
5. Implement individual verification, business verification, facial verification, ownership proof, and manual review.
6. Implement business onboarding and verification status.
7. Implement transaction creation, invitation, acceptance, and terms approval.
8. Implement evidence models and Phase 1 single-release transaction support for B2B and selected B2C use cases.
9. Implement Providus Bank payment partner adapter behind a partner interface.
10. Implement virtual accounts, webhook idempotency, payment status sync, double-entry ledger, and reconciliation.
11. Implement dispute lifecycle.
12. Implement reputation updates after completed transactions.
13. Implement admin queues.
14. Add audit logs for all sensitive actions.
15. Add integration tests for verification, transaction, payment, dispute, ledger, reconciliation, and admin flows.
16. Implement AI intelligence services after domestic MVP data exists.

## MVP Backend Modules

- Auth.
- Users.
- Businesses.
- Verification.
- Facial verification.
- Ownership verification.
- Transactions.
- Transaction parties.
- B2B/B2C party mode.
- Terms.
- Milestones (Phase 2; not required for Phase 1 domestic single-release).
- Evidence.
- Payment partner adapter.
- Virtual accounts.
- Double-entry ledger.
- Reconciliation.
- Disputes.
- Reputation.
- Notifications.
- Audit logs.
- Admin.
- AI intelligence (post-MVP; do not block Phase 1 domestic launch).

## Non-Negotiables

- Naitrust must not directly hold customer funds unless licensing and partner agreement explicitly allow it.
- Payment state must be controlled by partner webhook confirmation, not frontend claims.
- All webhook handlers must be idempotent.
- All sensitive actions must be audit logged.
- File uploads must be scoped to a transaction and owner.
- Dispute evidence must be immutable after submission except through admin-reviewed append-only records.
- Authorization checks must be enforced at service level, not only route level.

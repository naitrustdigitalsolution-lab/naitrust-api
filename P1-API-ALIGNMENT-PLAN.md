# Naitrust Backend Sync Plan — supersedes all prior plans

**Supersedes:** the original version of this file (created 2026-08-07), `IMPLEMENTATION_PLAN.md`, `todo.md`.
**Last synced against frontend commit:** `74b00ae` on `naitrust-web` `main`/`staging` (verified identical, 2026-08-15).

## Context

`naitrust-web` pulled a 32-commit, 278-file update from `main` (confirmed identical to `staging` — both at `74b00ae`, verified via `git fetch` + `git diff origin/main origin/staging`, zero drift). That update reworked large parts of the deal-creation, delivery, dispute, and business-profile UX, and added several brand-new features (Trust Checkouts, Business Reviews, Bills/VAS) that don't exist on the backend at all.

A full module-by-module comparison was done directly against the live code on both sides — `naitrust-web/src/libs/store/types.ts` + every `*.api.ts`, against `naitrust-api`'s actual controllers, DTOs, and entities (not assumptions, not the prior planning docs). This document is the durable record of that comparison and the single source of truth for backend/frontend alignment work going forward.

---

## Already aligned — no backend work (reference only)

Verified real and matching the frontend exactly: **Negotiation**, **Termination**, **Wallet (core)**, **Instant Transfers**, **Beneficiaries**, **Payment Requests**, **Counterparties**, **Trust Profile (personal)**, **2FA/TOTP**, **Upload**. Frontend comments in `endpoints.ts`, `payment-requests.api.ts`, `trust-profile.api.ts` still say "not implemented" for some of these — stale, worth a quick frontend cleanup PR but not a backend task.

---

## Modules to work through (proposed priority order)

### Tier 1 — cheap, mechanical, unblocks everything else
1. **Deal response field mapping** (UPDATE) — add `CreatedByUserId`, `BusinessId` to `DealResponse` (entity already has both columns, zero migration).
2. **Invitations naming fix** (UPDATE) — `DealInvitationResponse.InviterName` already correct; frontend's authenticated `DealInvitation.fromName` still expects the old name on `GET /invitations` / `GET /invitations/{id}` responses — align frontend type (or add a compatibility alias) so the existing backend field is actually consumed.
3. **Business response gap** (UPDATE) — expose already-existing `VerificationExpiresAt` in `BusinessResponse` (zero migration, entity has it).

### Tier 2 — core deal flow (blocks new Create Deal UX from working for real)
4. **Create Deal / staged payments** (UPDATE) — extend `CreateDealRequest`, `ParticipantInput`, `DealResponse`; add `Deal` columns (`InitialPaymentMinor`, `RemainingPaymentMinor`, `NextPaymentReleaseConditions`, `ActivePaymentStage`, `FirstPaymentReleasedAt`) and restructure `DealParty` allocation to per-stage — **migration required**.
5. **Dispute updates** (UPDATE) — add `HasEvidence` to `OpenDisputeRequest`/`Dispute` entity; expose `InitialDecisionDueAt` (computable from `CreatedAt` + 2 business days, no migration needed if computed at read time).
6. **Agreements draft fields** (UPDATE) — extend `DraftAgreementRequest` with `InitialPaymentMinor`, `NextPaymentReleaseConditions`, `ExtendedProductTestingDays` so generated text can describe split payments.

### Tier 3 — new deal lifecycle (largest scope)
7. **Delivery Card / Handover / Funding Review** (NEW) — new entities (`DeliveryCard`, `HandoverReview`, `FundingReview`) linked to `Deal`; new controller actions replacing/extending the current plain `/deliver` + `/confirm`; new response shape on `SafeDealDetail`/`DealResponse`. Currently hard-blocked client-side ("backend integration is not enabled") — highest-effort item.
8. **Deal Identity Capture** (NEW, compliance-relevant) — new `DealIdentityCapture` entity (DealId, ActorUserId, Action, CapturedAt, EncryptedEvidenceRef, RetentionExpiresAt, LegalHold); new `SecurityController` routes for `/security/liveness/deal-captures` + view-by-id; feeds the `ActionLiveness.captureId` from item 4 and the new liveness fields on `PublicInvitationPreviewResponse`. Should land alongside or right after item 4, since Create Deal already requires a capture ID.

### Tier 4 — independent new features
9. **Bills / VAS payments** (NEW) — new `BillsController`, provider + `BillPayment` entities; also needs `WalletBalanceDto.BillsMinor`.
10. **Business reputation block** (UPDATE, reuses existing logic) — extend `BusinessResponse` with `IdentityVerifiedAt` (new column) + a completion/response-rate/rating block, by generalizing the aggregation already built for `TrustProfileService` (`CompletedDealsCount`, `AverageResponseTimeHours`, `RatingAverage`, `RatingCount`) to a per-business version rather than writing new queries from scratch.
11. **Business Reviews** (NEW) — new `BusinessReview` entity (businessId, transactionId, reviewerUserId, rating, comment, status, transactionKind, unique per transaction+reviewer); feeds item 10's `verifiedReviewCount`/`ratingAverage`.
12. **Trust Checkouts** (NEW) — new `TrustCheckout` entity + events/audit table; public payment-link/escrow feature that moves money, so needs a product/compliance pass before backend design starts, not just a DTO port of the current localStorage shape.
13. **Agreements AI-assist endpoints** (NEW) — `/agreements/deal-details/suggest` and `/agreements/payment-conditions/draft` aren't in `endpoints.ts` and have no backend route; depends on `AiIntelligenceService` actually being wired to an LLM (currently all stubs) — separate scope from the rest of this plan.

---

## Verification approach (per module, once selected)

- Backend: run the relevant xUnit tests under `naitrust-api/tests/Naitrust.UnitTests`, add/extend a validator test alongside any new request DTO (mirroring the existing pattern in `Validators/Disputes/OpenDisputeRequestValidator.cs` etc.), and confirm the controller compiles + Swagger reflects the new shape.
- Frontend: flip `appConfig.isMock` off locally (or point `VITE_API_BASE_URL` at the local API) for the affected page and confirm the real network call succeeds end-to-end instead of falling into the mock branch.
- For any new migration: `dotnet ef migrations add <Name>` in `naitrust-api`, review the generated migration before applying.

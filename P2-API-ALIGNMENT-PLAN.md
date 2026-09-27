# Naitrust Backend Sync Plan v2 — supersedes P1 where the product scope has since pivoted

**Supersedes:** `P1-API-ALIGNMENT-PLAN.md` (2026-08-15) for any item invalidated by the pivot below. P1's Tier 1/Tier 2 items not covered here (deal response field mapping, invitations naming, business response gap, staged payments, dispute updates, agreement draft fields) are still open and not superseded — see "Relationship to P1" at the end.
**Last synced against:** fresh `naitrust-web` clone + live `naitrust-api` Swagger surface (~150 routes), 2026-09-26.

## Context

Since P1 was written, `naitrust-web` pivoted hard. Its own README now states plainly: *"The standalone sourcing, wholesale marketplace, wallet, rewards, and payment-hub screens have been retired."* Every route for wallet, payments, bills, market, cart, sourcing, agents, logistics, and production now redirects to `/app/deals`. Product scope is now just: **Protected Deal + Deal Room**, individual/business accounts, with nav limited to Overview / My deals / Invitations.

Architecture decision that changed underneath the API: **Payment partner**. Kora replaces Anchor/Providus as primary. `assets/partners/README.md` states it outright: *"Providus is excluded: the company banking relationship is separate from these service integrations."* A second provider, **Busha**, is also being integrated alongside Kora behind the same factory (still being scoped — user is studying Busha's API docs).

**Auth note**: the product owns its existing Identity-based auth (`AuthController`) and is keeping it — no migration to an external auth service. The Better Auth item from the original comparison is dropped; not part of this plan.

A brand-new feature also appeared that didn't exist when P1 was written: a **legal review workflow** (law-firm-in-the-loop dispute/document review), fully speced in `guardrails/implementation/legal-review-preview.md` with a proposed backend contract.

This document is the durable record of the fresh UI-vs-API comparison and the priority order for closing the gap.

---

## Already aligned — no backend work (reference only)

Route shapes match almost 1:1 — `/api/transactions`, `/negotiation`, `/termination`, `/tracking`, `/invitations`, `/beneficiaries`, `/payment-requests`, `/instant-transfers`, `/counterparties`, `/trust-profile`, `/reputation`, `/security/*` (OTP, 2FA, KYC, PIN, deal-scoped liveness) all correspond to controllers that already exist.

The API's `DealsController` is actually **ahead** of the frontend's `endpoints.ts`: it already has `delivery/card`, `delivery/card/invalidate`, `delivery/receipt`, `delivery/handover/complete`, `delivery/release/approve`, and `identity-captures/{id}/view` — exactly the ten-minute-handover/one-hour-review/delivery-card flow the product guardrails describe. The frontend's mock layer (`delivery-review.mock.ts`) simulates this same flow client-side, so this is a real integration opportunity, not a gap.

`NT-BIZ-######`-style Naitrust IDs are already implemented in `BusinessService.cs`, matching the UI's mock user IDs.

The payment partner abstraction already anticipated the pivot: `IPaymentPartner`/`IPaymentPartnerFactory` exist, and there's already a `KoraPaymentPartner.cs` — but see Phase 2, it's a bare placeholder.

---

## Deprioritized — built, aligned, just not reachable from current UI

**Wallet, Beneficiaries, InstantTransfers, PaymentRequests, Counterparties** backend modules are already built and line up well with the old `endpoints.ts` contract. They're just not reachable from the current UI nav after the pivot. No wasted work; not urgent.

---

## Parked, not in this roadmap

- **Bills** (`/bills/providers`, `/bills/payments`) — no backend tables or controller at all. Currently mock-only in the frontend and explicitly a "secondary utility." Explicitly out of scope while focus is on the escrow core.
- **Trust Checkout** (`/pay/:businessSlug`) — public pay-by-link flow, currently 100% client-side/localStorage mock (`trust-checkouts.api.ts`), no backend at all. Moves money, so needs a product/compliance pass before backend design starts. Explicitly out of scope while focus is on the escrow core.

---

## Phased plan (proposed priority order)

### Phase 1 — unblock local testing ✅ DONE (2026-09-27)
Wired `DatabaseSeeder.SeedAll(...)` into `NaitrustDbContext.OnModelCreating`. Along the way, found and fixed a real bug in `RoleSeed.cs`: it was seeding the raw `IdentityRoleClaim<Guid>` type instead of the app's actual `NaitrustRoleClaim` entity (which maps to table `RoleClaims`), which would have created a phantom, unused `IdentityRoleClaim<Guid>` table instead of populating `RoleClaims`. Fixed the seed to use `NaitrustRoleClaim`, generated migration `SeedRolesTransactionTypesAdminUserAiPromptVersions` (via a `.NET 8` SDK container, since the host only has `.NET 10` and this project pins 8 via `global.json`), applied it, and verified end-to-end: Roles table has 3 rows, RoleClaims has 16, no phantom table, and a real `/api/auth/register` call successfully created a user auto-assigned the `User` role.

### Phase 2 — stand up both apps together locally ✅ DONE (2026-09-27)
Backend now runs natively via `dotnet run` (not Docker — Docker stack was stopped once staging became the target DB) on `https://localhost:7346`, connected to the **staging Postgres DB** via user secrets (not local Docker Postgres — a deliberate choice: full local-stack-against-staging testing, prod stays untouched). Frontend runs via `npm run dev` on `http://localhost:5184`, `.env` pointed at `VITE_API_BASE_URL=https://localhost:7346`.

Along the way, found and fixed several real bugs blocking end-to-end auth, none of which were in the original plan:
- **CORS + HTTPS-redirect conflict**: `UseHttpsRedirection()` ran before `UseCors(...)` in `Program.cs`, so any request to the `http://` port got 307-redirected before CORS headers were added — browsers refuse to follow a redirected preflight, surfacing as "Unable to reach the server." Fixed by pointing the frontend at the HTTPS port (`7346`) directly and trusting the local dev cert (`dotnet dev-certs https --trust`); also fixed a stray leading-space typo in `Cors:AllowedOrigins`.
- **`isSuccessful` vs `success` field mismatch (app-wide)**: the backend's real contract is `{ statusCode, message, data, isSuccessful }` (confirmed live, and already documented in the frontend's own `backend-api.ts` reference comments), but the frontend's `ApiResponse`/`ApiSuccess` types and ~25 consuming files checked `.success` instead — always `undefined` against the real backend, so every real API call silently hit the frontend's failure branch regardless of actual outcome. Renamed the field end-to-end (types, mocks, fixtures, consumers) and verified with a full `tsc --noEmit` pass (zero errors) rather than relying on grep alone.
- **Login error UX**: a 401 on a public/`skipAuth` call (e.g. login itself) was being handled by the same logic as an expired authenticated session, always showing "Unauthorized: Please login again" instead of the backend's actual message (e.g. "Invalid email or password"). Fixed by branching on `extras.skipAuth` in the shared `request()` handler.
- **Dead `Email:Host`/`Email:Port` config**: `ResendEmailProvider` hardcodes `api.resend.com` and only uses `Email:ApiKey`; removed the unused fields from `appsettings.json`.
- **`App:WebBaseUrl` mismatch**: still points at `http://localhost:3000` (old default) while the frontend actually runs on `5184`, so verification/password-reset email links are currently dead links — **not yet fixed**, worth doing before relying on email links again (OTP-in-URL still works as a manual workaround).

A seeded test account now exists in staging for ongoing testing: `ukpojuojdave12@gmail.com` (role `User`, email verified). Full auth loop confirmed working end-to-end through the real UI: register → verify-email (OTP) → login → dashboard renders.

### Phase 3 — verify + wire the "already aligned" core escrow routes for real (IN PROGRESS)
Deals/Transactions, Negotiation, Termination, Tracking, Invitations, Counterparties, Trust Profile, Reputation, Security (OTP/2FA/KYC/PIN). These were assessed as matching 1:1 from reading code on both sides — this phase is where that gets confirmed against live traffic, one flow at a time, fixing whatever doesn't actually line up.

**Bugs already found and fixed just from the dashboard's first render:**
- `notifications.api.ts` (`GET /notifications`) and `transactions.api.ts` (`getMyTransactions`, `GET /transactions/my`) both assumed the backend returns a bare array in `data`, but the backend actually paginates both (`data: { items, page, pageSize, totalCount, totalPages }`). This crashed `ProtectedDashboardLayout` outright (`data?.filter is not a function`, no error boundary) — login would succeed and navigate but render a blank white screen. Fixed both to unwrap `.items`.
- Confirmed **not universal**: `/api/invitations` returns a bare array (no pagination wrapper) — so this needs checking per-endpoint, not a blanket fix. Any other list endpoint touched during this phase should be spot-checked live (`curl` the real route) before trusting the frontend's existing assumption about its shape.

**Verified working end-to-end through the real UI (2026-09-27):** login → dashboard (Overview, My deals, Invitations nav, notification bell, account menu all render without error).

**Not yet done**: haven't yet clicked into My Deals or Invitations themselves, created a test deal, or exercised Negotiation/Termination/Tracking/Counterparties/Trust Profile/Reputation/Security against live staging data. Pick up here next.

### Phase 4 — delivery-card/handover/release integration
Connect the frontend to the already-built `DealsController` delivery-card/handover/release endpoints, replacing `delivery-review.mock.ts` with real calls. No new backend work — this is the most detailed piece of the escrow flow (10-minute handover / 1-hour review), so it comes after the simpler flows in Phase 3 are proven solid.

### Phase 5 — payment provider factory: Kora + Busha
- Implement `KoraPaymentPartner` (currently every method throws `NotImplementedException`) and a new `BushaPaymentPartner`.
- Register both in `PaymentPartnerFactory.GetPartner` (currently only wires up Anchor — Kora/Busha would hit `NotSupportedException` before reaching their own stubs).
- Add webhook handlers for both in `WebhooksController` (currently Anchor-only, checks `x-anchor-signature`).
- Decide whether Anchor stays as a fallback/sandbox adapter or gets retired.
- Comes after Phase 4 rather than before: funding/release in the deal flow is the actual consumer of payments, so the exact shape of calls needed is clearer once that flow is proven. Also waiting on the user's own study of the Busha API docs.

### Phase 6 — legal review module (net-new build)
No `LegalController`, no `LegalProviders`/`LegalAssignments`/`LegalReviewProposals` tables exist yet. `guardrails/implementation/legal-review-preview.md` already gives a proposed contract (`GET /legal/providers`, `POST /transactions/:id/legal-review`, decisions, payments, assignments, findings) — close to spec-ready, use it as the spec for entities + controller design. Fully separate from the escrow core, so it doesn't block or get blocked by anything above — lowest priority.

---

## Verification approach (per phase)

- Backend: run the relevant xUnit tests under `naitrust-api/tests/Naitrust.UnitTests`; add/extend validator tests alongside any new request DTO; confirm the controller compiles and Swagger reflects the new shape.
- Frontend: flip `appConfig.isMock` off locally (or point `VITE_API_BASE_URL` at the local API) for the affected page and confirm the real network call succeeds end-to-end instead of falling into the mock branch.
- For any new migration: `dotnet ef migrations add <Name>` in `naitrust-api`, review the generated migration before applying.
- Do not implement any phase until reviewed and confirmed — this is a plan-then-execute workflow.

---

## Relationship to P1

P1's Tier 3/Tier 4 items (Delivery Card/Handover/Funding Review, Deal Identity Capture, Bills/VAS, Business Reputation, Business Reviews, Trust Checkouts, Agreements AI-assist) are superseded or re-scoped by this document:
- Delivery Card/Handover — **superseded**: already built (see "Already aligned" above), just needs frontend integration (Phase 3), not new backend design.
- Bills, Trust Checkouts — **carried forward unchanged** as deferred items.
- Business Reputation, Business Reviews, Agreements AI-assist — **out of current scope**: these belonged to marketplace/wallet-adjacent surfaces that were retired in the pivot; not reintroduced here unless product brings them back.

P1's Tier 1/Tier 2 items (deal response field mapping, invitations naming fix, business response gap, staged payments, dispute updates, agreement draft fields) were **not re-verified** in this pass and are not addressed by this document — they should be re-checked against the current frontend before being treated as still-open, since some may have shifted or already landed during the pivot.

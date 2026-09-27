# Backend API Contract

> Updated to match the real, implemented `NaitrustResponse<T>` envelope
> (`Naitrust.Domain.Models.Dtos.Common.ApiResponse.cs`) and the actual controllers in
> `Naitrust.Api/Controllers/`. The previous version of this doc described an aspirational
> `{success, data, error}` shape that was never implemented — every controller action either
> already uses `NaitrustResponse<T>` (`PublicController`) or is a stub (`NotImplementedException`)
> that will use the same shared helper once built, since it's shared app-wide infrastructure, not
> per-controller.

Use JSON over HTTPS.

## Implementation Status (important — read before building against this doc)

**Only `PublicController` (`/api/Public/*` — joinWaitlist, contactUs, subscribe, submitFeedback,
reportConcern) is actually implemented.** Every other controller listed below —
Auth, Users, Businesses, Transactions, Milestones, Evidence, Payments, Disputes, Verification,
Reputation, Admin, AI, Notifications, Webhooks — exists only as route-attributed method stubs that
`throw new NotImplementedException()`. The routes are scaffolded (real `[Route]`/`[Http*]`
attributes), so they're a solid starting shape, but no request/response body, validation, or
persistence logic exists yet behind them.

**Known mismatch with the frontend (`naitrust-web`) worth resolving before building these out:**
the frontend's `src/libs/api/endpoints.ts` already calls transaction-related paths that do **not**
match these stubs for overlapping concepts — e.g. the frontend calls
`/transactions/:id/messages` (chat), `/transactions/:id/tracking/advance` (step-based progress),
`/transactions/:id/negotiation/propose` (term proposals), and `/transactions/:id/termination`,
none of which exist here. This backend instead stubs a simpler lifecycle-action model:
`/transactions/:id/invite`, `/accept`, `/reject`, `/terms`, `/approve-terms`, `/fund`, `/deliver`,
`/confirm`, `/cancel`. Since nothing is implemented on either side of that gap yet, this is a
product/architecture decision to make deliberately (which model to build) rather than something to
silently reconcile — flagging it here so it doesn't get built twice in two different shapes.

## Response Envelope

Every endpoint returns `NaitrustResponse<T>`:

```json
{
  "statusCode": 200,
  "message": "Request successful",
  "data": {},
  "isSuccessful": true
}
```

Error (4xx/5xx) — same shape, `data` is typically `null`:

```json
{
  "statusCode": 422,
  "message": "Human readable error",
  "data": null,
  "isSuccessful": false
}
```

Construct these via the shared `NaitrustResponse<T>` / `NaitrustResponse` static factory methods
(`Success`, `Created`, `BadRequest`, `Unauthorized`, `Forbidden`, `NotFound`, `Conflict`,
`UnprocessableEntity`, `InternalServerError`, etc.) — do not hand-roll the envelope per controller.

## Endpoint Groups

### Auth

- `POST /auth/register`
- `POST /auth/login`
- `POST /auth/logout`
- `GET /auth/me`
- `POST /auth/verify-email`
- `POST /auth/forgot-password`
- `POST /auth/reset-password`

### Users and Businesses

- `GET /users/me`
- `PATCH /users/me`
- `POST /businesses`
- `GET /businesses/me`
- `GET /businesses/:id`
- `PATCH /businesses/:id`
- `POST /businesses/:id/members`
- `PATCH /businesses/:id/members/:memberId`

### Transactions

- `POST /transactions`
- `GET /transactions`
- `GET /transactions/:id`
- `PATCH /transactions/:id`
- `GET /transaction-types`
- `POST /transactions/:id/invite`
- `POST /transactions/:id/accept`
- `POST /transactions/:id/reject`
- `POST /transactions/:id/terms`
- `POST /transactions/:id/approve-terms`
- `POST /transactions/:id/fund`
- `POST /transactions/:id/deliver`
- `POST /transactions/:id/confirm`
- `POST /transactions/:id/cancel`

### Milestones

Milestone endpoints are Phase 2 and should not block Phase 1 domestic single-release build.

- `POST /transactions/:id/milestones`
- `PATCH /transactions/:id/milestones/:milestoneId`
- `POST /transactions/:id/milestones/:milestoneId/submit`
- `POST /transactions/:id/milestones/:milestoneId/approve`

### Evidence

- `POST /transactions/:id/evidence`
- `GET /transactions/:id/evidence`
- `GET /evidence/:id`

### Protected Funding and Payments

- `POST /transactions/:id/virtual-account`
- `GET /transactions/:id/payment-status`
- `POST /transactions/:id/request-release`
- `POST /webhooks/payment-partners/:partner/funding`
- `POST /webhooks/payment-partners/:partner/transfer`
- `POST /payment-partners/:partner/validate-payout-account`
- `GET /transactions/:id/ledger`
- `GET /transactions/:id/reconciliation-status`

Webhook endpoints require provider signature verification and idempotency.

### Disputes

- `POST /transactions/:id/disputes`
- `GET /transactions/:id/disputes`
- `GET /disputes/:id`
- `POST /disputes/:id/messages`
- `POST /disputes/:id/evidence`

### Verification

- `POST /verification/start`
- `GET /verification/status`
- `POST /verification/individual`
- `POST /verification/business`
- `POST /verification/:requestId/facial`
- `POST /verification/:requestId/documents`
- `POST /verification/:requestId/ownership`
- `POST /verification/:requestId/verify-code`
- `GET /verification/requests/:id`
- `POST /verification/requests/:id/run`
- `POST /verification/requests/:id/request-more-info`

### Reputation

- `GET /reputation/:profileId`
- `GET /reputation/me`
- `POST /transactions/:id/reviews`

### Admin

- `GET /admin/transactions`
- `GET /admin/transactions/:id`
- `GET /admin/disputes`
- `PATCH /admin/disputes/:id`
- `GET /admin/verifications`
- `PATCH /admin/verifications/:id`
- `GET /admin/audit-logs`

### AI Intelligence

- `POST /ai/transactions/:id/risk-assessment`
- `POST /ai/transactions/:id/evidence-checklist`
- `POST /ai/disputes/:id/summary`
- `POST /ai/verifications/:id/summary`
- `POST /ai/reputation/:profileId/summary`
- `POST /ai/admin/cases/:id/copilot`
- `POST /ai/feedback`

## Authorization Rules

- Users can see transactions where they are a party.
- Business members can see business transactions according to role.
- Admins can see all transactions.
- Only eligible transaction parties can accept terms, submit evidence, approve delivery, or open disputes.
- Only admins can resolve disputes.

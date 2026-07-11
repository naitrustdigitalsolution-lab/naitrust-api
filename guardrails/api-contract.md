# Backend API Contract

Use JSON over HTTPS.

## Response Envelope

Success:

```json
{
  "success": true,
  "data": {},
  "message": "Optional message"
}
```

Error:

```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Human readable error",
    "details": {}
  }
}
```

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

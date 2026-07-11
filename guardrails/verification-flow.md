# Backend Verification Flow

Verification is a core risk-control layer for Naitrust. It supports trusted transactions but does not replace legal, payment, or dispute controls.

Use old backend references:

- `../../naitrust-api-old/src/services/third-party/qoreid.service.ts`
- `../../naitrust-api-old/src/services/verification`
- `../../naitrust-api-old/src/db/schema/verification.ts`
- `../../naitrust-api-old/src/routes/verification.routes.ts`
- `../../naitrust-api-old/src/controllers/verification.controller.ts`

## Provider Direction

QoreID should be the primary reference provider if the old implementation is still valid.

Old QoreID-supported concepts included:

- CAC verification.
- BVN verification.
- BVN face verification.
- facial verification against supported ID types.
- business assurance testing.

The new backend must wrap QoreID behind an adapter so the domain does not depend directly on provider-specific response shapes.

## Verification Domains

### Individual Verification

Purpose:

- prove the user is a real person.
- reduce fake buyer/seller accounts.
- support business owner/director checks.
- support higher-risk safe deals.

Data may include:

- legal name.
- date of birth.
- phone.
- email.
- ID type: NIN, BVN, passport, driver's license, or other provider-supported ID.
- ID number.
- selfie image.
- face verification result.

Statuses:

- not_started
- pending
- payment_pending
- processing
- verified
- needs_more_info
- manual_review
- rejected
- expired

### Business Verification

Purpose:

- confirm CAC registration details.
- confirm business existence and status.
- identify directors/proprietors where provider data supports it.
- connect the Naitrust user to the business through ownership proof.

Data may include:

- CAC type: RC, BN, IT, LLP.
- registration number.
- legal business name.
- TIN.
- registered address.
- directors/proprietors.
- CAC certificate.
- proof of address.
- tax certificate.

Statuses are the same as individual verification.

### Facial Verification

Purpose:

- prove the applicant is present.
- match selfie to BVN/NIN/passport/driver's license where provider supports it.
- support manual fallback with selfie and selfie-with-ID evidence.

Backend rules:

- frontend only captures images.
- backend validates file type and size.
- backend stores images securely.
- backend calls provider or queues admin review.
- backend stores normalized match result and raw provider payload separately.

### Ownership Verification

Purpose:

- prove the verified user controls or represents the verified business.

Methods:

- identity match with CAC director/proprietor.
- CAC email/phone OTP.
- bank account ownership check through regulated partner.
- manual review.

## Payment-First vs Risk-First

If verification is paid, follow payment-first:

1. Create verification request.
2. Create provider funding/payment request.
3. Wait for signed webhook confirmation.
4. Run verification.
5. Store result.

If verification is required for a high-risk transaction and not separately paid, follow risk-first:

1. Create verification request linked to transaction.
2. Run required checks.
3. Block risky transaction actions until verification is complete.

## Verification Reuse and Freshness Policy

Do not force users or businesses to repeat full verification for every transaction.

The backend should maintain reusable verification state:

- `users.identity_verified_at`
- `users.last_liveness_verified_at`
- `users.last_transaction_activity_at`
- `businesses.business_verified_at`
- `businesses.ownership_verified_at`
- `businesses.verification_expires_at`
- `verification_requests.expires_at`

Reusable checks:

- email verification.
- phone verification.
- individual ID verification.
- business/CAC verification.
- ownership proof for the same business.
- approved manual verification.

Re-verification triggers:

- expired verification.
- changed identity or business details.
- high-risk transaction category.
- amount exceeds risk threshold.
- provider mismatch.
- admin flag.
- fraud/dispute flag.
- account recovery or suspicious login.

### Liveness Freshness Rule

Liveness is freshness-based. A valid identity verification does not always mean the user is currently present.

Default rule:

> Require fresh liveness if the user has had no completed deal or meaningful transaction activity for more than 30 days.

The risk engine may also require fresh liveness for:

- high-value transactions.
- new device or suspicious session.
- account recovery.
- dispute escalation.
- payment release request on a high-risk transaction.
- admin decision.

Backend behavior:

- check reusable verification before creating a new verification request.
- create a liveness-only request when identity is still valid but freshness is stale.
- store liveness results separately from identity verification.
- never reset full verification status just because liveness is stale.
- return `requiredVerificationActions` to the frontend.

## Verification Request Lifecycle

1. User starts verification.
2. Backend stores request as pending.
3. Backend collects consent and required fields.
4. Backend optionally initializes payment.
5. Backend verifies payment through webhook if payment is required.
6. Backend calls provider adapter or starts manual review.
7. Backend stores normalized result.
8. Backend updates user/business verification status.
9. Backend emits real-time status event.
10. Backend writes audit logs.

## Manual Review Lifecycle

Use manual review when:

- provider is unavailable.
- provider result conflicts with submitted data.
- face match fails.
- ownership cannot be proven automatically.
- transaction value or risk level requires human review.

Admin actions:

- approve.
- reject.
- request more information.
- run face match.
- override with reason.

Every admin action must be audit logged.

## Suggested Tables

Use the main `database-design.md` as source of truth, but ensure it supports:

- verification_requests.
- verification_subjects or subject_type/subject_id fields.
- verification_steps.
- verification_documents.
- verification_provider_events.
- face_match_results.
- ownership_checks.
- manual_review_notes.

## Events

Emit SignalR events for:

- `verification.request.created`
- `verification.payment.confirmed`
- `verification.step.updated`
- `verification.manual_review.required`
- `verification.completed`
- `verification.rejected`
- `verification.more_info_requested`

Events are notifications only. Database state remains source of truth.

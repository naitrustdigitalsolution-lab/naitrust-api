# Backend Workflow

## Safe Deal Lifecycle

1. User creates a draft transaction.
2. Backend creates transaction reference.
3. User selects a party mode: `b2b` or `b2c`.
4. User selects a `TransactionType`; Phase 1 defaults are `domestic_single_release` and `domestic_b2c_single_release`.
5. Backend sends invitation.
6. Counterparty accepts invitation.
7. Both parties propose and accept transaction terms.
8. Agreement is frozen and becomes the release/dispute source of truth.
9. Backend creates a per-transaction virtual account or collection account through Providus Bank in development.
10. Buyer/customer funds the partner-issued virtual account.
11. Partner confirms funding through signed webhook.
12. Backend posts balanced ledger entries and marks the transaction funded.
13. Seller/vendor/service provider delivers and uploads required evidence.
14. Buyer/customer confirms delivery or auto-confirm window elapses.
15. Backend validates release conditions and payout account name match.
16. Backend sends signed release instruction to partner.
17. Partner confirms payout to verified seller/vendor/service-provider account.
18. Backend posts ledger entries, updates reputation, completes transaction, and writes audit records.

## Party Modes

The same engine must support:

- `b2b`: business to business, such as SME to supplier, contractor, wholesaler, distributor, vendor, agent, or service provider.
- `b2c`: individual customer to business/vendor/service provider, such as customer to event vendor, renter to agent, buyer to verified high-value seller, or homeowner to contractor.

B2C is not a broad consumer payment flow. It must still use agreement terms, protected funding, evidence, confirmation/release, disputes, verification, and reputation.

## Approved Transaction State Machine

Only the Transaction Orchestrator may change transaction state.

- `DRAFT` -> `PENDING_COUNTERPARTY`, `CANCELLED`
- `PENDING_COUNTERPARTY` -> `TERMS_NEGOTIATION`, `CANCELLED`
- `TERMS_NEGOTIATION` -> `TERMS_AGREED`, `CANCELLED`
- `TERMS_AGREED` -> `AWAITING_FUNDING`
- `AWAITING_FUNDING` -> `FUNDED`, `CANCELLED`
- `FUNDED` -> `IN_PROGRESS`
- `IN_PROGRESS` -> `EVIDENCE_SUBMITTED`, `DISPUTED`
- `EVIDENCE_SUBMITTED` -> `BUYER_REVIEW`
- `BUYER_REVIEW` -> `RELEASE_APPROVED`, `DISPUTED`
- `RELEASE_APPROVED` -> `PAID_OUT`
- `DISPUTED` -> `RELEASE_APPROVED`, `REFUNDED`
- `PAID_OUT` -> `COMPLETED`
- `REFUNDED` -> `COMPLETED`
- `CANCELLED` terminal
- `COMPLETED` terminal

## Dispute Lifecycle

1. A party opens a dispute from a transaction.
2. Backend freezes release actions where required.
3. Backend records reason and evidence.
4. Admin reviews timeline, terms, payment status, and evidence.
5. Admin requests more evidence if needed.
6. Admin resolves as release, refund, split, or close.
7. Backend calls partner action if money movement is required.
8. Backend records final resolution.
9. Backend updates transaction, reputation, notifications, and audit logs.

## Payment Partner Workflow

The backend owns partner coordination.

Rules:

- Create virtual account only after terms are accepted and the agreement is frozen.
- Providus Bank is the development adapter.
- Kora/Korapay, Wema Bank, and Anchor are future adapter placeholders.
- Confirm funds only from signed provider webhook or trusted provider API polling.
- Store raw webhook payload in `payment_partner_events`.
- Process each provider event once.
- Do not release payment while a dispute is open.
- Do not trust frontend callbacks as final payment state.
- Every money-impacting event must create balanced double-entry ledger postings.
- Reconciliation must compare ledger balances against partner virtual-account balances.
- Payout is allowed only to a bank account whose name matches the verified seller identity or approved business identity.

## Verification Workflow

Verification must be risk-based.

Low-risk informal transaction:

- phone/email check.
- basic identity where needed.

Phase 1 domestic high-value transaction:

- user identity verification.
- business registration verification.
- B2B or B2C party-mode risk assessment.
- seller payout-account name matching.
- manual admin review for mismatches.

Very high-risk transaction:

- enhanced due diligence.
- stricter evidence requirement.
- manual approval before virtual account issuance or release.

## Admin Workflow

Admin actions should be audit logged.

Admin queues:

- pending verification.
- risky transactions.
- payment exceptions.
- open disputes.
- suspicious evidence.
- reported users/businesses.

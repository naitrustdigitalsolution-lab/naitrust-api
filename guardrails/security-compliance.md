# Security and Compliance Notes

## Regulatory Posture

Naitrust should operate as the transaction workflow, evidence, dispute, verification, and reputation layer.

Money movement and fund custody should be handled by a regulated bank, payment provider, or licensed financial partner.

Approved Phase 1 custody rule:

> Naitrust orchestrates the transaction; it never takes custody of funds.

Implementation constraints:

- Use per-transaction virtual accounts issued by the licensed partner.
- Do not pool third-party funds in a Naitrust-controlled account.
- Naitrust sends signed instructions; the partner executes money movement.
- Internal ledger mirrors partner-held balances but does not represent Naitrust custody.
- Release is only allowed through the Transaction Orchestrator.

## Required Legal Review

Before launch, confirm:

- whether Naitrust can use the word "escrow" in product and marketing.
- who legally holds customer funds.
- who performs KYC and AML/CFT checks.
- who is responsible for fraud loss.
- how disputes are resolved.
- what happens on refund, split release, and chargeback.
- what disclosures must be shown to users.
- whether any CBN, SEC, FCCPC, NDPA, or other regulatory obligations apply.
- whether Providus Bank explicitly permits protected-transaction flows for Phase 1 development and launch.
- whether Kora/Korapay, Wema Bank, Anchor, or other future partners permit the same flow.

## Security Controls

- JWT auth with refresh strategy.
- rate limiting on auth, invitations, uploads, payment initiation, and disputes.
- provider webhook signature validation.
- idempotency keys for write-heavy and payment endpoints.
- double-entry ledger balancing.
- scheduled reconciliation against partner virtual-account balances.
- audit logs for all sensitive transitions.
- role-based access control for admin and business team actions.
- file type and size validation.
- malware scanning if evidence upload risk increases.
- encryption in transit.
- careful secret management.

## Data Protection

Treat these as sensitive:

- identity documents.
- selfie and facial verification images.
- face match results.
- BVN, NIN, passport, driver's license, and other ID numbers.
- business registration documents.
- bank details.
- payment references.
- dispute evidence.
- private transaction terms.
- AI assessments that summarize private transactions, disputes, verification, or risk.

Only collect what is needed for the transaction and verification level.

## Compliance As Design Constraints

- KYC before funding: both parties must satisfy the required verification level before funding.
- AML monitoring: transactions should be monitored for suspicious patterns.
- SCUML: Naitrust should maintain SCUML registration if legally required for the operating model.
- NDPA/NDPR: BVN and personal data require consent, minimization, encryption at rest, and access logs.
- Partner terms: selected partner must explicitly permit protected-transaction or escrow-style flows on its rails.
- Payout control: seller payout account must name-match verified identity or approved business identity.

AI-specific protection:

- redact sensitive data before model calls where possible.
- keep OpenAI calls server-side.
- log prompt version and model version.
- do not allow AI to make final verification, dispute, compliance, or payment decisions.
- do not expose internal risk summaries directly to counterparties without filtering.

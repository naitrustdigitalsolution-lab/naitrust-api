# Domain Glossary

## Safe Deal

A structured transaction between parties with agreed terms, evidence, protected payment status, and completion/dispute flow.

## Transaction Room

The API-backed object that aggregates transaction, parties, terms, milestones, evidence, payment status, disputes, activity, and allowed actions.

## Protected Payment

Payment flow handled by a regulated bank or payment partner. Backend stores partner references and statuses; it does not imply Naitrust custody.

## Custody Firewall

The hard boundary that Naitrust never holds pooled customer funds. Funds sit in partner-issued per-transaction virtual accounts. Naitrust stores state and sends instructions.

## Transaction Orchestrator

The only backend service allowed to change transaction state or authorize release/refund instructions.

## TransactionType

A configuration template that defines verification level, evidence requirements, release mode, dispute rules, fee model, and auto-confirm window.

Phase 1 starts with `domestic_single_release`.

## VirtualAccount

A partner-issued account dedicated to one transaction. In development, this should be created through the Providus Bank adapter.

## LedgerEntry

An immutable double-entry posting that records the financial truth of funding, fee recognition, payout, refund, or reconciliation.

## PaymentInstruction

A signed instruction sent to the licensed partner to release, refund, split, or sweep fees. Naitrust sends instructions; the partner moves money.

## Protected Funding Request

A generic request to a partner to begin protected funding. For Phase 1 transaction funding, prefer the more precise `VirtualAccount` model.

## Release Request

A backend-created request to the partner to release funds according to agreed terms and dispute state.

## Verification

Risk-control process for users and businesses. Includes individual identity, business/CAC, facial/liveness, ownership, document, and manual review checks.

## Liveness

Fresh proof that the verified person is present now. Stored separately from reusable identity verification.

## Ownership Check

Backend record proving whether a user controls or represents a business.

## Evidence

Immutable or append-only supporting material attached to transactions, milestones, or disputes.

## Dispute

A formal transaction conflict. Disputes block unsafe release actions until resolved by rules or admin action.

## Reputation

Aggregated trust record from completed safe deals, reviews, dispute history, and transaction categories.

## AI Assessment

Stored AI output with model, prompt version, input references, confidence, and structured recommendations. Advisory only.

## Allowed Actions

Backend-computed action permissions for the current user and transaction state. The frontend uses this to enable or disable major actions.

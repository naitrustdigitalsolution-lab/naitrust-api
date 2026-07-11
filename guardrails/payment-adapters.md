# Payment and Bank Adapter Guardrail

This file defines how Naitrust integrates with licensed payment and bank partners.

Approved Phase 1 direction comes from `../../Naitrust Technical Spec v2.docx`.

## Custody Boundary

Naitrust is an orchestrator, not a custodian.

Rules:

- Naitrust never holds pooled customer funds.
- Each funded transaction uses a partner-issued virtual account dedicated to that transaction.
- Funds legally sit within the regulated partner environment.
- Naitrust stores transaction state, agreement, evidence, ledger postings, audit trail, and signed instructions.
- Naitrust sends instructions to the partner; the partner moves money.
- Every release/refund/payout action must go through the Transaction Orchestrator and Payment Adapter.

## Development Partner

Use Providus Bank as the development payment adapter.

Naming:

- Use `providus` as the internal adapter id.
- User-facing docs may say `Providus Bank` where needed.
- Do not hardcode Providus into domain services; depend on the `IPaymentPartner` interface.

Providus adapter should support:

- create or assign a per-transaction collection account using Providus Digital Collection Services where approved.
- receive funding webhook.
- verify webhook signature.
- query virtual account/funding status.
- integrate Providus Transfer Services for release/refund instructions where approved.
- perform name enquiry or bank account validation where provider supports it.
- execute release/transfer to verified seller payout account.
- execute refund to buyer where supported.
- return normalized partner errors.

Implementation sources:

- Providus developer portal: `https://developer.providusbank.com/`
- Digital Collection Services for Pay with Transfer / collection account flows.
- Transfer Services for secure third-party partner transfers.
- Sandbox/API Reference for authentication, request format, webhook signatures, credentials, and test tooling.

Do not infer unsupported endpoint behavior. If the public docs or credentials are unavailable for a required method, implement the `IPaymentPartner` method with a mocked provider test and mark the live integration as blocked.

## Future Adapter Placeholders

Create adapter placeholders for:

- Kora/Korapay.
- Wema Bank.
- Anchor.
- Other licensed bank/PSP partners.

Do not implement production logic for placeholder adapters until credentials, documentation, and permitted use cases are confirmed.

## Payment Partner Interface

All partners must implement the same port.

Suggested C# interface:

```csharp
public enum PaymentPartnerId
{
    Providus,
    Kora,
    Wema,
    Anchor
}

public interface IPaymentPartner
{
    Task<CreateVirtualAccountResult> CreateVirtualAccountAsync(CreateVirtualAccountRequest request, CancellationToken cancellationToken);
    Task<VerifiedWebhookEvent> VerifyWebhookAsync(VerifyWebhookRequest request, CancellationToken cancellationToken);
    Task<FundingStatusResult> GetFundingStatusAsync(FundingStatusRequest request, CancellationToken cancellationToken);
    Task<PayoutAccountValidationResult> ValidatePayoutAccountAsync(ValidatePayoutAccountRequest request, CancellationToken cancellationToken);
    Task<PaymentInstructionResult> ReleaseFundsAsync(ReleaseFundsRequest request, CancellationToken cancellationToken);
    Task<PaymentInstructionResult> RefundFundsAsync(RefundFundsRequest request, CancellationToken cancellationToken);
}
```

## Required Normalized Objects

### VirtualAccount

- transaction id.
- partner id.
- partner account reference.
- bank name.
- account number.
- account name.
- status.
- expires at if applicable.

### FundingWebhook

- partner id.
- event id.
- event type.
- virtual account reference.
- amount in minor units.
- currency.
- paid by metadata where available.
- raw payload.

### PaymentInstruction

- instruction id.
- instruction type: release, refund, split, fee_sweep.
- partner id.
- idempotency key.
- status.
- signed payload hash.
- partner response.

## Double-Entry Ledger Requirements

Every money-impacting event must create balanced ledger postings in one database transaction.

Required ledger event types:

- virtual account funded.
- release approved.
- platform fee recognized.
- seller payout executed.
- buyer refund executed.
- split resolution executed.
- fee swept.
- reconciliation adjustment.

Amounts must be stored as integer minor units, such as kobo.

Never use floating point numbers for money.

## Reconciliation

Add scheduled reconciliation through Hangfire.

The job must compare:

- internal ledger balance.
- transaction expected balance.
- partner virtual account balance/status.

If mismatch occurs:

- mark transaction as reconciliation_blocked.
- block release.
- create admin alert.
- write audit event.

## Payout Account Name Matching

Before release:

- seller identity or business must be verified.
- seller payout account must be validated.
- account name must match verified seller identity or approved business name under deterministic rules.
- failed match sends transaction to manual review.

This is the anti-mule control.

## Webhook Rules

- Verify signature before processing.
- Store raw event.
- Process each provider event once.
- Use idempotency keys.
- Never trust frontend payment redirects.
- Never release funds based only on frontend state.

## Tests Required

- Providus adapter unit tests with mocked HTTP responses.
- placeholder adapter tests proving unavailable providers cannot be selected as live providers.
- webhook signature tests.
- duplicate webhook tests.
- virtual account creation tests.
- funding state transition tests.
- ledger balancing tests.
- reconciliation mismatch tests.
- payout account name-match tests.
- release/refund instruction idempotency tests.

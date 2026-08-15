> **Superseded — see `P1-API-ALIGNMENT-PLAN.md`.** This early scaffolding plan no longer reflects the current backend state; kept for historical reference only.

# Naitrust API — Service Implementation Plan

## Context
The Naitrust API has a fully scaffolded backend: 17 service interfaces (94 methods), 18 service implementations (17 are stubs), 15 controllers (14 commented out), 42 request DTOs, 38 response DTOs, 41 validators (all TODO stubs), and a complete Identity + EF Core database layer. Only `PublicFormService` and `PublicController` are implemented.

## Phase 0: Documentation & Scaffolding — DONE
- [x] 0A: XML doc comments on all 17 service interfaces
- [x] 0B: Uncomment & wire all 14 controllers
- [x] 0C: DTO `Role` → `Roles` (IList<string>)
- [x] 0D: This plan file

## Phase 1: Auth Foundation — NOW
- [ ] 1A: TokenService (JWT generation, refresh token management)
- [ ] 1B: AuthService (register, login, logout, profile, email verify, password reset, token refresh)

## Phase 2: User & Business — NEXT
- [ ] 2A: UserService (get/update user)
- [ ] 2B: BusinessService (CRUD business, manage members)

## Phase 3: Transactions Core — NEXT
- [ ] 3A: TransactionService (CRUD, types)
- [ ] 3B: TransactionOrchestrator (state machine)
- [ ] 3C: PartyService

## Phase 4: Payments & Ledger — LATER
- [ ] 4A: LedgerService (double-entry accounting)
- [ ] 4B: PaymentService

## Phase 5: Verification — LATER
- [ ] VerificationService (13 methods, KYC/AML)

## Phase 6: Disputes & Evidence — LATER
- [ ] DisputeService
- [ ] EvidenceService

## Phase 7: Reputation & Notifications — LATER
- [ ] NotificationService
- [ ] ReputationService

## Phase 8: Admin & Audit — LATER
- [ ] AdminService
- [ ] AuditLogService

## Phase 9: AI Intelligence — DEFER
- [ ] AiIntelligenceService (stub until OpenAI configured)

## Phase 10: Validators — DEFER
- [ ] All 41 FluentValidation validators

## State Machine (Transaction Orchestrator)
```
Draft → PendingCounterparty       (InviteParty)
PendingCounterparty → TermsNeg   (AcceptInvitation)
PendingCounterparty → Cancelled   (RejectInvitation)
TermsNegotiation → AwaitingFunding (ApproveTerms)
AwaitingFunding → Funded          (after payment confirmed)
Funded → DeliveryInProgress       (SubmitDelivery)
DeliveryInProgress → Completed    (ConfirmDelivery)
Any active → Cancelled            (Cancel)
```

## Verification Checklist
After each phase: `dotnet build` to confirm 0 errors.

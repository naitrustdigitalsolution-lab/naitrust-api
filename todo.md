> **Superseded — see `P1-API-ALIGNMENT-PLAN.md`.** This early scaffolding todo list no longer reflects the current backend state; kept for historical reference only.

# Naitrust API — Implementation Todo

Everything below is scaffolded but not yet implemented (throws `NotImplementedException` or has empty `// TODO` bodies).

---

## 1. Entity Configurations (36 files)

Define table constraints, indexes, relationships, and column types via `IEntityTypeConfiguration<T>`.

Location: `src/Naitrust.Domain/Configurations/EntityConfigurations/`

- [ ] UserConfiguration
- [ ] BusinessConfiguration
- [ ] BusinessMemberConfiguration
- [ ] PartyConfiguration
- [ ] TransactionConfiguration
- [ ] TransactionTypeConfiguration
- [ ] TransactionPartyConfiguration
- [ ] AgreementConfiguration
- [ ] MilestoneConfiguration
- [ ] EvidenceFileConfiguration
- [ ] VirtualAccountConfiguration
- [ ] PaymentPartnerEventConfiguration
- [ ] LedgerEntryConfiguration
- [ ] PaymentInstructionConfiguration
- [ ] ReleaseRequestConfiguration
- [ ] PayoutAccountConfiguration
- [ ] DisputeConfiguration
- [ ] DisputeMessageConfiguration
- [ ] DisputeEvidenceConfiguration
- [ ] ReputationProfileConfiguration
- [ ] ReviewConfiguration
- [ ] NotificationConfiguration
- [ ] OwnershipCheckConfiguration
- [ ] FaceMatchResultConfiguration
- [ ] VerificationRequestConfiguration
- [ ] VerificationStepConfiguration
- [ ] VerificationDocumentConfiguration
- [ ] VerificationProviderEventConfiguration
- [ ] RefreshTokenConfiguration
- [ ] OutboxMessageConfiguration
- [ ] IdempotencyKeyConfiguration
- [ ] AuditLogConfiguration
- [ ] AiAssessmentConfiguration
- [ ] AiFeedbackConfiguration
- [ ] AiPromptVersionConfiguration
- [ ] VectorDocumentConfiguration

---

## 2. Service Implementations (17 files)

All throw `NotImplementedException`. Location: `src/Naitrust.Application/Services/Implementations/`

- [ ] AuthService — Register, Login, Logout, VerifyEmail, ForgotPassword, ResetPassword, RefreshToken, GetCurrentUser
- [ ] TokenService — JWT token generation and validation
- [ ] UserService — GetUser, UpdateUser
- [ ] BusinessService — CRUD + member management
- [ ] TransactionService — Create, Get, List, Update, GetTransactionTypes
- [ ] TransactionOrchestrator — invite, accept, reject, propose terms, approve terms, fund, deliver, confirm, cancel
- [ ] PartyService — party management for transactions
- [ ] PaymentService — CreateVirtualAccount, GetPaymentStatus, RequestRelease, GetLedger, Reconciliation, ValidatePayoutAccount
- [ ] LedgerService — double-entry ledger operations
- [ ] VerificationService — individual, business, facial, ownership verification flows
- [ ] DisputeService — open, message, evidence, resolve disputes
- [ ] EvidenceService — upload and manage evidence files
- [ ] ReputationService — profiles and reviews
- [ ] NotificationService — send, list, mark-read notifications
- [ ] AdminService — admin queues, review, resolve
- [ ] AuditLogService — query audit logs
- [ ] AiIntelligenceService — risk scoring, dispute summary, evidence checklist (post-MVP)

---

## 3. Controllers (14 files)

All throw `NotImplementedException`. Location: `src/Naitrust.Api/Controllers/`

- [ ] AuthController (7 endpoints)
- [ ] UsersController
- [ ] BusinessesController
- [ ] TransactionsController (13 endpoints)
- [ ] MilestonesController
- [ ] PaymentsController (6 endpoints)
- [ ] VerificationController
- [ ] DisputesController
- [ ] EvidenceController
- [ ] ReputationController
- [ ] NotificationsController
- [ ] AdminController
- [ ] AiController
- [ ] WebhooksController

---

## 4. External Services (12 files)

All throw `NotImplementedException`. Location: `src/Naitrust.Application/ExternalServices/`

- [ ] ProvidusPaymentPartner — Providus Bank API integration
- [ ] AnchorPaymentPartner — Anchor payment adapter
- [ ] KoraPaymentPartner — Kora payment adapter
- [ ] WemaPaymentPartner — Wema payment adapter
- [ ] PaymentPartnerFactory — partner selection logic
- [ ] ProvidusWebhookValidator — Providus-specific webhook validation
- [ ] QoreIdVerificationProvider — QoreId KYC/verification API
- [ ] ImageKitStorageService — file upload and storage
- [ ] RedisCacheService — distributed caching operations
- [ ] EmailService — email sending
- [ ] TermiiSmsService — SMS sending via Termii
- [ ] OpenAiProviderService — OpenAI API calls (post-MVP)

---

## 5. Validators (29 files)

All have empty constructors with `// TODO: Add validation rules`. Location: `src/Naitrust.Application/Validators/`

### Auth (6)
- [ ] RegisterRequestValidator
- [ ] LoginRequestValidator
- [ ] RefreshTokenRequestValidator
- [ ] VerifyEmailRequestValidator
- [ ] ForgotPasswordRequestValidator
- [ ] ResetPasswordRequestValidator

### Businesses (4)
- [ ] CreateBusinessRequestValidator
- [ ] UpdateBusinessRequestValidator
- [ ] AddBusinessMemberRequestValidator
- [ ] UpdateBusinessMemberRequestValidator

### Transactions (6)
- [ ] CreateTransactionRequestValidator
- [ ] UpdateTransactionRequestValidator
- [ ] InvitePartyRequestValidator
- [ ] ProposeTermsRequestValidator
- [ ] CreateMilestoneRequestValidator
- [ ] UpdateMilestoneRequestValidator

### Payments (3)
- [ ] CreateVirtualAccountRequestValidator
- [ ] RequestReleaseValidator
- [ ] ValidatePayoutAccountRequestValidator

### Disputes (4)
- [ ] OpenDisputeRequestValidator
- [ ] ResolveDisputeRequestValidator
- [ ] AddDisputeMessageRequestValidator
- [ ] AddDisputeEvidenceRequestValidator

### Verification (7)
- [ ] StartVerificationRequestValidator
- [ ] IndividualVerificationRequestValidator
- [ ] BusinessVerificationRequestValidator
- [ ] FacialVerificationRequestValidator
- [ ] OwnershipVerificationRequestValidator
- [ ] UploadVerificationDocumentRequestValidator
- [ ] VerifyCodeRequestValidator

### Other (3)
- [ ] UploadEvidenceRequestValidator
- [ ] SubmitReviewRequestValidator
- [ ] AiFeedbackRequestValidator
- [ ] UpdateUserRequestValidator
- [ ] ResolveAdminDisputeRequestValidator
- [ ] UpdateAdminVerificationRequestValidator

---

## 6. Webhook Handlers (2 files)

Location: `src/Naitrust.Application/Webhooks/`

- [ ] PaymentWebhookHandler — process payment partner webhooks
- [ ] VerificationWebhookHandler — process verification provider webhooks

---

## 7. Background Jobs (7 files)

Location: `src/Naitrust.Application/BackgroundJobs/`

- [ ] ReconciliationJob — hourly payment reconciliation
- [ ] AutoConfirmJob — auto-confirm deliveries after window
- [ ] NotificationDispatchJob — send queued notifications
- [ ] OutboxProcessorJob — process outbox messages
- [ ] WebhookRetryJob — retry failed webhook deliveries
- [ ] VirtualAccountExpiryJob — expire unused virtual accounts
- [ ] VerificationExpiryJob — expire stale verification requests

---

## 8. Utility Services (4 files)

Location: `src/Naitrust.Application/Services/Utility/`

- [ ] TransactionStateMachine — transaction state transitions
- [ ] PaymentStateMachine — payment state transitions
- [ ] DisputeStateMachine — dispute state transitions
- [ ] VerificationStateMachine — verification state transitions

---

## 9. Helpers (4 files)

Location: `src/Naitrust.Application/Helpers/`

- [ ] ClaimsHelper — extract claims from HttpContext
- [ ] PaginationHelper — pagination utilities
- [ ] ReferenceGenerator — generate transaction/payment references
- [ ] PayoutNameMatcher — match payout account names

---

## 10. Middleware (2 files)

Location: `src/Naitrust.Api/Middleware/`

- [ ] RateLimitingMiddleware — Redis-based sliding window rate limiting
- [ ] IdempotencyMiddleware — idempotency key header check and caching

---

## 11. Authorization Handlers (2 files)

Location: `src/Naitrust.Api/Authorization/`

- [ ] TransactionPartyHandler — verify user is a party to the transaction
- [ ] BusinessMemberHandler — verify user is a member of the business

---

## 12. SignalR Hubs (3 files)

Location: `src/Naitrust.Api/Hubs/`

- [ ] TransactionHub — real-time transaction status updates
- [ ] VerificationHub — real-time verification progress
- [ ] NotificationHub — real-time notification delivery

---

## 13. Background Service (1 file)

Location: `src/Naitrust.Infrastructure/BackgroundServices/`

- [ ] OutboxBackgroundService — hosted service for outbox pattern

---

## 14. Role & Claims Management (Decision Pending)

Naitrust currently uses a simple `UserRole` enum on the `User` entity (no role table, no claims). Need to decide on approach:

**Option A — ASP.NET Identity**
- Swap `DbContext` for `IdentityDbContext<User, Role, Guid>`
- Get `UserManager`, `RoleManager`, `SignInManager` for free
- Role/claim tables auto-created
- Adds Identity columns to User (PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, etc.)

**Option B — Custom role/claims tables (keep current approach)**
- Add `Role` entity + `UserRole` join table (many-to-many)
- Add `RoleClaim` entity for granular permissions per role
- Build lightweight `RoleService` and `RoleClaimService`
- No Identity baggage, cleaner schema

**Regardless of approach, these need implementing:**
- [ ] Role storage (table or Identity)
- [ ] Claims/permissions per role
- [ ] RoleService — CRUD for roles
- [ ] RoleClaimService — manage claims per role
- [ ] Seed default roles (User, Admin, SuperAdmin)
- [ ] Seed role claims/permissions
- [ ] Policy registration in Program.cs (wire up the 6 policies in `Policies.cs`)
- [ ] TokenService — include roles/claims in JWT
- [ ] ClaimsHelper — extract role/claims from HttpContext
- [ ] TransactionPartyHandler — verify user is party to transaction
- [ ] BusinessMemberHandler — verify user is business member

---

## Build Order Reference (from plan.md)

Follow this order when implementing:

1. ~~Scaffold~~ (done)
2. ~~Port safe patterns from old code~~ (done)
3. Entity configurations + database migrations
4. Auth and user profile
5. Verification (individual, business, facial, ownership, manual review)
6. Business onboarding
7. Transaction creation, invitation, acceptance, terms
8. Evidence + Phase 1 single-release
9. Providus payment partner adapter
10. Virtual accounts, webhooks, ledger, reconciliation
11. Dispute lifecycle
12. Reputation
13. Admin queues
14. Audit logs
15. Integration tests
16. AI intelligence (post-MVP)

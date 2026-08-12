# P1 API Alignment Plan — Frontend ↔ Backend

**Created:** 2026-08-07
**Status:** In Progress
**Goal:** Make every frontend API call hit a matching backend endpoint with the correct request/response shape.

---

## Module 1: AUTH ✏️ (Start Here)

**Priority:** Highest — everything depends on it

### Endpoints to Verify

| # | Endpoint | Method | Status | Notes |
|---|----------|--------|--------|-------|
| 1.1 | `/auth/register` | POST | ✅ Exists | Verify response shape: `{user, token}` |
| 1.2 | `/auth/login` | POST | ✅ Exists | Verify response: `{user, token}` or `{user, requires2FA}` |
| 1.3 | `/auth/login/verify-2fa` | POST | ✅ Exists | Verify accepts `{userId}` or `{email}` + `{token}` |
| 1.4 | `/auth/logout` | POST | ✅ Exists | |
| 1.5 | `/auth/profile` | GET | ✅ Exists | Verify returns `{user: {...}}` not flat user |
| 1.6 | `/auth/profile` | PUT | ✅ Exists | Verify accepts `{firstName, lastName, phoneNumber, bio, address, city, state, country}` |
| 1.7 | `/auth/change-password` | POST | ✅ Exists | Frontend sends `{oldPassword, newPassword}` — verify field names match backend |
| 1.8 | `/auth/verify-email` | POST | ✅ Exists | Verify returns `{user, token}` on success |
| 1.9 | `/auth/resend-verification-otp` | POST | ✅ Exists | |
| 1.10 | `/auth/forgot-password` | POST | ✅ Exists | |
| 1.11 | `/auth/verify-otp` | POST | ✅ Exists | Must return `{resetToken}` |
| 1.12 | `/auth/reset-password` | POST | ✅ Exists | Frontend sends `{email, resetToken, newPassword}` |

### Known Issues to Check

- [ ] `GET /auth/profile` — frontend `fetchProfile` does `set({ user: response.data })` but `auth.api.ts` does `setUserData(response.data.user)`. Verify the backend wraps user in `{user: {...}}` inside `data`.
- [ ] `POST /auth/change-password` — frontend sends `{oldPassword, newPassword}`, backend DTO has `{CurrentPassword, NewPassword}`. **Field name mismatch: `oldPassword` vs `CurrentPassword`**.
- [ ] `POST /auth/register` — `pendingBusinessData` DTO is missing fields: `description`, `email`, `phoneNumber`, `website`, `socialHandles`, `verificationType`.
- [ ] User response shape — frontend expects: `{id, email, firstName, lastName, name, role, phone, kycLevel, kycVerified, isEmailVerified, isPhoneVerified}`. Verify backend `AuthResponse` returns all these.

### Files to Modify

- `src/Naitrust.Domain/Models/Dtos/Requests/Auth/RegisterRequest.cs` — expand `PendingBusinessDataInput`
- `src/Naitrust.Domain/Models/Dtos/Requests/Auth/ChangePasswordRequest.cs` — verify field names
- `src/Naitrust.Domain/Models/Dtos/Responses/Auth/AuthResponse.cs` — verify user shape
- `src/Naitrust.Application/Services/Implementations/Auth/AuthService.cs` — verify response building

---

## Module 2: SECURITY 🔴 (Full Rewrite)

**Priority:** High — tied to onboarding/verification flow

### Current State: All P1 stubs are WRONG — none match the frontend

### Delete These Endpoints (not called by frontend)

- `POST /security/change-password` (frontend uses `/auth/change-password`)
- `POST /security/change-pin`
- `POST /security/change-transaction-pin`
- `POST /security/disable-2fa`
- `POST /security/setup-biometric`
- `POST /security/verify-biometric`
- `GET /security/active-sessions`

### Create These Endpoints (what frontend actually calls)

| # | Endpoint | Method | Request Body | Response |
|---|----------|--------|-------------|----------|
| 2.1 | `/security/email/send-otp` | POST | `{email}` | `null` |
| 2.2 | `/security/email/verify` | POST | `{code}` | `{verified: bool}` |
| 2.3 | `/security/phone/send-otp` | POST | `{phone}` | `null` |
| 2.4 | `/security/phone/verify` | POST | `{code}` | `{verified: bool}` |
| 2.5 | `/security/2fa/start` | POST | `{email}` | `{secret, otpauthUri}` |
| 2.6 | `/security/2fa/verify` | POST | `{code}` | `{enabled: bool}` |
| 2.7 | `/security/kyc` | POST | `{kind: "individual"\|"business", ...payload}` | TBD |
| 2.8 | `/security/pin/set` | POST | `{pin}` | `{set: bool}` |
| 2.9 | `/security/pin/verify` | POST | `{pin}` | `{valid: bool}` |

### DB Impact

- Add `PinHash` field to `NaitrustUser` entity
- Possibly add `KycSubmission` entity for KYC document tracking

### Files to Modify/Rewrite

- `src/Naitrust.Api/Controllers/SecurityController.cs` — full rewrite
- `src/Naitrust.Application/Services/Interfaces/ISecurityService.cs` — full rewrite
- `src/Naitrust.Application/Services/Implementations/Security/SecurityService.cs` — full rewrite
- `src/Naitrust.Domain/Models/Dtos/Requests/Security/*` — delete all, create new
- `src/Naitrust.Domain/Models/Dtos/Responses/Security/*` — delete all, create new

---

## Module 3: BUSINESS (DTO Fixes)

**Priority:** High — needed for business registration flow

| # | Issue | Fix |
|---|-------|-----|
| 3.1 | `socialHandles[].handle` → frontend expects `value` | Rename `SocialHandleDto.Handle` to `Value` |
| 3.2 | `PendingBusinessDataInput` missing fields | Add: `Description`, `Email`, `PhoneNumber`, `Website`, `SocialHandles`, `VerificationType` |
| 3.3 | `PUT /businesses/:id` vs `PATCH /businesses/:id` | Add `[HttpPut]` alias alongside existing `[HttpPatch]` |
| 3.4 | `CreateBusinessData` frontend sends `category` | Backend maps `Type` → `Category` in response — verify create accepts `category` too |

### Files to Modify

- `src/Naitrust.Domain/Models/Dtos/Responses/Businesses/BusinessResponse.cs` — rename `Handle` → `Value` in `SocialHandleDto`
- `src/Naitrust.Domain/Models/Dtos/Requests/Auth/RegisterRequest.cs` — expand `PendingBusinessDataInput`
- `src/Naitrust.Api/Controllers/BusinessesController.cs` — add PUT alias

---

## Module 4: INVITATIONS (Field Renames)

**Priority:** Medium — deal flow entry point

| # | Issue | Fix |
|---|-------|-----|
| 4.1 | `PublicInvitationPreview.inviterName` | Backend returns `FromName` — rename or alias |
| 4.2 | `agreement` shape | Frontend expects `{version, generatedByAi, sections}`, backend returns `AgreementSnapshotDto {sections}` — add `Version` and `GeneratedByAi` |

### Files to Modify

- `src/Naitrust.Domain/Models/Dtos/Responses/Invitations/PublicInvitationPreviewResponse.cs`
- `src/Naitrust.Domain/Models/Dtos/Responses/Invitations/DealInvitationResponse.cs`

---

## Module 5: NEGOTIATION (Route + Shape Fixes)

**Priority:** Medium — deal room feature

| # | Issue | Fix |
|---|-------|-----|
| 5.1 | Route `/negotiation/start` | Rename to `/negotiation/propose` |
| 5.2 | Route `/negotiation/respond` (no ID) | Change to `/negotiation/proposals/{proposalId}` |
| 5.3 | Missing `POST /negotiation/withdraw` | Add endpoint |
| 5.4 | Response field `transactionId` | Rename to `dealId` |
| 5.5 | `ProposedChangesInput.AgreementSections` | Replace with `AgreementNote` (string) |

### Files to Modify

- `src/Naitrust.Api/Controllers/NegotiationsController.cs`
- `src/Naitrust.Domain/Models/Dtos/Requests/Negotiations/*`
- `src/Naitrust.Domain/Models/Dtos/Responses/Negotiations/*`
- `src/Naitrust.Application/Services/Implementations/Negotiations/NegotiationService.cs`

---

## Module 6: DISPUTES (Route Fix)

**Priority:** Medium

| # | Issue | Fix |
|---|-------|-----|
| 6.1 | `/transactions/:id/disputes` (plural) | Add singular alias `/transactions/:id/dispute` |
| 6.2 | Message route `POST /disputes/:id/messages` | Add alias at `POST /transactions/:id/dispute/messages` |

### Files to Modify

- `src/Naitrust.Api/Controllers/DisputesController.cs`

---

## Module 7: TERMINATION (Shape Fix)

**Priority:** Medium

| # | Issue | Fix |
|---|-------|-----|
| 7.1 | Request: `{accept: bool, reason?, byName?}` | Change from `{action: string}` to match |
| 7.2 | Response missing: `requestedByName`, `requestedByYou`, `respondedByName`, `responseReason` | Resolve user IDs to names, add computed `requestedByYou` |
| 7.3 | Response field `transactionId` | Rename to `dealId` |

### DB Impact

- Add `ResponseReason` column to `DealTermination` entity

### Files to Modify

- `src/Naitrust.Domain/Models/Dtos/Requests/Transactions/RespondTerminationRequest.cs`
- `src/Naitrust.Domain/Models/Dtos/Responses/Transactions/DealTerminationResponse.cs`
- `src/Naitrust.Domain/Models/Entities/DealTermination.cs`
- `src/Naitrust.Application/Services/Implementations/Transactions/TransactionSubResourceService.cs`

---

## Module 8: TRACKING (Add Write Endpoints)

**Priority:** Medium

| # | Endpoint | Method | Request | Notes |
|---|----------|--------|---------|-------|
| 8.1 | `/transactions/:id/tracking` | POST | `{title, description?, afterStepId?}` | Add tracking step |
| 8.2 | `/transactions/:id/tracking/advance` | POST | (no body) | Advance to next milestone |
| 8.3 | `/transactions/:id/tracking/revert` | POST | (no body) | Revert last advance |
| 8.4 | `/transactions/:id/tracking/:stepId` | PATCH | `{title, description?}` | Edit a step |

### Files to Modify

- `src/Naitrust.Api/Controllers/TrackingController.cs` — add 4 actions
- `src/Naitrust.Application/Services/Interfaces/ITransactionSubResourceService.cs` — add methods
- `src/Naitrust.Application/Services/Implementations/Transactions/TransactionSubResourceService.cs` — implement

---

## Module 9: UPLOAD (Route Fix)

**Priority:** Low — one line

| # | Issue | Fix |
|---|-------|-----|
| 9.1 | `POST /upload` | Change to `POST /upload/verification-document` |

### Files to Modify

- `src/Naitrust.Api/Controllers/UploadController.cs`

---

## Module 10: NEW MODULES (Future — Mock-only on Frontend)

These modules have no backend at all. Frontend uses mock data. Defer until core flows work.

| Module | Endpoints | New Entities |
|--------|-----------|-------------|
| Wallet | 5 (get, fund, withdraw, bank-accounts, activity) | `Wallet`, `WalletActivity`, `LinkedBankAccount` |
| Instant Transfers | 4 (validate, create, get, list) | `InstantTransfer` |
| Beneficiaries | 3 (list, create, delete) | `Beneficiary` |
| Payment Requests | 3 (list, create, respond) | `PaymentRequest` |
| Counterparties | 3 (list, favourite, block) | `Counterparty` |
| Trust Profile | 1 (get mine) | Computed view |

---

## DB Migration Summary

When all modules are done, run a single migration covering:

- [ ] `PinHash` on `NaitrustUser` (Module 2)
- [ ] `ResponseReason` on `DealTermination` (Module 7)
- [ ] Any new KYC entity (Module 2, TBD)

---

## Execution Checklist

- [ ] Module 1: Auth — verify & fix shapes
- [ ] Module 2: Security — full rewrite
- [ ] Module 3: Business — DTO fixes
- [ ] Module 4: Invitations — field renames
- [ ] Module 5: Negotiation — route + shape fixes
- [ ] Module 6: Disputes — route fix
- [ ] Module 7: Termination — shape fix
- [ ] Module 8: Tracking — add write endpoints
- [ ] Module 9: Upload — route fix
- [ ] Migration: `dotnet ef migrations add ApiAlignment_P1`
- [ ] Build verification: `dotnet build`
- [ ] End-to-end test per module

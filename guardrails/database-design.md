# Database Design

Use PostgreSQL with Entity Framework Core and Npgsql.

This is the initial product data model. It should be implemented as EF Core entity classes and `IEntityTypeConfiguration<T>` mappings with constrained enums where practical.

Approved source: `../../Naitrust Technical Spec v2.docx`.

Project decision: the API is ASP.NET Core with PostgreSQL and Entity Framework Core. Entity Framework Core is the only approved backend ORM. Do not use Prisma or Drizzle in the API.

Money is stored as integer minor units, such as kobo. Never store money as floating-point values.

## Core Tables

### parties

Represents an individual or business participating in transactions.

- `id`
- `party_type`: individual, business
- `user_id`
- `business_id`
- `display_name`
- `verification_status`
- `bvn_reference`
- `cac_reference`
- `reputation_profile_id`
- `created_at`
- `updated_at`

### users

- `id`
- `email`
- `phone`
- `password_hash`
- `first_name`
- `last_name`
- `role`: user, admin, super_admin
- `status`: active, suspended, deleted
- `email_verified_at`
- `phone_verified_at`
- `identity_verified_at`
- `last_liveness_verified_at`
- `last_transaction_activity_at`
- `created_at`
- `updated_at`

### businesses

- `id`
- `owner_user_id`
- `name`
- `type`
- `registration_number`
- `tax_id`
- `country`
- `state`
- `address`
- `verification_status`
- `business_verified_at`
- `ownership_verified_at`
- `verification_expires_at`
- `risk_level`
- `created_at`
- `updated_at`

### business_members

- `id`
- `business_id`
- `user_id`
- `role`: owner, admin, operator, viewer
- `status`
- `created_at`
- `updated_at`

### verification_requests

- `id`
- `subject_type`: user, business
- `subject_id`
- `transaction_id`
- `requested_by_user_id`
- `provider`
- `verification_type`
- `verification_level`: basic, standard, enhanced
- `status`
- `payment_status`
- `payment_reference`
- `provider_reference`
- `result_summary`
- `risk_flags`
- `expires_at`
- `reviewed_by`
- `reviewed_at`
- `created_at`
- `updated_at`

### verification_steps

- `id`
- `verification_request_id`
- `step`: email, phone, id_document, cac, tin, bvn, facial, ownership, manual_review
- `provider`
- `status`: pending, processing, success, failed, skipped
- `message`
- `started_at`
- `completed_at`
- `created_at`
- `updated_at`

### verification_documents

- `id`
- `verification_request_id`
- `uploaded_by_user_id`
- `document_type`: selfie, selfie_with_id, cac_certificate, tax_certificate, proof_of_address, personal_id, supporting_document
- `file_url`
- `file_name`
- `mime_type`
- `file_size`
- `status`: pending, accepted, rejected
- `review_notes`
- `created_at`
- `updated_at`

### face_match_results

- `id`
- `verification_request_id`
- `provider`
- `id_type`: bvn, nin, passport, drivers_license, other
- `id_number_hash`
- `match`
- `match_score`
- `confidence`
- `liveness_passed`
- `raw_provider_response`
- `created_at`

### ownership_checks

- `id`
- `verification_request_id`
- `business_id`
- `user_id`
- `method`: identity_match, cac_email, cac_phone, bank_account, manual_review
- `status`: pending, verified, failed, manual_review
- `evidence_summary`
- `created_at`
- `updated_at`

### verification_provider_events

- `id`
- `verification_request_id`
- `provider`
- `provider_reference`
- `event_type`
- `payload`
- `created_at`

## Transaction Tables

### transactions

- `id`
- `reference`
- `transaction_type_id`
- `created_by_user_id`
- `business_id`
- `party_mode`: b2b, b2c
- `title`
- `description`
- `category`: domestic_supplier_contractor, b2c_vendor_service, wholesale, real_estate, event_vendor, home_service, other
- `amount_minor`
- `fee_minor`
- `currency`
- `status`: draft, pending_counterparty, terms_negotiation, terms_agreed, awaiting_funding, funded, in_progress, evidence_submitted, buyer_review, release_approved, disputed, paid_out, refunded, cancelled, completed
- `payment_status`
- `verification_level_required`
- `risk_level`
- `agreement_id`
- `terms_accepted_at`
- `auto_confirm_at`
- `completed_at`
- `cancelled_at`
- `created_at`
- `updated_at`

### transaction_types

Configuration template that drives the protected transaction engine.

- `id`
- `key`: domestic_single_release, domestic_b2c_single_release, wholesale, real_estate_deposit
- `name`
- `required_verification_level`
- `evidence_requirements`
- `release_mode`: single, milestone
- `dispute_rules`
- `fee_model`
- `auto_confirm_window_hours`
- `is_active`
- `created_at`
- `updated_at`

### transaction_parties

- `id`
- `transaction_id`
- `user_id`
- `business_id`
- `party_type`: buyer, seller, customer, vendor, service_provider, client, agent, admin_observer
- `party_mode`: b2b, b2c
- `display_name`
- `email`
- `phone`
- `status`: invited, accepted, rejected, removed
- `accepted_at`
- `created_at`
- `updated_at`

### agreements

- `id`
- `transaction_id`
- `version`
- `summary`
- `description`
- `delivery_conditions`
- `release_conditions`
- `proof_requirements`
- `dispute_rules`
- `auto_confirm_window_hours`
- `delivery_due_at`
- `created_by_user_id`
- `buyer_accepted_at`
- `seller_accepted_at`
- `frozen_at`
- `created_at`
- `updated_at`

### milestones

Phase 2 entity. Do not make milestones required for Phase 1 domestic single-release transactions.

- `id`
- `transaction_id`
- `title`
- `description`
- `amount`
- `due_at`
- `status`: pending, in_progress, submitted, approved, disputed, completed
- `submitted_at`
- `approved_at`
- `created_at`
- `updated_at`

### evidence_files

- `id`
- `transaction_id`
- `milestone_id`
- `uploaded_by_user_id`
- `type`: invoice, receipt, photo, video, waybill, shipping_document, inspection_report, contract, other
- `file_url`
- `file_name`
- `mime_type`
- `file_size`
- `description`
- `created_at`

## Payment, Ledger, and Custody Tables

### virtual_accounts

- `id`
- `transaction_id`
- `partner`: providus, kora, wema, anchor
- `provider_reference`
- `account_number`
- `account_name`
- `bank_name`
- `amount_expected_minor`
- `amount_received_minor`
- `currency`
- `status`: requested, issued, funded, expired, closed, failed
- `expires_at`
- `funded_at`
- `created_at`
- `updated_at`

### payment_partner_events

- `id`
- `virtual_account_id`
- `partner`: providus, kora, wema, anchor
- `provider_event_id`
- `event_type`
- `payload`
- `processed_at`
- `created_at`

### ledger_entries

Append-only double-entry ledger postings.

- `id`
- `transaction_id`
- `entry_group_id`
- `event_type`: funding_confirmed, release_approved, fee_recognized, seller_payout_executed, buyer_refund_executed, split_resolution_executed, fee_swept, reconciliation_adjustment
- `account`
- `debit_minor`
- `credit_minor`
- `currency`
- `memo`
- `created_at`

### payment_instructions

Signed instructions sent to payment/bank partners.

- `id`
- `transaction_id`
- `virtual_account_id`
- `instruction_type`: release, refund, split, fee_sweep
- `partner`: providus, kora, wema, anchor
- `idempotency_key`
- `signed_payload_hash`
- `status`: pending, sent, confirmed, failed, cancelled
- `partner_reference`
- `partner_response`
- `created_at`
- `updated_at`

### release_requests

- `id`
- `transaction_id`
- `requested_by_user_id`
- `provider`
- `provider_reference`
- `status`: requested, processing, released, rejected, failed
- `reason`
- `requested_at`
- `resolved_at`
- `created_at`
- `updated_at`

### payout_accounts

- `id`
- `party_id`
- `bank_code`
- `bank_name`
- `account_number_hash`
- `account_name`
- `name_match_status`: pending, matched, mismatch, manual_review
- `provider_reference`
- `verified_at`
- `created_at`
- `updated_at`

## Dispute Tables

### disputes

- `id`
- `transaction_id`
- `opened_by_user_id`
- `status`
- `reason`
- `description`
- `admin_owner_id`
- `resolution`
- `resolved_at`
- `created_at`
- `updated_at`

### dispute_messages

- `id`
- `dispute_id`
- `sender_user_id`
- `message`
- `created_at`

### dispute_evidence

- `id`
- `dispute_id`
- `evidence_file_id`
- `submitted_by_user_id`
- `created_at`

## Reputation Tables

### reputation_profiles

- `id`
- `subject_type`: user, business
- `subject_id`
- `completed_transactions_count`
- `disputed_transactions_count`
- `cancelled_transactions_count`
- `total_completed_value`
- `rating_average`
- `rating_count`
- `updated_at`

### reviews

- `id`
- `transaction_id`
- `reviewer_user_id`
- `reviewee_subject_type`
- `reviewee_subject_id`
- `rating`
- `comment`
- `created_at`

## Operational Tables

### notifications

- `id`
- `user_id`
- `type`
- `title`
- `body`
- `metadata`
- `read_at`
- `created_at`

## AI Intelligence Tables

### ai_assessments

- `id`
- `entity_type`: transaction, user, business, verification_request, dispute, evidence
- `entity_id`
- `assessment_type`: risk_score, dispute_summary, evidence_checklist, fraud_signal, verification_summary, reputation_summary
- `model`
- `prompt_version`
- `input_refs`
- `output`
- `risk_level`
- `confidence`
- `created_by`: system, admin
- `created_at`

### ai_feedback

- `id`
- `assessment_id`
- `user_id`
- `feedback_type`: helpful, not_helpful, wrong, unsafe, missing_context
- `notes`
- `created_at`

### ai_prompt_versions

- `id`
- `name`
- `version`
- `purpose`
- `schema`
- `status`: draft, active, retired
- `created_at`
- `updated_at`

### vector_documents

- `id`
- `source_type`
- `source_id`
- `embedding_model`
- `embedding`
- `metadata`
- `created_at`

### audit_logs

- `id`
- `actor_user_id`
- `action`
- `entity_type`
- `entity_id`
- `before`
- `after`
- `ip_address`
- `user_agent`
- `created_at`

### idempotency_keys

- `id`
- `key`
- `scope`
- `request_hash`
- `response_body`
- `status_code`
- `created_at`
- `expires_at`

## Important Indexes

- `users.email`
- `users.phone`
- `businesses.owner_user_id`
- `businesses.registration_number`
- `verification_requests.subject_type, verification_requests.subject_id`
- `verification_requests.transaction_id`
- `verification_steps.verification_request_id`
- `verification_documents.verification_request_id`
- `transactions.reference`
- `transactions.created_by_user_id`
- `transactions.business_id`
- `transactions.status`
- `transaction_parties.transaction_id`
- `transaction_parties.user_id`
- `payment_intents.transaction_id`
- `payment_intents.provider_reference`
- `payment_events.provider_event_id`
- `disputes.transaction_id`
- `notifications.user_id`
- `ai_assessments.entity_type, ai_assessments.entity_id`
- `ai_feedback.assessment_id`
- `vector_documents.source_type, vector_documents.source_id`
- `audit_logs.entity_type, audit_logs.entity_id`

# AI Intelligence Plan

Naitrust should use AI as a trust intelligence layer, not as the final authority on payments, verification, or disputes.

The goal is to make Naitrust the best trusted-transaction platform by helping users and admins understand risk, organize evidence, detect suspicious behavior, and complete safer deals faster.

Approved Phase 1 technical spec note:

> The full AI agent suite is not required for the domestic MVP. AI-assisted dispute triage should return after real transaction data exists.

This plan is the AI roadmap, not a requirement to block the Phase 1 domestic protected-transaction build.

## OpenAI Architecture

Use OpenAI through backend services only.

Recommended OpenAI capabilities:

- Responses API for structured reasoning, summaries, and assisted workflows.
- Structured Outputs for predictable JSON outputs.
- Tool/function calling for controlled access to internal data.
- Embeddings for similarity search across disputes, fraud patterns, documents, and transaction history.
- Moderation/safety checks for uploaded text, messages, and public profile content.
- Agents SDK later for admin copilots and multi-step investigation workflows.
- Evals for measuring AI quality before production changes.

Official docs references:

- Responses/text generation: https://developers.openai.com/api/docs/guides/text?api-mode=responses
- Tools/function calling: https://developers.openai.com/api/docs/guides/tools
- Structured outputs: https://developers.openai.com/api/docs/guides/structured-outputs
- Embeddings: https://developers.openai.com/api/docs/guides/embeddings
- Moderation: https://developers.openai.com/api/docs/guides/moderation
- Agents SDK: https://developers.openai.com/api/docs/guides/agents
- Evals: https://developers.openai.com/api/docs/guides/evals
- Safety best practices: https://developers.openai.com/api/docs/guides/safety-best-practices

## AI Principles

- AI supports human and system decisions; it does not replace regulated providers, admins, or legal/compliance judgment.
- AI outputs must be explainable and stored with model name, prompt version, input references, and confidence level.
- AI should return structured JSON for risk scores, reasons, recommended next actions, and missing evidence.
- AI must never receive more personal data than needed.
- AI must not expose raw BVN, NIN, ID numbers, facial images, private documents, or provider raw responses to unauthorized users.
- AI should not make final dispute decisions, release payments, reject verification, or suspend users without deterministic rules or human approval.

## Core AI Features

### 1. Transaction Risk Scoring

Purpose:

- identify risky safe deals before money movement.
- recommend the right verification level.
- surface suspicious patterns to admins.

Inputs:

- transaction amount.
- category.
- account age.
- verification status.
- liveness freshness.
- prior completed/disputed deals.
- counterparties.
- evidence completeness.
- device/IP risk signals where available.

Outputs:

- risk level: low, medium, high, critical.
- reasons.
- required verification level.
- recommended admin review.
- user-friendly safety guidance.

### 2. Verification Intelligence

Purpose:

- decide whether existing verification is reusable.
- decide whether fresh liveness is needed.
- summarize verification mismatches for admins.

AI may help summarize:

- name mismatches.
- CAC/director mismatches.
- document inconsistency.
- stale liveness.
- conflicting addresses or phone numbers.

Deterministic rules still control:

- whether verification has expired.
- whether liveness freshness is required.
- whether a provider result passed or failed.

### 3. Evidence Completeness Assistant

Purpose:

- help users upload the right evidence before a dispute happens.

Examples:

- import transaction: invoice, proforma, waybill, shipping document, inspection photo.
- freelance project: scope, milestone proof, delivery files, approval messages.
- real estate: property documents, agent ID, inspection proof, payment receipt.

Outputs:

- missing evidence checklist.
- plain-language explanation.
- risk if evidence is missing.

### 4. Dispute Summary Assistant

Purpose:

- help admins understand disputes quickly.

Inputs:

- transaction terms.
- milestone history.
- evidence metadata.
- party messages.
- payment status.
- activity timeline.

Outputs:

- neutral dispute summary.
- timeline.
- claims by buyer.
- claims by seller.
- missing evidence.
- recommended next admin questions.

AI must not decide the winner.

### 5. Fraud Pattern Detection

Purpose:

- detect repeated suspicious patterns across accounts and transactions.

Signals:

- repeated failed verifications.
- reused documents.
- similar dispute narratives.
- repeated bank/account/payment references.
- high-risk category plus new account.
- sudden transaction amount jump.
- repeated counterparties with circular behavior.

Use embeddings for similarity matching where useful.

### 6. Smart Transaction Drafting

Purpose:

- help users create clearer deal terms.

AI can help generate:

- milestone suggestions.
- release conditions.
- delivery evidence requirements.
- dispute prevention checklist.
- simpler plain-English agreement summary.

All generated terms must be editable and user-approved.

### 7. Reputation Intelligence

Purpose:

- turn completed safe deals into meaningful trust.

AI can summarize:

- transaction history.
- strengths.
- common categories.
- evidence quality.
- dispute response quality.

Do not generate reputation claims that cannot be backed by completed transaction data.

### 8. Admin Copilot

Purpose:

- help Naitrust operations teams review risky cases faster.

Capabilities:

- summarize a user/business.
- compare submitted data against verified data.
- list risk flags.
- ask for missing information.
- draft admin notes.
- prepare customer-support replies.

Admin copilot cannot:

- approve verification by itself.
- reject verification by itself.
- resolve disputes by itself.
- trigger money release by itself.
- suspend accounts by itself.

## Data Model Additions

### ai_assessments

- `id`
- `entity_type`: transaction, user, business, verification_request, dispute, evidence
- `entity_id`
- `assessment_type`: risk_score, dispute_summary, evidence_checklist, fraud_signal, reputation_summary
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

## API Endpoints

Internal/admin endpoints:

- `POST /ai/transactions/:id/risk-assessment`
- `POST /ai/transactions/:id/evidence-checklist`
- `POST /ai/disputes/:id/summary`
- `POST /ai/verifications/:id/summary`
- `POST /ai/reputation/:profileId/summary`
- `POST /ai/admin/cases/:id/copilot`
- `POST /ai/feedback`

Public user-facing endpoints should return only safe, filtered AI outputs.

## Implementation Phases

### AI Phase 1: Safe Internal AI

- transaction risk summary.
- dispute summary.
- verification mismatch summary.
- evidence checklist.
- admin-only UI.

### AI Phase 2: User-Facing Assistants

- smart transaction drafting.
- missing evidence guidance.
- simple safety tips.
- profile/reputation summary.

### AI Phase 3: Pattern Intelligence

- embeddings for similar disputes.
- similar fraud reports.
- repeated document/account/device risk signals.
- admin case clustering.

### AI Phase 4: Agentic Admin Workflows

- admin copilot with tool access.
- multi-step case investigation.
- traceable agent runs.
- evals before every major prompt/model change.

## Required Evals

Create evals for:

- risk scoring consistency.
- dispute summary neutrality.
- evidence checklist completeness.
- unsafe recommendation detection.
- privacy leakage prevention.
- hallucinated facts.
- refusal to make final regulated/payment decisions.

## Prompt Guardrails

Every AI prompt should include:

- role and task.
- allowed data.
- forbidden actions.
- output JSON schema.
- instruction to cite internal evidence IDs.
- instruction to say `insufficient_information` when facts are missing.
- instruction not to make final legal, compliance, payment, or dispute decisions.

## Production Rules

- Store prompts and model versions.
- Log every AI assessment.
- Use structured outputs for critical workflows.
- Add human review for high-risk outputs.
- Rate limit AI endpoints.
- Redact sensitive fields before model calls where possible.
- Keep AI calls server-side.
- Use evals before changing prompts or models.

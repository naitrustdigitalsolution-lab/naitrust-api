# Backend UI Support Notes

The backend does not render the UI, but it must support the product experience described in `../../naitrust-web/guardrails/ui.md`.

## UI-Driven Backend Requirements

The API must provide enough data for these frontend panels without requiring fragile client-side inference:

- transaction overview.
- party trust panel.
- terms panel.
- virtual account funding panel.
- evidence requirements timeline.
- evidence gallery.
- activity log.
- dispute panel.
- reputation summary.
- admin review queues.

## Required Aggregated Views

### Transaction Detail View

`GET /transactions/:id` should return:

- transaction summary.
- parties.
- current terms version.
- evidence requirements.
- evidence summary.
- payment status summary.
- dispute summary if any.
- activity timeline.
- allowed actions for current user.

### Dashboard View

The backend should provide a dashboard endpoint or efficient query support for:

- active transactions.
- pending user actions.
- payment exceptions.
- open disputes.
- verification progress.
- recent activity.
- reputation summary.

### Admin Review View

Admin endpoints should return:

- risk flags.
- verification status.
- payment state.
- dispute status.
- evidence count.
- timeline summary.
- recommended next action labels.

AI can summarize risk or dispute context for admins, but final decisions must remain human/admin actions.

## Allowed Actions Pattern

To prevent frontend hallucination, transaction detail responses should include an `allowedActions` object.

Example:

```json
{
  "allowedActions": {
    "acceptTerms": true,
    "createPaymentIntent": false,
    "uploadEvidence": true,
    "approveDelivery": false,
    "requestRelease": false,
    "openDispute": true,
    "cancelTransaction": false
  }
}
```

The frontend should use this to enable or disable major actions.

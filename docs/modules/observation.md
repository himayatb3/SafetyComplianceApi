# Module: Observation (OBS-*)

Record type key: `observation` · Depends on: 04, 05 · Status: Draft

## 1. Purpose
Capture safe/unsafe acts and conditions seen in the workplace (positive and negative), fast. Feeds leading indicators; escalates to near-miss/incident when warranted.

## 2. Permissions (default roles)
`observation.record.create` (all incl. contractor) · `.view` (scope) · `.triage` (HSE officer, supervisor) · `.assign` · `.close` · `.void` (HSE manager) · `.reclassify`.

## 3. Fields
Required at create: site, occurredAt, type (`Unsafe act | Unsafe condition | Safe behaviour/Positive`), short description.
Optional: area, category (taxonomy), photo(s), immediate action taken, person observed (optional, non-punitive), risk rating guess, geo-pin.
Required by triage: category, risk level (low/med/high), disposition.

## 4. Workflow
`Reported → Triaged → ActionPending → Closed` (+ `Cancelled`, `Reclassified`)
- Positive observations: `Reported → Acknowledged(closed)` — no actions needed; optional recognition.
- Low risk + immediate fix recorded: may go `Reported → Closed` with supervisor sign-off (tenant setting).
- SLA: triage ≤ 24 h (tenant configurable).

## 5. Rules
- OBS-001 Positive observations never create corrective actions by default and are excluded from "unsafe" counts but included in engagement metrics.
- OBS-002 Observer may mark "fixed on the spot" with description; still requires supervisor acknowledgement.
- OBS-003 High-risk observation → system suggests reclassification to Near-miss/Incident (REC-005) and notifies HSE immediately.
- OBS-004 Observations about a named person are non-disciplinary by design: person field optional, visible only to HSE roles; tenant can disable the field.
- OBS-005 Observation cards in bulk (e.g. behaviour-based safety walk): multi-item submit in one session (each item its own record, shared `SessionId`).
- OBS-006 Observer cannot triage or close their own observation if separation-of-duties is on (ACC-022).
- OBS-007 Unactioned observations in `Triaged` > SLA escalate (WFL-030) and feed G-02/G-03.

## 6. Edge cases
- OBS-E01 Same hazard reported by many people in a day → duplicate suggestion + "me too" counter linked to one record (counts as engagement, one action). T
- OBS-E02 Observation about contractor from another company → visible to host-site HSE and the contractor company's safety contact only if configured.
- OBS-E03 Observer later says "this was a near-miss" → reclassify, don't edit type. T
- OBS-E04 Observation with no photo in low-connectivity area → saves offline (EDG-020).
- OBS-E05 Hazard persists after close (recurs) → G-04 repeat detection; linking to prior closed record is suggested.
- OBS-E06 Observer targets a colleague maliciously → identity visible to HSE for follow-up; flag/moderation path (EDG-024).

## 7. Notifications & Insights
Events: reported (to supervisor/site HSE), high-risk (immediate), triage overdue, action overdue, closed (feedback to observer).
Rules fed: G-01, G-03, G-04, G-10, G-11, G-14.

## 8. API
`POST /api/v1/observations` · `GET /api/v1/observations?site&status&from&to&cursor` · `GET /{id}` · `PATCH /{id}` · `POST /{id}/transitions` · `POST /{id}/reclassify` · `POST /{id}/attachments` · actions via shared `/actions`.

## 9. Acceptance
- [ ] Create with 4 fields works offline and online, idempotent.
- [ ] Positive observation closes without actions.
- [ ] Tenant B cannot see/guess tenant A observation ids (404).
- [ ] Triage SLA breach escalates once and is audited.

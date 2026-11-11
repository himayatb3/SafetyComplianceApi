# Module: Near-miss (NMS-*)

Record type key: `nearmiss` · Depends on: 04, 05 · Status: Draft

## 1. Purpose
Capture events that could have caused harm/damage but did not. Strongest leading indicator; the **potential severity** drives investigation depth.

## 2. Permissions
`nearmiss.record.create` · `.view` · `.triage` · `.investigate` · `.close` · `.void` · `.reclassify`.

## 3. Fields
Required at create: site, occurredAt, what happened (short), reporter (or anonymous).
Optional: area/equipment, category, people involved (roles only unless permitted), contributing factors, photo, immediate action.
Required by triage: **potential severity** + likelihood (→ risk score), category.
Required to close: cause (taxonomy) when risk ≥ tenant threshold, ≥1 action or documented "no action needed" reason.

## 4. Workflow
`Reported → Triaged → (Investigation, if risk ≥ threshold) → ActionPending → Verification → Closed` (+ `Cancelled`, `Reclassified`)
Investigation depth by potential risk: Low = quick review; Medium = supervisor + simple RCA; High = full investigation (INC investigation tooling reused).

## 5. Rules
- NMS-001 Reporting must feel safe: anonymous allowed by tenant setting; no field implies blame; wording tested with "no-blame" guidelines.
- NMS-002 Risk score = potential severity × likelihood using the tenant matrix; matrix change does not rescore closed records (snapshot score on record).
- NMS-003 Potential severity ≥ "Serious" auto-escalates: notify HSE manager, require investigation, start SLA; behaves like an incident for oversight (G-09).
- NMS-004 Reporter gets feedback: on close, reporter-visible summary of actions taken (closes the loop, sustains reporting).
- NMS-005 Near-miss that is later found to have caused injury → reclassify to Incident (REC-005); original closed `Reclassified`, link kept. Metrics count it once (as incident).
- NMS-006 Recognition: optional "good catch" acknowledgement; no monetary reward logic in platform.
- NMS-007 Contractor near-misses on tenant site are tenant data; contractor company gets a read-only copy only if configured.

## 6. Edge cases
- NMS-E01 Reported days later from memory → `OccurredAt` approximate flag; late-report note; still accepted. T
- NMS-E02 Multiple witnesses file separate reports of same event → merge as duplicates (REC-006), keep all narratives as comments, count as one event. T
- NMS-E03 Reporter overstates/understates potential severity → HSE can adjust with reason; both reporter and final values stored.
- NMS-E04 Potential severity is "fatal" but nobody investigates (SLA breach) → escalates up reporting line then tenant HSE head; cannot be silently closed. T
- NMS-E05 Anonymous reporter wants to add info → receipt code thread (ACC-021).
- NMS-E06 Event involved equipment under a (future) asset register → link when available; free-text until then.
- NMS-E07 Near-miss at a client's site (not tenant's) → site field supports "external location" with disclosure rules.

## 7. Notifications & Insights
Events: reported, high potential (immediate), investigation due/overdue, closed → reporter feedback.
Rules fed: G-01, G-04, G-05, G-09, G-10, G-13, G-14.

## 8. API
Same shape as Observation under `/api/v1/near-misses` + `POST /{id}/risk-assessment`, `POST /{id}/investigation`.

## 9. Acceptance
- [ ] High potential near-miss cannot reach Closed without investigation or waiver.
- [ ] Reclassify to Incident links records and counts once.
- [ ] Reporter sees feedback after close; anonymous reporter via code.
- [ ] Matrix change leaves closed scores unchanged.

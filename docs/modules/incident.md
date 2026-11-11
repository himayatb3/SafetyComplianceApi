# Module: Incident (INC-*)

Record type key: `incident` · Depends on: 04, 05, 07 · Status: Draft
Regulatory detail is jurisdiction-pluggable (P7). Examples below are illustrative; **verify legal requirements per jurisdiction before shipping a pack.**

## 1. Purpose
Record events that caused or could reasonably have caused injury, illness, damage, spill/release, fire, or loss; investigate root cause; prevent recurrence; meet reporting duties.

## 2. Permissions
`incident.record.create` · `.view` · `.triage` · `.investigate` · `.approve` (HSE manager/site head) · `.close` · `.void` · `.regulatory.file` · `incident.pii.view` · `incident.confidential.view`.

## 3. Fields
Required at create (fast "first report"): site, occurredAt, what happened, incident type (injury/illness, property damage, environmental, fire, security, vehicle, other), immediate severity (Unknown allowed), reporter.
Enriched by HSE: people (injured/witness/involved/first-aider) with PII controls, injury details (body part, nature, treatment, lost time, days), damage, environmental release (substance, quantity, contained?), contractor involvement, equipment/area, weather/shift, immediate actions, photos, statements.
Investigation: team, timeline of events, root causes (taxonomy), contributing factors, 5-Why/fishbone attachments, findings, corrective/preventive actions.

## 4. Workflow
`Reported → Triaged → Investigation → Review/Approval → ActionPending → Verification → Closed` (+ `Cancelled`, `Reclassified`, `Reopened`)
- Immediate-response branch on create if severity ≥ threshold: alert chain, regulatory clock starts.
- First-report is deliberately minimal; triage completes classification within SLA (default 4 h for serious).

## 5. Rules
- INC-001 Severity classification scale is tenant-configurable but must map to a platform-defined tier set (`FirstAid, Medical, LostTime, Serious, Fatal` + non-injury tiers) so cross-tenant tooling and regulatory packs work.
- INC-002 Serious/Fatal creation triggers immediate notification chain (unmutable, REC-031), regulatory clock (WFL-033), and restricts record to need-to-know until triage (confidentiality of names).
- INC-003 Lost-time injuries track days away / restricted days; updates allowed after close via addendum, not edit of closure.
- INC-004 Investigation required for all ≥ Medical or tenant-defined; must have ≥1 root cause (taxonomy) and ≥1 corrective action before approval; low-severity may use short form.
- INC-005 Statements/evidence are append-only with author+time; hash on files (chain of custody, REC-009).
- INC-006 Injured person's medical data, treatment, and identity: PII fields (ACC-024), encrypted, audited reads; reports/dashboards use anonymised aggregates.
- INC-007 Regulatory filing record: jurisdiction pack defines forms, deadlines, authority; platform stores submission reference, date, proof; status feeds G-12. Filing is human-confirmed — platform prepares data, does not auto-submit in v1.
- INC-008 Closure needs: approved investigation, all actions Done (and Verified for ≥ Serious), regulatory filings complete or "not applicable" with reason, closer ≠ investigator.
- INC-009 Reopen (new info, injury worsens, death after injury) allowed with permission+reason; regulatory impact re-evaluated (death after injury restarts/adds clock).
- INC-010 Incident can spawn related incidents/sub-records (e.g. vehicle collision with 3 injured persons = one incident, three injured participants, one investigation).
- INC-011 Contractor incidents: record owned by host tenant; contractor company contact notified; contractor-employer-specific reporting duty flagged.
- INC-012 Environmental release: quantity, substance, reportable-quantity check via pack; escalate to environment role.
- INC-013 Cost capture optional (direct/indirect) for business-case dashboards; no financial system integration in v1.

## 6. Edge cases
- INC-E01 First report says "minor", later becomes fatal → reclassify severity (not type) with reason; clock logic re-run; timeline shows original. T
- INC-E02 Injured person unidentified / refuses to give details → record with "withheld"; no blocking. 
- INC-E03 Reporter is the injured person and also an anonymous-reporting user → identity rules per ACC-021 yet injured-person facts still needed; handled as participant with consent note.
- INC-E04 Multi-site / multi-company event (shared yard, joint venture) → primary owner site + `SharedWith` sites with read access; one investigation, one regulatory filing. T
- INC-E05 Police/authority holds evidence or requests records → legal hold (EDG-040) + export pack for authority with audit.
- INC-E06 Regulatory clock deadline falls within weekend/holiday → clock is calendar-hours per pack (not business hours) unless pack says otherwise (WFL-033).
- INC-E07 Investigator has conflict of interest (involved person or their manager) → system prevents assignment when participant relationship known; override needs approval.
- INC-E08 Investigation team member leaves mid-investigation → orphan rule (WFL-032); evidence preserved.
- INC-E09 Duplicate reports of same incident from several people → merge (REC-006); narratives preserved as statements. T
- INC-E10 Incident discovered long after the fact (e.g. occupational illness diagnosed later) → `OccurredAt` vs `DiscoveredAt` both captured; late-report flag; clocks from discovery per pack.
- INC-E11 Injured person is a visitor/third party/member of public → participant type `ThirdParty`; extra notification duties flagged; PII minimised.
- INC-E12 Root cause taxonomy changed after incident closed → old codes remain resolvable (soft-retire, never delete taxonomy values).
- INC-E13 Person dies, record contains health data and the family requests data → handled via legal process; platform provides export with approval; no self-service.

## 7. Notifications & Insights
Events: created (tiered), clock approaching/breached, investigation overdue, approval requested, action overdue, closed, reopened.
Rules fed: G-03, G-04, G-05, G-06, G-08, G-12, G-13.

## 8. API
`/api/v1/incidents` (CRUD-lite as per observation) · `POST /{id}/participants` · `POST /{id}/investigation` · `POST /{id}/root-causes` · `POST /{id}/regulatory-filings` · `POST /{id}/transitions` · `POST /{id}/reopen` · `POST /{id}/reclassify` · `POST /{id}/legal-hold`.

## 9. Acceptance
- [ ] Serious incident: alert chain fires within 60 s, clock starts, names hidden from out-of-scope users.
- [ ] Cannot close with open corrective actions / without root cause (unless waiver).
- [ ] Severity escalation after creation reruns clocks and notifications.
- [ ] PII reads audited; masked without permission.
- [ ] Multi-person incident handled as one record with N participants.

# 06 — Platform edge-case register (EDG-*)

Format: ID · scenario · required behaviour · test? (T = must have automated test). Module-specific cases live in `modules/*.md`.

## Tenancy & identity (EDG-001..019)
- EDG-001 · Request carries another tenant's record id · 404 (not 403), no timing difference · T
- EDG-002 · Background job runs without tenant context · fail fast; never run unfiltered · T
- EDG-003 · Tenant suspended while user has valid token · next request 403 `Tenant.Suspended`; reporting from public/QR link shows neutral notice · T
- EDG-004 · Same email in two tenants · allowed (TEN-020); login requires tenant resolution; password reset is per tenant · T
- EDG-005 · Email domain claimed by tenant SSO but user logs in with password · blocked if SSO enforced
- EDG-006 · Last Tenant Admin deactivated/demoted · rejected · T
- EDG-007 · User moves site/company · open assigned items re-evaluated against new scope; old records keep their original site; user keeps access to items they own until reassigned
- EDG-008 · User leaves company with open actions · orphan rule WFL-032 · T
- EDG-009 · Tenant renames site/company · history shows name-at-time via snapshot on records (store `SiteNameSnapshot`)
- EDG-010 · Site closed/decommissioned · read-only; open actions must be reassigned or bulk-closed with reason
- EDG-011 · Merge two sites/companies (acquisition) · admin tool with dry-run; records re-parented; numbers preserved; audit entry
- EDG-012 · Tenant data export of 100k records + 50 GB files · async job, resumable, signed expiring link
- EDG-013 · Tenant hits plan limit while someone is reporting a fatality · never block (TEN-016) · T
- EDG-014 · Clock skew on mobile (device time wrong) · server stamps `ReportedAt`; keep device time as `ClientTime` for forensics
- EDG-015 · Token replay after logout / refresh reuse · family revoked (ACC-010) · T
- EDG-016 · Sequence number collision under concurrency · unique constraint + retry; gaps OK, duplicates never · T
- EDG-017 · Tenant region (data residency) mismatch for a new integration/storage · deny; region immutable
- EDG-018 · Subdomain/tenant-code squatting or reuse after deletion · reserved list; codes not reusable for 12 months
- EDG-019 · Impersonation by platform staff · consent + time-box + visible banner + audit (TEN-022)

## Reporting & field reality (EDG-020..039)
- EDG-020 · Offline report with photos, sync hours later · client GUID idempotency; `OccurredAt` kept; attachments upload resumable, record visible before files complete · T
- EDG-021 · Same offline report submitted twice (retry/double tap) · one record (REC-011) · T
- EDG-022 · Offline edit conflicts with server triage edit · REC-010 conflict flow, nothing silently lost
- EDG-023 · Reporter unknown / no account (visitor, contractor walk-up) · QR code per site opens minimal public form (captcha, rate limit, no listing, optional contact); creates record with `ReporterType=External`
- EDG-024 · Spam/abuse on public form · rate limit per IP+site, honeypot, moderation flag; never auto-delete
- EDG-025 · Report contains PII of third party (injured person not user) · stored as `RecordParticipant` with PII controls (ACC-024)
- EDG-026 · Language: report in Hindi, investigator reads English · store original + language tag; optional machine translation shown as translation, original kept
- EDG-027 · Very long text / emoji / RTL / pasted HTML · length caps, sanitized rendering, no HTML execution (XSS) · T
- EDG-028 · Photo of the wrong thing / inappropriate image · attachment can be flagged/removed by permission; removal keeps hash+audit stub, not silent deletion
- EDG-029 · Incident happens across midnight / DST change / different time zone than reporter · occurredAt local + zone stored; regulatory clock uses site zone · T
- EDG-030 · Reporter wants to know outcome but anonymous · receipt code lookup (ACC-021)
- EDG-031 · Retaliation concern · reporter identity visible only to named roles; access reads audited
- EDG-032 · Record created for wrong site · move-site action (permission+reason); scope/notifications recomputed; old site loses access; audit shows move

## Workflow, actions, deadlines (EDG-040..059)
- EDG-040 · Legal hold on records · blocks edit-delete-purge and retention jobs; requires permission to set/release; audited · T
- EDG-041 · Retention expiry vs open action/legal hold · purge only closed, un-held, past-retention; purge writes tombstone
- EDG-042 · Workflow definition changed while records in flight · WFL-001 versioning; no record is stranded
- EDG-043 · Approver on leave / deactivated · delegation or role-queue fallback (WFL-007/032)
- EDG-044 · Action due date falls on holiday/weekend · site calendar; displayed as-is, SLA clocks use business calendar
- EDG-045 · Repeated due-date extensions · count surfaced; > N requires approval (REC-024); feeds Insights
- EDG-046 · Closing record with open actions · blocked unless waiver (REC-025) · T
- EDG-047 · Reopen after closure · permission + reason; reopened counter; original closure preserved in timeline
- EDG-048 · Circular dependencies between linked records/actions · rejected
- EDG-049 · Timer fires twice (worker redundancy) · idempotent by (timerId, firedAt) · T
- EDG-050 · Notification provider outage · queued retry; in-app always available; critical alerts escalate to alternate channel
- EDG-051 · Mass reassignment (100s of items) · batch job with preview and audit, not N requests
- EDG-052 · Regulatory deadline already passed at the time of classification · clock starts as breached, flagged, requires explanation field

## Data, API, ops (EDG-060..079)
- EDG-060 · Optimistic concurrency conflict · 409 + server state (REC-010) · T
- EDG-061 · Large list queries across years · mandatory date range or cursor; max page 100; export via async job
- EDG-062 · File storage up but DB down (or inverse) · upload finalise step; orphan sweeper job; DB is source of truth
- EDG-063 · Virus detected after file was shared · quarantine, notify participants, timeline note
- EDG-064 · Partial migration failure · forward-only, idempotent scripts; app/N-1 compatible
- EDG-065 · Webhook consumer down/slow · retry with backoff, dead-letter, per-endpoint circuit breaker, replay UI; signed payloads
- EDG-066 · API key leaked · scoped keys, rotation, instant revoke, last-used info
- EDG-067 · Report/export contains confidential records · filtered by requester's scope; export itself is audited
- EDG-068 · Bulk import (legacy data) · staged import with validation report, dry-run, mapping of legacy statuses, original numbers kept as `LegacyNumber`; imported records marked and excluded or flagged in metrics
- EDG-069 · Daylight/clock-driven jobs on tenants in many zones · schedule per tenant/site zone, not server zone
- EDG-070 · Soft-deleted user's email reused · allowed after anonymization window; history keeps user id + display snapshot
- EDG-071 · GDPR/DPDP erasure request vs safety/legal retention · pseudonymize personal fields, retain safety facts; document legal basis; decision logged
- EDG-072 · Injury/health data classed as special-category · encryption at rest (column-level for PII), minimal notification content, access audited

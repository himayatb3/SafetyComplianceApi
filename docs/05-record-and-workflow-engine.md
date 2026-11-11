# 05 — Record & workflow engine (REC-*, WFL-*)

Observation, Near-miss and Incident share one spine. Build it once.

## Safety Record (aggregate)
`SafetyRecord` = common envelope; type-specific data lives in validated custom fields + a few typed side tables.

| Field | Notes |
|---|---|
| Id, TenantId, RecordTypeId | RecordType = Observation / NearMiss / Incident / (future) |
| Number | Human id, per tenant+type+year sequence (TEN numbering config) |
| CompanyId, SiteId, AreaId? | Where. Site required; area optional |
| OccurredAt (UTC) + OccurredAtLocal + SiteTimeZone | When it happened (may be backdated); `ReportedAt` = server time |
| ReportedBy / IsAnonymous / ReporterType | Employee, contractor, visitor, anonymous |
| Title, Description | Free text, language tag |
| Category / SubCategory | Tenant taxonomy |
| Severity (actual), PotentialSeverity, Likelihood, RiskScore | Matrix configured per tenant |
| Status, StatusReason | From workflow |
| Assignee(s), Owner | Responsible people / role queue |
| Confidential | Flag (ACC-020) |
| CustomFields (json), SchemaVersion | Form data |
| Links | Related records, actions, assets, people, permits |
| RowVersion, IsDeleted | Concurrency, soft delete |

Side entities: `RecordParticipant` (injured, witness, involved, supervisor), `RecordAttachment`, `RecordComment`, `RecordTimelineEvent`, `RecordLink`, `ActionItem`.

### Rules
- REC-001 Create requires only: type, site, occurredAt, short description (P1). Everything else optional at creation, **required by workflow gates** at later stages.
- REC-002 `OccurredAt` ≤ now + 5 min clock skew; may be past (no limit, but > 30 days back flags "late report").
- REC-003 Edit window: reporter may edit own record until triage; afterwards only via permissioned edit with reason; all edits versioned (field diff in timeline).
- REC-004 Records are never hard-deleted. "Delete" = void with reason + permission; voided records stay in audit and are excluded from metrics by default.
- REC-005 Reclassification (e.g. Near-miss → Incident, Observation → Near-miss): creates a **new** record linked to the original (original closed as `Reclassified`), preserving timeline; numbers are not reused.
- REC-006 Duplicate detection at submit: same site + type + similar time window (±2 h) + text similarity → soft warning with "link as duplicate" (never block safety reports).
- REC-007 Linking: parent/child, duplicate-of, related-to, caused-by. Cycles rejected. Cross-tenant links impossible (TEN-008).
- REC-008 Custom fields: tenant-defined per record type; types text/number/date/select/multi/person/site/file/geo; required-by-stage; versioned schema; old records keep their schema version; removing a field hides it, never deletes data.
- REC-009 Attachments: ≤ 25 MB each (configurable), allow-listed MIME by content sniffing, virus scan before availability, EXIF GPS stripped optionally, signed short-lived URLs, hash stored (chain of custody for evidence).
- REC-010 Concurrency: updates require `RowVersion`; conflict → 409 with current server state; field-level merge for non-overlapping edits offered in UI.
- REC-011 Idempotent create via `Idempotency-Key`; offline clients supply client-generated GUID as `ClientRequestId` (unique per tenant).

## Actions (CAPA) — shared across all modules
`ActionItem`: Id, TenantId, Number, Source record(s), Type (Immediate/Corrective/Preventive), Description, Owner, DueDate, Priority, Status (Open→InProgress→Done→Verified | Rejected | Cancelled), CompletionNotes, Evidence, VerifiedBy.
- REC-020 An action may link to many records; a record may have many actions.
- REC-021 Due date required; cannot be in the past at creation (except immediate actions marked "already done").
- REC-022 Overdue = past due and not Done; escalation per WFL-030.
- REC-023 Verification by someone other than owner (ACC-022). Failed verification reopens with reason.
- REC-024 Reassign/extend due date requires reason; extensions beyond N need approval; every change audited.
- REC-025 Closing a record requires all its **linked corrective actions** Done (Verified if tenant setting), unless explicit waiver with approver + reason.

## Workflow engine
Per tenant, per record type: `WorkflowDefinition` (exists) → versioned states, transitions, guards, effects. Seeded from system templates; editable within safety rails.

```
State{Key, Name, Category(Open|InProgress|Closed|Cancelled), SlaMinutes?, RequiredFields[], EntryRoles[]}
Transition{From, To, Permission, Guards[], RequiredFields[], RequiresReason, Effects[]}
Guard: field present · role/person condition · all actions done · approval obtained · custom expression (safe DSL)
Effect: notify · assign · create action · start timer · call webhook · set field
```
- WFL-001 Workflow definitions are **versioned**; a record stays on the version it started with unless migrated by an admin tool (mapping old→new states required).
- WFL-002 Every transition: authorize (`ACC`), evaluate guards, write timeline + audit, enqueue effects via outbox — one transaction.
- WFL-003 Invalid/unknown transition → 409 `Workflow.InvalidTransition`; never silently coerce.
- WFL-004 Published workflow must validate: one start state, reachable end states, no dead states, every state has an exit or is terminal, SLA/escalation targets exist.
- WFL-005 Terminal states (`Closed`, `Cancelled`) are immutable except explicit **Reopen** transition (permission + reason + counts toward metrics).
- WFL-006 Parallel steps (e.g. investigation + medical follow-up) modelled as sub-tasks (actions), not parallel states; keeps engine simple.
- WFL-007 Approvals: single, any-of-N, all-of-N, sequential; approver unavailable → delegate (WFL-032) or role-queue fallback.

### SLA, escalation, delegation
- WFL-030 SLA timers per state (business-hours calendar per site; pause on "Awaiting info"). Breach → escalate up reporting line / role; repeated breach → next level; all logged.
- WFL-031 Timers are durable (DB-backed), idempotent, survive restarts; evaluated by worker with tenant context; skew tolerant ±1 min.
- WFL-032 Delegation: user may delegate for date range; auto-delegate when user deactivated/on leave; **orphan rule**: records/actions assigned to a deactivated user are reassigned to their manager or the site HSE role queue within 1 h and flagged.
- WFL-033 Regulatory clocks (e.g. notify authority within X hours) are SLA timers supplied by jurisdiction packs; cannot be disabled by tenants, only acknowledged.

## Notifications
- Channels: in-app, email, push, SMS (optional, cost-gated), webhook. Templates per tenant+locale; variables whitelisted.
- REC-030 Rules: event → recipients (role / assignee / reporter / reporting line / site role) → channel → throttle/digest.
- REC-031 Users control non-critical preferences; **critical safety notifications (serious incident, regulatory clock, overdue critical action) cannot be muted**.
- REC-032 Delivery tracked (queued/sent/failed/read); retries with backoff; failure of notification never fails the business transaction (outbox).
- REC-033 Never put confidential/PII content in push/SMS/email subject; send link + generic text.

## Timeline
Single immutable feed per record: status changes, comments, field edits, attachment adds, action events, notifications sent. Comments support @mention; internal-only vs reporter-visible comments (reporter feedback loop — P1).

# 02 — High-Level Design

## 1. Context
Clients: Web SPA (admin/office), Mobile app/PWA (field, offline-capable), External integrations (webhooks/API keys, SSO).
Backend: one modular monolith (ASP.NET Core). Split into services only when a module's scaling/ownership demands it (see decisions ADR-002).

```
 Clients ──HTTPS──► API (auth, tenant resolve, rate limit)
                       │
              Application (use cases, authz, workflow, validation)
                       │
        ┌──────────────┼───────────────────────────┐
   Persistence     Infrastructure              Background workers
 (SQL Server/EF)  (JWT, hashing, blob,        (SLA timers, notifications,
                   email/SMS/push, queue)      insights, exports, outbox)
```

## 2. Layering (existing solution, keep)
- **Domain** — entities, value objects, domain events, invariants. No dependencies.
- **Application** — use cases (command/query handlers), DTOs, interfaces (`IRepository`, `IClock`, `ICurrentUser`, `IBlobStore`, …), authorization, validation, `Result<T>`.
- **Persistence** — EF Core `DbContext`, configurations, migrations, repositories, tenant query filters.
- **Infrastructure** — JWT, password hashing, file storage, notification channels, external adapters.
- **API** — controllers (thin), auth setup, middleware, versioning, OpenAPI.
- Dependency rule: Domain ← Application ← {Persistence, Infrastructure, API}. Application never references EF/Dapper/ASP.NET.

## 3. Platform building blocks (build once, reuse everywhere)
| Block | Purpose | Doc |
|---|---|---|
| Tenant & Org structure | Tenant → Company → Site → Area | 03 |
| Identity & Access | Users, roles, permissions, data scope | 04 |
| Record engine | Generic safety record, custom fields, timeline | 05 |
| Workflow engine | Per-tenant state machines, SLA, escalation | 05 |
| Actions (CAPA) | Shared corrective/preventive tasks, linkable to any record | 05 |
| Attachments | Photos/video/docs, virus scan, signed URLs | 05 |
| Notifications | In-app, email, push, SMS; templates, preferences | 05 |
| Audit & history | Immutable change log | 04/05 |
| Insights | Metrics, rules, gap alerts, dashboards | 07 |
| Config & taxonomy | Tenant-specific lists, numbering, forms, locales | 03/05 |
| Integration | Webhooks, API keys, SSO (OIDC/SAML), export | 06 |

## 4. Data
- SQL Server, **shared database + shared schema, `TenantId` on every tenant row** (ADR-001). Dedicated-DB option for enterprise tenants later via tenant catalog.
- Custom fields stored as validated JSON (`nvarchar(max)` + schema version) with promoted/indexed columns for filterable fields.
- Files in object storage (blob), DB holds metadata + hash. Never store files in SQL.
- Optimistic concurrency (`rowversion`) on all editable aggregates.
- Outbox table for reliable events (notifications, webhooks, insights).
- Read models for dashboards (pre-aggregated per tenant/site/day), not live scans.

## 5. Request flow (every authenticated request)
1. Rate-limit → authenticate JWT → **resolve tenant from claim** (reject if tenant suspended/deleted).
2. Set `ICurrentUser` (UserId, TenantId, CompanyIds, SiteScope, permissions).
3. Controller → Application handler → authorize (`permission` + `scope`) → validate → execute → audit → outbox.
4. EF global query filter appends `TenantId = @current` (TEN-003); Row-Level Security as backstop (TEN-005).
5. Response wrapped consistently (`ProblemDetails` for errors, single success envelope), includes `traceId`.

## 6. Cross-cutting
- **Observability:** structured logs (Serilog) with TenantId/UserId/TraceId; metrics; health checks (`/health/live`, `/health/ready`).
- **Security:** OWASP ASVS L2 target; secrets in vault/env; TLS only; CORS allow-list per environment; no PII in logs.
- **Time:** store UTC; each Site has an IANA time zone; display converts; regulatory clocks computed in site time.
- **i18n:** all user-facing text keyed; tenant default locale + per-user override; RTL-ready.
- **Versioning:** URL `/api/v1`; additive changes only within a version.
- **Idempotency:** `Idempotency-Key` header on create endpoints (mobile retries).
- **Pagination:** cursor-based for timelines, page+size for admin lists; hard max page size 100.
- **Background jobs:** durable queue (Hangfire/Quartz or cloud queue — ADR-004); jobs carry TenantId and re-establish tenant context.

## 7. Non-functional targets (NFR)
- NFR-001 99.9% monthly availability for reporting endpoints.
- NFR-002 P95 < 400 ms for create/read of a single record; dashboards < 2 s from read models.
- NFR-003 Report submission works offline; syncs within 30 s of connectivity with no duplicates.
- NFR-004 RPO ≤ 15 min, RTO ≤ 4 h; per-tenant restore procedure documented.
- NFR-005 Support 5,000 tenants, 500 users/tenant avg, 5,000 records/tenant/year without re-architecture.
- NFR-006 Records retained per tenant policy (default 7 years); audit log retained ≥ record life.

## 8. Environments & delivery
Local → Dev → Staging → Prod. CI: build, unit + integration tests (SQL Server container), tenant-isolation test suite (mandatory gate), vulnerability scan, migration dry-run.
Migrations are forward-only, backward-compatible with N-1 app version (zero-downtime deploys).

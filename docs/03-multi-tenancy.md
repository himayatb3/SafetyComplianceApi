# 03 — Multi-tenancy (TEN-*)

## Hierarchy
```
Tenant (the SaaS customer account, billing + isolation boundary)
 └─ Company (legal entity / business unit; ≥1 per tenant)
     └─ Site (plant, project, office; has time zone, address, jurisdiction)
         └─ Area / Location (building, unit, line, zone — optional tree)
```
`Tenant` is NEW (current code starts at `Company`). Add `TenantId` to every entity (introduce `TenantEntity : BaseEntity`).
Platform-owned (non-tenant) tables: `Tenant`, `Plan`, `Module` catalog, jurisdiction packs, system role templates.

## Rules
- TEN-001 Tenant identity comes ONLY from the authenticated token (`tenant_id` claim) or a verified host mapping. Never from body/query/header supplied by the client.
- TEN-002 Every tenant-owned entity inherits `TenantEntity` (`TenantId` non-null, indexed, part of every unique index).
- TEN-003 EF Core global query filter on `TenantId` for all `TenantEntity` types. `IgnoreQueryFilters()` allowed only in platform-admin code paths, behind a named service and audited.
- TEN-004 `SaveChanges` stamps `TenantId` on insert and **rejects** any entity whose `TenantId` ≠ current tenant (also on update/delete).
- TEN-005 Defense in depth: SQL Server Row-Level Security policy using `SESSION_CONTEXT('TenantId')` set per connection. Dapper/raw SQL must go through the tenant-aware connection factory; raw SQL without `TenantId` predicate fails code review.
- TEN-006 Uniqueness (username, email, employee code, record number, role code) is **per tenant**, case-insensitive. Exception: login identity — see TEN-020.
- TEN-007 Cache keys, blob paths, search indexes, queue messages, logs, and background jobs all carry TenantId. Blob path: `/{tenantId}/{module}/{recordId}/{fileId}`.
- TEN-008 Cross-tenant references are impossible: FKs between tenant tables include `TenantId` (composite) or are validated in Application.
- TEN-009 A mandatory automated **tenant-isolation test suite** (two tenants, every endpoint, every entity) gates CI.

## Lifecycle
`Provisioning → Trial → Active ⇄ Suspended → Offboarding → Deleted`
- TEN-010 Provisioning is idempotent and transactional: tenant row, default company/site placeholder, system roles cloned from templates, first admin invited, default taxonomies + workflows seeded.
- TEN-011 Suspended: reads allowed for admins only (export), writes blocked, reporters see a maintenance notice; reporting links do not leak tenant existence.
- TEN-012 Offboarding: full export (JSON + files) available 30 days; then hard delete after grace period, with certificate of deletion. Legal hold (EDG-040) blocks deletion.
- TEN-013 Trial → Active upgrade keeps all data; Active → lower plan never deletes data (read-only when above limits).

## Plans & entitlements
- TEN-014 `Plan` → set of `Module` entitlements + limits (users, sites, storage GB, API calls). `TenantModule` row = enabled module. Enforced in Application (not only UI): disabled module endpoints return 403 `Module.NotEnabled`.
- TEN-015 Feature flags per tenant for gradual rollout; flags are server-evaluated.
- TEN-016 Limit breach behaviour is defined per limit (soft-warn, hard-block-new, never-block-reporting). **Safety reporting is never blocked by billing limits** (hard rule).

## Identity across tenants
- TEN-020 Login identity = email + tenant. A person in two tenants has two `User` rows (no shared account in v1). Login screen resolves tenant by subdomain (`acme.app.com`), tenant code, or an email-domain lookup; ambiguous → ask for tenant code.
- TEN-021 Optional per-tenant SSO (OIDC/SAML) with enforced-SSO setting; JIT user provisioning maps IdP groups → roles.
- TEN-022 Platform operators use a separate identity realm; impersonation/"support access" requires tenant-admin consent, time-boxed, fully audited, and visible to tenant.

## Tenant configuration (all versioned, all audited)
Locale/time zone defaults · fiscal calendar · record numbering (`INC-{SITE}-{YYYY}-{SEQ}`, sequence per tenant+type+year, gap-tolerant but unique) · taxonomies (hazard categories, injury types, body parts, root-cause codes, severity/likelihood matrix) · custom fields per record type · workflows · notification rules · retention · branding · data-residency region (set at provisioning, immutable).

## Edge cases specific to tenancy → see EDG-001..EDG-019 in 06

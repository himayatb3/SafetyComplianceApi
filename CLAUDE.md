# CLAUDE.md — AI entry point (keep this file short)

Product: multi-tenant SaaS for HSE (Health, Safety, Environment). Goal: simplify HSE work and
show each establishment where it is lacking. Stack: .NET 9, ASP.NET Core, EF Core (SQL Server),
Clean Architecture (API → Application ← Domain; Infrastructure and Persistence implement Application).

## How to work in this repo (token-efficient)
1. Do NOT read all of `docs/`. Read `docs/README.md` (index, ~1 page), then ONLY the files your task names.
2. Rules have IDs (e.g. `TEN-003`, `INC-012`). Tasks reference IDs; follow them exactly, do not re-derive.
3. Implementing a module: read `docs/ai/coding-rules.md` + that module's file in `docs/modules/`. Nothing else unless it links you.
4. Use `docs/ai/task-template.md` to scope work. One task = one vertical slice = one commit.
5. If a rule is missing or conflicts with code, STOP and ask; propose a doc change before changing code.
6. Docs are the source of truth. Change a rule → update the doc in the same commit.

## Non-negotiables (full list: docs/ai/coding-rules.md)
- Every tenant-owned row has `TenantId`; every query is tenant-filtered. Tenant comes from the JWT, never from the request body (TEN-001..004).
- `CreatedBy`/`CompanyId`/`SiteId` of the actor come from claims, never from the request body.
- Authorization by permission (`module.resource.action`) + data scope, enforced in Application layer (ACC-001..).
- Timestamps UTC (`DateTimeOffset`/`DateTime.UtcNow`). Soft delete only. Audit every state change.
- No secrets in git. No `obj/` or `bin/` in git. No logic in controllers.
- Every feature ships with tests for its edge cases listed in the spec.

## Layout
`SafetyCompliance/` solution: API, Application, Domain, Infrastructure, Persistence. Docs in `docs/`.

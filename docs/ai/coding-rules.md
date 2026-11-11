# AI/Developer coding rules (COD-*) — mandatory for every change

## Architecture
- COD-001 Dependency rule: Domain ← Application ← {Persistence, Infrastructure, API}. Application has no EF/Dapper/ASP.NET references.
- COD-002 Organise Application by **feature** (vertical slice): `Application/Features/<Module>/<UseCase>/{Command|Query, Handler, Validator, Dto}`. One use case per file group.
- COD-003 Controllers: map HTTP → use case → HTTP. No logic, no repositories, no `DbContext`.
- COD-004 Use cases return `Result<T>`; expected failures are `ApplicationError` (code, message, type). Exceptions only for bugs/infrastructure.
- COD-005 New module = new feature folder + record type seed + workflow template + permissions constants + doc spec. No copy-paste of another module's handlers; extend the engine.

## Tenant & security (never skip)
- COD-010 Entities inherit `TenantEntity`; no raw SQL without tenant predicate (TEN-005).
- COD-011 Take `TenantId`, `UserId`, `CompanyId`, `SiteScope` from `ICurrentUser`. Request DTOs must NOT contain `TenantId`, `CreatedBy`, or other identity fields.
- COD-012 Every use case calls `authorizer.Require(permission, scope)` first; list queries apply scope predicates.
- COD-013 No secrets in repo (appsettings holds placeholders; real values via env/user-secrets/vault). No `obj/`, `bin/`, `.user` files committed.
- COD-014 Validate all input (length, format, enum range, collection size). Never trust client ids for FKs — verify they belong to the tenant and scope.
- COD-015 Never log PII, tokens, passwords, or confidential record text.

## Data
- COD-020 Time: store UTC (`DateTime.UtcNow` via `IClock`); never `DateTime.Now`. Site-local time computed from site time zone.
- COD-021 Soft delete only; audit fields set by `SaveChanges` interceptor (Created/Modified On/By, TenantId, RowVersion) — handlers do not set them.
- COD-022 Enums stored as strings or stable ints with explicit values; taxonomy values are rows, never deleted (retire flag).
- COD-023 Migrations: one per change, named `YYYYMMDD_<what>`, backward-compatible with previous app version; seed via migrations/idempotent seeders.
- COD-024 No N+1: use projection (`Select` to DTO), `AsNoTracking` for reads, paging mandatory on lists.
- COD-025 Concurrency token on editable aggregates; handle `DbUpdateConcurrencyException` → 409.

## API
- COD-030 Route `/api/v1/<plural-kebab>`; verbs by REST; create → 201 + `Location`; errors via `ProblemDetails` with `traceId` and error `code`.
- COD-031 Create endpoints accept `Idempotency-Key`; list endpoints support `cursor/limit` (max 100) and filters documented in OpenAPI.
- COD-032 DTOs separate from entities; never expose entities or internal flags (hash, salt, otp, refresh token).

## Quality
- COD-040 Each use case has unit tests for rules + edge cases from the spec (IDs in test names, e.g. `INC_E01_Severity_Escalation_Reruns_Clock`).
- COD-041 Integration tests run against real SQL Server container; tenant-isolation suite must pass (TEN-009).
- COD-042 Methods ≤ ~40 lines, no commented-out code, no TODO without issue ref. Names: PascalCase types/methods, `_camel` or `camel` fields consistently; async methods end `Async` and take `CancellationToken`.
- COD-043 Definition of Done: builds with warnings-as-errors for nullable; tests pass; docs/spec updated; migration included; permissions seeded; audit + notification hooks present; PR description lists rule IDs implemented.

## Known debt to remove in Phase 0 (do not copy these patterns)
Anonymous endpoints & unused JWT · request-supplied `CreatedBy/CompanyId` · committed signing key & `obj/` · `DateTime.Now` · undisposed Dapper connections · fake-entity delete · no lockout counting · invalid connection string · weather sample.

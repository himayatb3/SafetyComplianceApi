# 04 — Access control (ACC-*)

Existing entities already model: Role, UserRole, Module, MenuItem, RoleMenuAccess, UserMenuAccessOverride, WorkflowDefinition, RoleWorkflowAccess, UserWorkflowAccessOverride. This doc keeps that direction and adds **permissions** and **data scope**.

## Model: Permission × Scope
A user may do action A on resource R **iff** they hold permission `module.resource.action` through a role (or override) **and** the target record falls within the permission's **scope**.

- Permission string: `incident.record.create`, `incident.record.investigate`, `action.item.close`, `admin.user.manage`. Catalogue is code-defined (constants) and seeded; tenants cannot invent permissions, only compose roles.
- Scope levels (per role assignment): `Tenant` · `Company(ids)` · `Site(ids)` · `Area(ids)` · `Own` (records I created / am assigned) · `Team` (my reports, via reporting line).
- ACC-001 Authorization runs in the Application layer on every use case (`IAuthorizer.Require(permission, target)`); controllers carry `[Authorize]` only for authentication.
- ACC-002 Deny by default. No permission → 403. Scope miss → **404** for reads of single records (don't confirm existence), 403 for writes on visible records.
- ACC-003 List endpoints apply scope as a query predicate (never fetch-then-filter).
- ACC-004 Menu/UI visibility (`RoleMenuAccess`) is a convenience layer derived from permissions; it is never the security boundary.
- ACC-005 Per-user overrides (`...Override`) are explicit allow/deny with reason, optional expiry, and appear in an admin review report. Deny beats allow.
- ACC-006 System roles (Tenant Admin, HSE Manager, HSE Officer, Site Head, Supervisor, Reporter, Contractor, Read-only Auditor) are templates cloned per tenant; tenants may clone/edit copies but **cannot remove the last Tenant Admin** or grant platform permissions.
- ACC-007 Role changes take effect within 60 s (token carries a `perm_version`; server compares to cached tenant-user version, forces refresh).

## Authentication
- ACC-010 JWT access token (≤15 min) + rotating refresh token (hashed at rest, single-use, family revocation on reuse). Claims: `sub`, `tenant_id`, `perm_version`, `sid` (session). Roles/permissions are NOT stuffed in the token.
- ACC-011 Validate issuer, audience, lifetime, signature (key from vault, rotatable via `kid`).
- ACC-012 Passwords: PBKDF2 ≥ 600k iterations or Argon2id; min length 12 recommended, breached-password check, history of last 5; tenant may tighten policy.
- ACC-013 Lockout: N failed attempts (default 5) → progressive delay/temporary block; counters per user **and** per IP; generic error text (no user enumeration; same latency).
- ACC-014 MFA (TOTP) optional per user, enforceable per tenant/role. Temp-password users must change password before any other call (`MustChangePassword` enforced server-side, not UI-only).
- ACC-015 Self-service reset via single-use, short-lived, hashed token; never reveals whether email exists.
- ACC-016 Session list + remote revoke; deactivating a user revokes all sessions immediately.

## Special visibility
- ACC-020 **Confidential records** (e.g. harassment, medical detail, whistleblower): record flag + permission `*.confidential.view`; restricted to named participants. Hidden from lists, search, dashboards counts for others.
- ACC-021 **Anonymous reporting** (tenant setting): reporter identity is not stored with the record (or stored encrypted, break-glass with justification — decided per tenant). Follow-up via anonymous receipt code.
- ACC-022 **Separation of duties** (configurable): reporter ≠ investigator ≠ closer; the person who closed an action cannot verify it.
- ACC-023 External users (contractors/visitors): own-records-only scope, no directory access, optional magic-link login.
- ACC-024 Field-level protection: injured person's medical/PII fields need `*.pii.view`; masked otherwise; reads of PII are audited.

## Audit
- ACC-030 Append-only `AuditLog`: who (user/session/IP/device), tenant, what (entity, id, action), before/after (field diff), when (UTC), why (reason where required), trace id. No update/delete API; DB permissions enforce.
- ACC-031 Audited: auth events, permission changes, every record state transition, PII reads, exports, impersonation, config changes.
- ACC-032 Audit reads are themselves permissioned (`admin.audit.view`).

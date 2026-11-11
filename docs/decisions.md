# Decisions (ADR log) and open questions

Status: Proposed → Accepted → Superseded. Owner sign-off needed for Proposed.

## ADR-001 Tenancy model — Proposed
Shared DB, shared schema, `TenantId` everywhere, EF global filter + SQL RLS. Dedicated DB per tenant as an enterprise option via tenant catalog (connection resolved per request). Reason: cost at 1000s of small tenants, simple ops; isolation enforced in depth. Trade-off: noisy neighbour, per-tenant restore is harder (needs tooling, NFR-004).

## ADR-002 Modular monolith first — Proposed
One deployable, modules as folders/namespaces with explicit boundaries. Extract workers (notifications, insights) first if needed. Reason: small team, fast change, simpler transactions (workflow + audit + outbox).

## ADR-003 Generic Record engine over per-module tables — Proposed
Common `SafetyRecord` + validated JSON custom fields + typed side tables for query-critical data (participants, injuries, root causes). Reason: new modules cheap, tenants configure forms. Trade-off: reporting on JSON needs promoted columns/read models.

## ADR-004 Background jobs / queue — Open
Options: Hangfire (SQL-backed, simplest), Quartz + outbox, or cloud queue. Recommendation: outbox table + Hangfire/Quartz in v1.

## ADR-005 Auth — Proposed
Own identity (JWT + rotating refresh) in v1, OIDC/SAML SSO per tenant in Phase 2; do not adopt a third-party IdP until requirements (SCIM, MFA breadth) demand it. Keep behind `IAuthService` so swap is possible.

## ADR-006 Offline — Proposed
PWA first (service worker, IndexedDB queue, client GUIDs) for reporting flows only; native app later if hardware (NFC/camera) needs it.

## ADR-007 Hosting/region — Open
Pick cloud + regions; data-residency region chosen at tenant provisioning and immutable (EDG-017).

## Open questions for the product owner (answers change the docs)
1. **Target market / jurisdictions first?** (India — Factories Act/OSHA-style, EU, US, GCC) → drives regulatory packs and data-residency.
2. **Is "Company" a legal entity under a tenant** (as assumed) or is Company == Tenant today? Current code treats Company as top-level.
3. **Anonymous reporting:** truly unlinkable, or encrypted-with-break-glass? Default proposed: tenant chooses.
4. **Who are the first 3 pilot customers/industries** (construction, manufacturing, oil & gas, hospitality)? Taxonomy/forms differ.
5. **Offline requirement for v1** — mandatory or Phase 2?
6. **Languages** needed at launch?
7. **Pricing unit** (per user / per site / per module)? Affects entitlements (TEN-014).
8. **Regulatory auto-filing** ever in scope, or "prepare and track" only?
9. **Mobile**: PWA acceptable for v1?
10. **Data retention default** (7 y proposed) and legal-hold needs?
11. **Contractor model**: separate tenant, external user in host tenant (proposed), or both?
12. **Workflow editor for tenants in v1**, or templates with limited toggles first (proposed: toggles first)?

## Phase plan
- **Phase 0 — Foundation hardening:** remove `obj/`, rotate key, real JWT auth, `TenantEntity` + filters + isolation tests, `ICurrentUser`, audit interceptor, migrations, CI.
- **Phase 1 — Platform:** tenant provisioning, org structure, IAM (roles/permissions/scope), taxonomy/config, attachments, notifications, outbox/worker, record+workflow engine, actions.
- **Phase 2 — Modules:** Observation → Near-miss → Incident (in that order; each reuses engine) + basic dashboards.
- **Phase 3 — Insights & Inspections.**
- **Phase 4+ — roadmap.**

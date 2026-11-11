# 01 — Product vision

## Problem
HSE work in most establishments is paper/Excel based: reports arrive late, actions are lost, management sees problems only after an incident,
and compliance proof is rebuilt by hand at audit time. Field staff avoid reporting because forms are long.

## Vision
One simple platform where (1) anyone can report in under 60 seconds, (2) the right people are pulled in automatically,
(3) nothing falls through (actions, deadlines, escalations), and (4) leadership sees **where the establishment is lacking** without building reports.

## Principles (these drive trade-offs)
- P1 **Simplest path for the reporter.** Minimum required fields; everything else is enrichable later by the safety team.
- P2 **Configuration over code.** Tenants configure forms, taxonomies, workflows, numbering; engineers add modules, not per-customer forks.
- P3 **Tenant isolation is absolute.** A leak between tenants is a P0 product-ending bug.
- P4 **Evidence by default.** Every change is audited; records are defensible in a regulatory inquiry or litigation.
- P5 **Works where HSE happens.** Mobile-first, poor connectivity tolerated, multilingual, usable with gloves/one hand.
- P6 **Leading over lagging.** Prefer surfacing weak signals (overdue actions, no observations) over counting injuries.
- P7 **Don't over-ask the law.** Jurisdiction rules (reporting clocks, forms) are pluggable packs, not hard-coded.

## Personas
| Persona | Needs |
|---|---|
| Worker / reporter | Report hazard/near-miss/incident fast, optionally anonymous, see feedback on what happened |
| Supervisor | Review team reports, assign quick actions, see site status |
| HSE officer | Triage, investigate, run CAPA, regulatory reporting |
| Site/Plant head | Site dashboard, overdue items, sign-offs |
| Corporate HSE / executive | Cross-site comparison, risk heat, trends, gap alerts |
| Contractor / visitor | Limited external reporting, restricted visibility |
| Tenant admin | Users, roles, structure, configuration, plan |
| Platform operator (us) | Provision tenants, plans, support access (audited), health |
| Auditor / regulator (read-only) | Time-boxed read access to evidence |

## Module scope
**Phase 1 (first release):** Observation, Near-miss, Incident, Corrective/Preventive Actions (shared), Notifications, Basic dashboards.
**Phase 2:** Insights/gap detection, Inspections & checklists, Investigation tools (5-Why, fishbone).
**Phase 3+ ("beyond"):** see [modules/roadmap.md](modules/roadmap.md).

## Out of scope (for now)
Payroll/HR, medical records system (we store only incident-related injury facts), IoT sensor ingestion, native desktop apps.

## Success metrics
Median time-to-report < 60 s · % actions closed on time · observation-to-incident ratio per site · time-to-first-triage · tenant onboarding < 1 day.

# Beyond the first three: module roadmap

Each new module = record type + workflow template + form + taxonomy + (optional) side tables + Insights rules. Use `_template.md`. Order is a proposal — validate with customers.

| Priority | Module | Why it fits | Reuses |
|---|---|---|---|
| P2-1 | **Inspections & Audits** (checklists, scheduled rounds, scoring) | Proactive; produces findings → actions; feeds coverage rules | Record, Actions, SLA, Insights |
| P2-2 | **CAPA register** (standalone view) | Single place for all actions across modules | Actions |
| P2-3 | **Investigation toolkit** (5-Why, fishbone, timeline, fault tree) | Quality of root cause = quality of prevention | Incident |
| P3-1 | **Risk assessment / JSA / HIRA** | Core HSE artefact; links hazards to controls | Record, taxonomy, risk matrix |
| P3-2 | **Permit to Work** (hot work, confined space, height, LOTO) | High-risk work control; time-boxed approvals | Workflow, approvals, SLA timers |
| P3-3 | **Training & competency** (matrix, expiry, induction) | Expired training = lagging cause; G-rules | Users, timers, notifications |
| P3-4 | **Contractor management** (onboarding, docs expiry, performance) | Contractors drive many incidents | External users, expiry timers |
| P4-1 | **Asset / equipment register + calibration/maintenance** | Link events to equipment; PM compliance | Links, timers |
| P4-2 | **Chemical / SDS register & exposure** | Regulatory + health | Taxonomy, documents |
| P4-3 | **Occupational health** (surveillance, exposure monitoring) | Health, with strict PII | ACC-024 |
| P4-4 | **Environmental** (emissions, waste, spills, permits) | The "E" in HSE | Record, timers |
| P4-5 | **Legal & compliance register** (obligations, applicability, evidence) | Prove compliance | Actions, timers |
| P4-6 | **Emergency preparedness** (drills, plans, muster) | Drills as scheduled records | Timers |
| P4-7 | **Management of Change (MoC)** | Approval-heavy workflow | Workflow |
| P5 | **Management review / KPIs / ESG export**, **AI assistant** (summarise, classify, suggest root cause — with human confirmation) | Differentiators | Insights |

## Platform enablers needed by these
Scheduled/recurring record generation · document library with expiry · QR/NFC tags for areas/equipment · public forms · calendar · geofencing · e-signature · SSO/SCIM · BI export/API · white-label.

## Module admission checklist
1. Spec file from template, reviewed. 2. Permissions catalogued. 3. Workflow template + SLA. 4. Edge cases with T flags. 5. Insights hooks. 6. Tenant-isolation tests extended. 7. Seed data + docs index updated.

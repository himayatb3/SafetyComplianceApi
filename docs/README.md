# Documentation index

Status: **DRAFT v0.1** — blueprint stage, no feature code yet. Decisions marked `Proposed` in `decisions.md` need owner sign-off.

| # | File | Read when |
|---|------|-----------|
| 01 | [product-vision.md](01-product-vision.md) | Scope, personas, principles, roadmap |
| 02 | [hld.md](02-hld.md) | System shape, components, cross-cutting concerns |
| 03 | [multi-tenancy.md](03-multi-tenancy.md) | Anything touching tenant data, provisioning, plans (TEN-*) |
| 04 | [access-control.md](04-access-control.md) | Auth, roles, permissions, data scope (ACC-*) |
| 05 | [record-and-workflow-engine.md](05-record-and-workflow-engine.md) | The shared engine every module is built on (REC-*, WFL-*) |
| 06 | [platform-edge-cases.md](06-platform-edge-cases.md) | Cross-module edge case register (EDG-*) |
| 07 | [insights-and-gap-detection.md](07-insights-and-gap-detection.md) | "Where are we lacking" engine (INS-*) |
| — | [modules/](modules/) | One file per module: observation, near-miss, incident, roadmap, `_template.md` |
| — | [ai/coding-rules.md](ai/coding-rules.md) | Mandatory for any code change (COD-*) |
| — | [ai/task-template.md](ai/task-template.md) | How to scope an AI task cheaply |
| — | [decisions.md](decisions.md) | ADRs + open questions |

## Core idea in one paragraph
Observation, Near-miss and Incident are not three apps. They are three **configurations of one Safety Record engine**
(report → triage → investigate → corrective actions → close → verify). Build the engine, tenant model and access model once;
each module becomes a record type + workflow + form + a few specific rules. New modules (inspections, permit-to-work,
training, …) then cost days, not months. The Insights engine reads the same records and tells each establishment where it is weak.

## Rule ID prefixes
TEN tenancy · ACC access · REC record · WFL workflow · INS insights · OBS observation · NMS near-miss · INC incident ·
EDG edge case · COD coding · NFR non-functional.

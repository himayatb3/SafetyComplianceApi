# 07 — Insights & gap detection (INS-*)

Purpose: tell each establishment **where it is lacking** without anyone building reports. This is the product differentiator; design for it from day one (data must be captured in a way these rules can read).

## Layers
1. **Metrics** — pre-aggregated per tenant/company/site/day (read models, EDG-061): counts, rates, durations.
2. **Rules** — declarative checks over metrics + records; produce **Findings**.
3. **Findings** — `Finding{Id, TenantId, Scope(site/company), RuleKey, Severity, Evidence(links/values), FirstSeen, LastSeen, Status(Open|Acknowledged|Resolved|Suppressed), Owner}`; de-duplicated by (rule, scope).
4. **Scorecards** — composite site score (0–100) from weighted rule groups; trend vs previous period and vs peer sites in the same tenant.

## Rules
- INS-001 Rules are data (key, params, thresholds, severity), defaults shipped by us, thresholds overridable per tenant. No per-tenant code.
- INS-002 Evaluation is scheduled per tenant (nightly + after relevant events) in site time zones; idempotent; tenant context mandatory.
- INS-003 A finding always shows **why** (the numbers and linked records) and a **suggested next step** (one-click create action / assign).
- INS-004 Findings respect ACC scope; users see only findings for sites in scope. Confidential records contribute to counts only for permitted viewers (no leakage via totals).
- INS-005 Low data volume must not produce false alarms: rules specify a minimum sample size; below it → "insufficient data" (itself a finding for "no reporting", see G-01).
- INS-006 Suppress with reason + expiry; suppressed findings reappear on expiry.
- INS-007 Benchmarks are only within a tenant. Cross-tenant benchmarking (anonymised, opt-in, k-anonymity ≥ 10 tenants) is a later, separate decision.

## Starter rule library (v1)
| Key | Detects | Default |
|---|---|---|
| G-01 No-reporting | Site with 0 observations/near-misses in N days (under-reporting culture) | 14 d |
| G-02 Overdue actions | Open actions past due; % and age | >10% or any critical |
| G-03 Slow triage | Median time-to-first-triage over SLA | >24 h |
| G-04 Repeat events | Same category/area/equipment ≥ N within window | 3 in 90 d |
| G-05 Open investigations | Incidents without investigation/RCA past SLA | per workflow |
| G-06 Weak CAPA | Actions of type "retraining/awareness" dominate root causes (low-effectiveness controls) | >60% |
| G-07 Extension abuse | Actions extended ≥ 2 times | 2 |
| G-08 Reopen rate | High reopen/rejected-verification rate | >10% |
| G-09 Severity drift | Near-miss potential severity high but no incident-level investigation | any |
| G-10 Ratio health | Observation:near-miss:incident ratio far from healthy pyramid | configurable |
| G-11 Coverage | Areas/shifts/contractors never reporting; users with no activity | 30 d |
| G-12 Regulatory | Regulatory clock breaches / filings overdue | any |
| G-13 Closure quality | Records closed with empty root cause / no evidence | any |
| G-14 Hot spots | Geographic/area clustering of events | top-N |

(Later modules add rules: expired training/permits, missed inspections, overdue calibrations, contractor compliance.)

## Dashboards (v1)
Site status (open by stage, overdue, SLA risk) · Leading vs lagging indicators · Hot-spot map/heat grid · Action aging · Corporate comparison table · "What needs my attention today" per user (assignments, approvals, expiring items).

## Data-capture implications (design now)
Taxonomy codes for category/root cause must be structured (not free text); severity vs **potential** severity captured; every action tagged by control type (elimination … PPE/training — hierarchy of controls); area/equipment links optional but encouraged; timestamps for every stage (so durations are derivable).

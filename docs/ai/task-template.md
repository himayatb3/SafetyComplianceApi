# AI task template (copy into the prompt; keeps context small)

```
TASK: <one vertical slice, e.g. "Create Observation use case">
READ ONLY: CLAUDE.md, docs/ai/coding-rules.md, docs/modules/observation.md, docs/05-record-and-workflow-engine.md (§Safety Record)
IMPLEMENT RULES: OBS-001, OBS-002, REC-001, REC-011, ACC-002, TEN-002
EDGE CASES TO TEST: OBS-E01, EDG-021
FILES TO TOUCH (expected): Domain/Entity/..., Application/Features/Observation/Create/..., Persistence/Configurations/..., API/Controllers/ObservationsController.cs, tests/...
OUT OF SCOPE: notifications, insights, UI
DONE WHEN: acceptance bullets from module §9 [list], tests green, docs updated if a rule changed
```

## Tips for cheap, accurate runs
- Name rule IDs instead of pasting rule text; the agent opens the file.
- One slice per session; commit; start next with a fresh context.
- If the agent finds a spec gap, it should append an entry to `docs/decisions.md` "Open questions" and stop.
- Prefer generating from the template for repetitive pieces (entity config, DTO, validator, endpoint) once the first module sets the pattern; the first module is the reference implementation — later modules say "follow `Features/Observation`".

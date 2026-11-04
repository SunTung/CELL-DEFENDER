# CELL DEFENDER — Unity Modular Framework V0.1

## Status
Source framework committed, Unity compilation and APK **not verified**.

## Layers
- Core: GameTypes, PlayerState, DamageSystem, GameSession lifecycle.
- Combat: AttackDefinition data (six attacks); runtime projectile simulation is pending.
- Enemies: EnemyDefinition data (seven behaviors); runtime AI controllers are pending.
- Progression: four independent skill tracks, experience threshold, trauma placeholder.
- Pickups: health 25%, shield +3 capped at 9, energy +10.
- Stages: six stages represented by ScriptableObject wave/boss definitions; stage runtime pending.
- UI/Art/Audio/Save: not implemented; maintain the 16-bit biomechanical art contract in ENGINEERING_EXECUTION_PLAN.md.

## Required follow-up
1. Validate actual Unity version and package compatibility; fix manifest/ProjectVersion.
2. Implement scene bootstrap + GameSession wiring; avoid mixing old prototype CellGame and new framework.
3. Implement reusable projectile pool and six attack strategies.
4. Implement seven enemy movement/AI controllers and stage spawner.
5. Implement touchscreen controller, skill selection, and HUD.
6. Build Android APK in Actions, collect build log and device verification.
7. Add art assets, collision profiles, and animation after mechanical slice passes.

## Known gaps
- Existing CellGame.cs remains a standalone unverified prototype and is not yet migrated.
- Skill choices are not yet randomized or gated by acquisition ledger.
- Trauma only reduces experience level; skill invalidation is pending.
- Progression handles one level threshold per pickup; multi-threshold award requires a loop.
- ScriptableObject asset definitions are schemas; actual game data assets not yet authored.
- No claim of a working APK or verified Unity build.

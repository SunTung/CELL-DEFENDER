# CELL DEFENDER Android Stage 14 — build staging

Base: Stage 13 validated local archive (2026-11-05).

## Current state
- Local Android project and game assets: available in Stage 13 ZIP, **not yet uploaded to this GitHub branch**.
- Local tests: six Node.js suites passed; Android preflight 15 checks passed; 17 bundled assets matched.
- GitHub Android build: NOT RUN. APK: NOT BUILT.
- Device acceptance: deferred until all desktop-side work completes.

## Required paths before running Android CI
`android/`, `game/`, `scripts/`, `.github/workflows/android-debug-apk.yml`.

Do not label this branch as an APK release until Actions has produced a verified artifact.

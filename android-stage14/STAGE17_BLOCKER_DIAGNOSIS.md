# Stage 17 — GitHub upload problem diagnosis (2026-11-05)

The blocker is **not GitHub's ability to compile Android APKs**. GitHub Actions can build a Gradle Android project, but the current branch still lacks the full web assets and test suites.

The authoritative source is the Stage 15 local project, not the separately reconstructed JS files uploaded in earlier partial commits. Avoid treating partial GitHub files as verified copies.

Local Stage 17 uploader:
- runs Android preflight, source-vs-bundle asset hashes, and all JS tests
- clones the isolated branch and overlays source files without deleting unrelated repository paths
- commits and pushes only after all tests pass
- supports repeat runs with no-op behavior

The uploader was validated against a local bare Git repository with existing files and binary PNG/WAV assets. It passed the initial push and the idempotent rerun. This **does not prove remote GitHub upload**.

To upload remotely, run `STAGE17_GITHUB_UPLOAD.sh` in a networked environment with GitHub credentials, or transfer all Stage 15 files using an authorized binary-capable connector. Do not run CI until complete.

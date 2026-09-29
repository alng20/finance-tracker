# Finance Tracker — GitHub Copilot Instructions

## Project

Finance Tracker is a personal/family finance tracking application.

The repository contains:

- ASP.NET Core / .NET 10 backend
- React 19 + TypeScript frontend
- PostgreSQL
- Docker and Docker Compose
- GitHub Actions CI/CD

## Repository structure

- `src/**` — backend source code
- `frontend/**` — frontend source code
- `tests/**` — backend and shared test projects
- `docs/**` — project documentation
- `docker/**` — local infrastructure
- `deploy/**` — deployment configuration
- `.github/**` — CI/CD and Copilot configuration

Keep application code, tests, documentation, and infrastructure in their designated directories.

## General development rules

- Inspect the closest analogous implementation before changing code.
- Prefer existing project abstractions, helpers, and dependencies.
- Keep changes focused on the requested behavior.
- Do not introduce unrelated refactoring.
- Add or update tests for behavior changes.
- Update documentation when public behavior or setup changes.
- Do not commit secrets, credentials, tokens, or production configuration.
- Preserve unrelated working-tree changes.

## Cross-layer contracts

When changing a backend API contract:

- inspect all frontend consumers;
- update backend and frontend types consistently;
- update related tests;
- preserve backward compatibility unless a breaking change is explicitly requested;
- update OpenAPI or generated artifacts when applicable.

## Verification

- Use the CI workflow as the source of truth for required checks.
- Run the smallest relevant checks first.
- Do not run unrelated expensive checks.
- Do not claim that a check passed unless it was actually run.
- Report passed, failed, skipped, and unavailable checks explicitly.

## Security baseline

- Do not weaken security settings to make tests pass.
- Do not expose secrets or sensitive authentication data in logs, tests, source code, or API responses.
- Use the existing project configuration and authentication mechanisms.
- Treat authentication, authorization, credentials, cookies, CORS, and user data as security-sensitive.

## Git branches

When branch creation is explicitly part of the task:

- `ft/add-<short-description>` — feature
- `ft/fix-<short-description>` — bug fix
- `ft/refactor-<short-description>` — refactoring
- `ft/add-test-<short-description>` — test-only change

Use lowercase kebab-case. Before creating or switching a branch, check the current branch and repository status. Do not switch branches when the platform already provides the target branch.

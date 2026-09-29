---
name: backend-feature
description: Use when implementing or modifying a Finance Tracker ASP.NET Core backend feature, including commands, queries, handlers, validators, DTOs, repositories, providers, controllers, or related application behavior.
---

# Backend Feature Development

Follow this workflow when implementing a backend feature.

## 1. Understand

Identify:

- requested behavior;
- affected endpoint/use case;
- input and output contracts;
- authentication/authorization requirements;
- persistence requirements;
- validation rules.

Do not implement immediately.

## 2. Inspect analogous code

Find the closest existing feature with similar behavior.

Inspect the relevant:

- Controller;
- Command/Query;
- Handler;
- Validator;
- DTO;
- Application interface;
- Infrastructure implementation;
- tests.

Prefer copying the project's established structure over introducing a new pattern.

## 3. Identify architectural impact

Determine which layers actually need changes:

- Domain;
- Application;
- Infrastructure;
- API;
- tests.

Do not modify a layer unless the feature requires it.

Respect the dependency direction.

Application must not depend on Infrastructure.

## 4. Reuse existing abstractions

Search before creating:

- repository interfaces;
- providers;
- current-user services;
- request services;
- Unit of Work behavior;
- validation patterns;
- DTO patterns.

If an existing abstraction is close but missing one operation, consider extending it before creating another abstraction.

## 5. Implement

Follow the existing project conventions.

Typical flow:

`Controller -> MediatR -> Handler -> Application abstraction -> Infrastructure`

Keep controllers thin.

Keep business logic in Application/domain code.

Use `CancellationToken`.

Use FluentValidation for request validation.

Return DTOs rather than Domain entities.

## 6. Add tests

Select the appropriate test level.

Application logic:
- Application unit test.

Persistence:
- Infrastructure integration test with PostgreSQL/Testcontainers.

HTTP/API behavior:
- API integration test with `FinanceTrackerApiFactory`.

Reuse `TestDataFactory`.

Cover the normal path and important failure/edge cases.

## 7. Verify

Run targeted tests first.

Then run the relevant broader tests.

Run formatting/build checks required by the project.

Review:

- architecture boundaries;
- authorization;
- validation;
- cancellation;
- error handling;
- database behavior;
- unrelated changes.

If the endpoint contract changes, inspect and update frontend consumers, shared types, OpenAPI artifacts, and related tests.

Do not finish with known failing tests unless the failure is explicitly explained and outside the scope of the change.

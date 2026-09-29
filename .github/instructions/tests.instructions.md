---
applyTo: "frontend/**/*.test.ts,frontend/**/*.test.tsx,frontend/**/*.spec.ts,frontend/**/*.spec.tsx,tests/**/*.cs"
---

# Finance Tracker Testing Instructions

## General

Choose the test level based on what is being tested.

Before writing a new test, inspect the closest analogous existing test.

Prefer testing behavior and contracts rather than implementation details.

Keep tests focused on one behavior.

Use descriptive test names that explain the expected behavior.

## Backend test levels

### Application unit tests

Use Application unit tests for:

- handlers;
- application logic;
- validation-related behavior;
- orchestration between Application abstractions.

Mock external dependencies with the existing mocking approach.

The project uses Moq and FluentAssertions.

Do not start PostgreSQL or EF Core for a test that only needs to verify Application logic.

Instantiate the handler directly when that is the existing pattern.

### Infrastructure integration tests

Use Infrastructure integration tests for:

- repositories;
- providers;
- EF Core queries;
- PostgreSQL-specific behavior;
- persistence behavior.

Use Testcontainers PostgreSQL as in the existing infrastructure tests.

Apply migrations against the test database when required.

Test the real repository/provider implementation rather than mocking EF Core.

Use `TestDataFactory` for reusable test data.

### API integration tests

Use API integration tests for:

- HTTP endpoints;
- routing;
- authentication/authorization behavior;
- request/response contracts;
- HTTP status codes;
- integration between API, Application, Infrastructure, and database.

Use the existing `FinanceTrackerApiFactory`.

Use `HttpClient` rather than calling controllers directly.

Use the existing test authentication mechanism for authenticated endpoints.

Verify database state when the behavior requires persistence verification.

## Test data

Prefer `TestDataFactory`.

If a reusable entity setup is missing, extend `TestDataFactory` rather than duplicating complex entity construction across tests.

Keep test data deterministic.

Do not use real secrets or production credentials in tests.

## Frontend tests

Use:

- Vitest
- React Testing Library
- `@testing-library/user-event`
- MSW

Mock HTTP at the network boundary with MSW.

Test what the user can observe:

- rendered content;
- loading state;
- error state;
- empty state;
- successful data;
- user interactions;
- navigation where relevant.

Avoid testing React implementation details.

## Regression tests

When fixing a bug:

1. reproduce the behavior with a failing test when practical;
2. implement the fix;
3. verify the regression test passes;
4. run the relevant broader test suite.

For asynchronous frontend code, include tests for relevant race/cancellation behavior when stale responses could overwrite current state.

## Verification

Run targeted tests first.

Then run the broader relevant test suite.

For backend changes, ensure the appropriate `dotnet build` and `dotnet test` checks pass.

For frontend changes, run from `frontend/`:

- `npm run lint`
- `npm run build`
- `npm run test:run`

- Use `npm run format:check` for verification.
- Use `npm run format` only when formatting changes are explicitly required or after implementation is complete.

pass when relevant to the change.

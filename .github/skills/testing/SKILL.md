---
name: testing
description: Use when adding, modifying, debugging, or reviewing tests in the Finance Tracker backend or frontend, including unit tests, integration tests, API tests, React tests, MSW tests, and regression tests.
---

# Testing Workflow

## 1. Determine the correct test level

Identify what the changed code is responsible for.

Use:

- Application unit tests for Application logic;
- Infrastructure integration tests for EF Core/PostgreSQL behavior;
- API integration tests for HTTP and end-to-end backend integration;
- frontend tests for observable React behavior.

Do not use a more expensive integration test when a focused unit test is sufficient.

## 2. Inspect existing tests

Find the closest analogous test.

Follow existing:

- naming;
- setup;
- fixtures;
- mocking;
- assertions;
- test data;
- authentication setup.

Do not introduce a second testing pattern when an existing one already works.

## 3. Build test data

Backend:

Use `TestDataFactory`.

Extend it when reusable test setup is missing.

Infrastructure/API tests should use the existing PostgreSQL/Testcontainers infrastructure.

Frontend:

Use MSW handlers for HTTP behavior.

Use realistic user interactions with `userEvent`.

## 4. Choose test cases

Start with the behavior that must work.

Then consider:

- invalid input;
- empty data;
- not found;
- authorization failures;
- server errors;
- boundary values;
- important state transitions;
- cancellation/concurrency where relevant.

Do not add large numbers of low-value tests merely for coverage.

## 5. Write focused tests

A test should clearly communicate:

- the initial state;
- the action;
- the expected result.

Prefer behavior assertions.

Avoid asserting internal implementation details unless they are themselves the contract being tested.

## 6. Run targeted tests

Run the tests related to the change first.

Fix failures before running the broader suite.

## 7. Run broader verification

For backend changes, run the relevant `dotnet build` and `dotnet test`.

For frontend changes, run the relevant:

- format check;
- lint;
- build;
- Vitest suite.

## 8. Review

Before finishing, check that tests:

- are deterministic;
- do not depend on production services;
- do not contain real secrets;
- do not duplicate existing setup unnecessarily;
- actually fail when the intended behavior is broken.

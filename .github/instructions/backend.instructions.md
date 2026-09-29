---
applyTo: "src/**/*.cs"
---
# Finance Tracker Backend Instructions

## Architecture

- Follow the existing Clean Architecture and Vertical Slice / CQRS structure.
- Respect the dependency direction:

  `API -> Application -> Domain`

  `Infrastructure -> Application/Domain`

- Application code must not reference Infrastructure types.
- Organize features by use case when the existing module follows Vertical Slice conventions.
- Inspect the nearest analogous feature before introducing a new layout.

## API layer

- Keep controllers thin.
- Controllers should bind HTTP input, send commands or queries through MediatR, and return the appropriate response.
- Do not put business logic, database queries, or complex validation in controllers.
- Follow existing routing, authorization, DTO, and error-response conventions.
- Do not expose Domain entities directly from API endpoints.

## Application layer

- Use MediatR commands and queries for application use cases.
- Handlers should coordinate application behavior, not directly depend on Infrastructure.
- Do not inject `FinanceTrackerDbContext` into Application handlers.
- Use existing abstractions under `Application.Common.Interfaces`.
- Before creating an interface, search for an existing repository, provider, current-user service, request service, or Unit of Work abstraction.
- Keep business logic in Application or Domain code rather than controllers.
- Pass `CancellationToken` through asynchronous calls.

## Validation and domain rules

- Use FluentValidation and the existing MediatR validation pipeline for request validation.
- Do not duplicate request-validation rules in controllers or handlers.
- Handlers and Domain objects must still enforce business invariants that cannot be expressed as request validation.
- Respect existing Domain methods, value objects, enums, and invariants.

## Infrastructure and persistence

- Keep EF Core and PostgreSQL concerns in Infrastructure.
- Repository and provider implementations belong in Infrastructure.
- Do not leak EF Core types into Application.
- Use `AsNoTracking()` for read-only queries where appropriate.
- Pass `CancellationToken` to asynchronous EF Core operations.
- Follow the existing Unit of Work pattern where the feature uses it.
- Do not edit an already-applied EF Core migration; create a new migration instead.
- Review generated migration SQL for destructive or data-loss operations.

## Authentication and authorization

- Use the existing authentication and authorization infrastructure.
- Do not create a parallel JWT, refresh-token, current-user, or cookie mechanism.
- Use existing current-user abstractions instead of manually extracting user information in every handler.
- Keep authorization requirements explicit according to existing project conventions.

## Backend tests

- Use Application unit tests for handlers and Application logic.
- Use Infrastructure integration tests for EF Core, repositories, providers, and PostgreSQL behavior.
- Use API integration tests for HTTP contracts, routing, authentication, authorization, and status codes.
- Use `FinanceTrackerApiFactory` for API integration tests.
- Prefer `TestDataFactory` over duplicated test setup.

## Backend change rule

- Modify only the layers required by the feature.
- Prefer the smallest coherent change.
- Do not introduce a new architectural pattern when an existing pattern solves the problem.

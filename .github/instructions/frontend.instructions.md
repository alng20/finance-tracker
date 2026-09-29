---
applyTo: "frontend/src/**/*.ts,frontend/src/**/*.tsx"
---

# Finance Tracker Frontend Instructions

## Architecture

The frontend uses a feature-based architecture.

Feature-specific code belongs under:

`frontend/src/features/<Feature>`

Follow the existing structure of the closest analogous feature.

Typical feature areas include:

- `api`
- `components`
- `hooks`
- `pages`
- `types`
- `validation`
- `mappers`

Do not introduce a new folder structure for an individual feature without a clear reason.

## API communication

API calls belong in feature API modules.

Use the existing `apiClient` and authentication/token handling.

Do not call `fetch` directly from feature components.

Before adding a new API helper, inspect the existing API modules for a reusable pattern.

Keep API response/request types explicit.

## Data fetching

Before creating a new hook, inspect existing hooks such as:

- `useGet`
- `useGetWithFilters`
- `useSearch`

Reuse existing abstractions when their behavior matches the requirement.

Do not duplicate data-fetching logic across components.

For asynchronous requests:

- handle loading state;
- handle errors;
- handle request cancellation where the existing abstraction supports it;
- avoid allowing stale requests to overwrite current state.

## Components

Components should focus on rendering and user interaction.

Keep API and complex data-fetching logic outside presentational components when an existing hook/API abstraction can own it.

Use existing shared components before creating duplicates.

Follow the existing naming and component composition conventions.

## Forms and validation

Follow the existing form and validation approach used by the feature.

Do not duplicate validation rules between unrelated components.

Use accessible labels and controls.

Prefer semantic HTML and native form behavior where appropriate.

## Testing

Use:

- Vitest
- React Testing Library
- `@testing-library/user-event`
- MSW

Tests should primarily verify observable behavior.

Prefer queries such as:

- `getByRole`
- `getByLabelText`
- `getByText`

Avoid asserting implementation details such as internal component state or private function calls.

Use `userEvent` for realistic user interactions.

Use MSW to mock API requests instead of mocking the API implementation itself.

For a feature with API behavior, cover relevant:

- successful responses;
- error responses;
- empty states;
- loading states;
- important user interactions.

## TypeScript

Keep types explicit at API and feature boundaries.

Avoid `any` unless there is a specific technical reason.

Prefer existing domain/API types over redefining equivalent types in multiple locations.

Use type-only imports where consistent with the existing project configuration.

## Changes

Follow existing feature patterns before introducing new abstractions.

Avoid moving code into `shared` unless it is genuinely reused by multiple independent features.

Avoid unrelated refactoring while implementing a feature or bug fix.

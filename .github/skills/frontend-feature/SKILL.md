---
name: frontend-feature
description: Use when implementing or modifying a Finance Tracker React/TypeScript feature, including pages, components, hooks, API modules, forms, validation, state management, or feature tests.
---

# Frontend Feature Development

Follow this workflow when implementing or modifying a frontend feature.

## 1. Understand

Identify:

- user-visible behavior;
- affected feature;
- API requirements;
- loading/error/empty states;
- form and validation requirements;
- navigation requirements;
- state ownership.

## 2. Inspect analogous code

Find the closest existing feature.

Inspect:

- page;
- components;
- hooks;
- API module;
- types;
- validation;
- tests.

Use the existing structure whenever possible.

## 3. Reuse existing abstractions

Before creating a new hook or API abstraction, inspect:

- `apiClient`;
- existing feature API modules;
- `useGet`;
- `useGetWithFilters`;
- `useSearch`;
- shared components;
- shared types/utilities.

Do not create a duplicate abstraction for behavior already provided by the project.

## 4. Plan state ownership

Determine which component or hook should own each piece of state.

Avoid unnecessary state duplication.

For asynchronous operations, consider:

- loading;
- error;
- stale responses;
- request cancellation;
- cleanup;
- effect dependencies.

Do not allow an obsolete request to overwrite state belonging to a newer request.

## 5. Implement

Follow the feature-based architecture.

Keep API communication in API modules.

Keep reusable asynchronous logic in hooks.

Keep presentational components focused on rendering and user interaction.

Use TypeScript types at boundaries.

Use accessible form controls and labels.

## 6. Test

Use:

- Vitest;
- React Testing Library;
- `userEvent`;
- MSW.

Test observable behavior.

For API-driven features, cover relevant:

- successful response;
- API error;
- loading;
- empty state;
- important user interaction.

For asynchronous/concurrent behavior, add a regression test when stale responses or cancellation can affect visible state.

## 7. Verify

Run frontend commands from the `frontend/` directory:

1. targeted tests;
2. full frontend test suite when appropriate;
3. formatting check;
4. lint;
5. TypeScript/build.

Expected project checks include:

`npm run format:check`

`npm run lint`

`npm run build`

`npm run test:run`

Use `npm run format` only when formatting changes are explicitly required.

Review the final change for:

- stale state;
- effect dependency issues;
- unnecessary re-renders;
- accessibility;
- duplicated logic;
- unrelated changes.

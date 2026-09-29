---
name: security-sensitive-change
description: Use when changing Finance Tracker authentication, authorization, JWTs, refresh tokens, cookies, CORS, forwarded headers, password handling, current-user/request context, secrets, configuration, or other security-sensitive backend/frontend behavior.
---

# Security-Sensitive Change Workflow

Treat the following as security-sensitive:

- login/register;
- authentication;
- authorization;
- JWT access tokens;
- refresh tokens;
- authentication cookies;
- password hashing;
- current-user context;
- request context;
- CORS;
- forwarded headers;
- secrets;
- configuration containing credentials;
- security-related middleware;
- privileged/admin endpoints.

## 1. Inspect the existing security model

Before changing security behavior, inspect the existing:

- authentication configuration;
- authorization policies;
- JWT configuration;
- refresh-token flow;
- cookie configuration;
- current-user services;
- request services;
- CORS configuration;
- forwarded-header configuration;
- options validation;
- related tests.

Do not invent a parallel authentication or authorization mechanism.

## 2. Identify the trust boundary

Determine:

- what comes from the browser/client;
- what is trusted only after authentication;
- what data is derived from the authenticated user;
- what data can be controlled by the request;
- which endpoints require authorization;
- which roles/claims are relevant.

Do not trust user identifiers supplied by the client when the authenticated identity already provides the authoritative user identity.

## 3. Protect secrets

Never:

- hard-code production secrets;
- commit API keys/passwords;
- log JWT secrets;
- log refresh tokens;
- expose credentials in API responses;
- put real credentials into tests.

Use the project's configuration/environment mechanisms for secrets.

When configuration changes, inspect both the application configuration and deployment configuration.

## 4. Authentication and cookies

When changing JWT or refresh-token behavior, verify:

- token validation;
- expiration;
- issuer/audience;
- refresh-token storage;
- refresh-token hashing where applicable;
- cookie `HttpOnly`;
- cookie `Secure`;
- cookie `SameSite`;
- cookie `Path`;
- logout/invalidation behavior.

Do not weaken cookie or token protections merely to make a local development scenario easier.

If a development-only setting is required, keep it explicitly environment-specific.

## 5. Authorization

Verify both:

- authenticated vs unauthenticated access;
- role/permission restrictions where applicable.

Add or update API integration tests for authorization-sensitive endpoints.

Test at least the relevant:

- allowed case;
- unauthenticated case;
- forbidden case.

## 6. Browser/network security

When changing CORS or forwarded headers, verify the actual deployment topology.

Do not use permissive configuration such as allowing arbitrary origins or trusting arbitrary proxy headers without a documented reason.

Keep trusted proxy/network configuration explicit.

## 7. Validation and data exposure

Validate externally supplied input.

Do not return sensitive internal information through validation errors or exception responses.

Do not expose Domain entities or internal persistence details through API responses.

## 8. Tests

Update security-focused tests for the changed behavior.

Prefer API integration tests for authentication/authorization behavior.

Use the existing test authentication infrastructure rather than bypassing authentication in production code.

## 9. Verify

Run targeted security-related tests first.

Then run the broader relevant test suite and build.

Before finishing, review the diff specifically for:

- secret exposure;
- authentication bypass;
- authorization bypass;
- insecure cookie settings;
- overly permissive CORS;
- unsafe proxy trust;
- sensitive logging;
- accidental changes to unrelated security behavior.

---
title: "LoginService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/account/login/login.service.ts"
updated: 2026-06-12
---

# LoginService

Orchestrates credential and social-provider authentication, token storage, two-factor branching and post-login redirect for the entire account area.

## Public interface

authenticate(finallyCallback?, redirectUrl?, captchaResponse?): void
externalAuthenticate(provider: ExternalLoginProvider): void
ensureExternalLoginProviderInitialized(provider, callback): void
twitterLoginCallback(token, verifier): void
wsFederationLoginStatusChangeCallback(errorDesc, token, error, tokenType): void
openIdConnectLoginCallback(resp): void
init(): void
authenticateModel: AuthenticateModel
authenticateResult: AuthenticateResultModel
externalLoginProviders: ExternalLoginProvider[]

## Used by

- [[account-module]]
- [[account-component]]
- [[login-component]]
- [[register-component]]
- [[reset-password-component]]
- [[send-two-factor-code-component]]
- [[validate-two-factor-code-component]]
- [[session-lock-screen-component]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- angular-oauth2-oidc
- adal-angular
- @azure/msal-browser
- ngx-spinner
- rxjs
- lodash-es

## Notes

Loads Facebook, Google GIS, and PayPal SDKs dynamically via ScriptLoaderService. MSAL PublicClientApplication is instantiated per-login-attempt rather than once at boot. Stores 2FA remember-client token with a one-year expiry in LocalForage.

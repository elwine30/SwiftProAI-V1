---
title: "AppAuthService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/auth/app-auth.service.ts"
updated: 2026-06-12
---

# AppAuthService

Handles user logout by calling the server TokenAuth endpoint and then clearing ABP auth tokens and local storage before optionally redirecting.

## Public interface

logout(reload?: boolean, returnUrl?: string): void
logoutInternal(reload?: boolean, returnUrl?: string): void

## Used by

- [[app-common-module]]
- [[linked-account-service]]
- [[session-timeout-modal-component]]

## External dependencies

- @angular/core
- abp-ng2-module

## Notes

Relies heavily on global abp.auth, abp.multiTenancy and abp.log namespaces. Uses XmlHttpRequestHelper directly instead of Angular HttpClient to avoid interceptor loops during logout.

---
title: "ExternalLoginInfoManagerFactory"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/External/ExternalLoginInfoManagerFactory.cs"
updated: 2026-06-12
---

# ExternalLoginInfoManagerFactory

Factory that resolves the correct IExternalLoginInfoManager implementation based on the OAuth provider name, returning WsFederation-specific handling when needed.

## Public interface

IDisposableDependencyObjectWrapper<IExternalLoginInfoManager> GetExternalLoginInfoManager(string loginProvider)

## Used by

- [[token-auth-controller]] calls this factory during ExternalAuthenticate to obtain the correct info manager

## External dependencies

- Abp.Dependency

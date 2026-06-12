---
title: "AbpOpenIddictClaimsPrincipalManager"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Claims/AbpOpenIddictClaimsPrincipalManager.cs"
updated: 2026-06-12
---

# AbpOpenIddictClaimsPrincipalManager

Singleton pipeline manager that iterates registered IAbpOpenIddictClaimsPrincipalHandler implementations inside a new DI scope to enrich or transform claims before token issuance.

## Public interface

virtual Task HandleAsync(OpenIddictRequest openIddictRequest, ClaimsPrincipal principal)

## Used by

- [[openiddict-registrar]] registered as the singleton claims pipeline during OpenIddict setup
- [[abp-open-id-dict-controller-base]] called from PreSignInCheckAsync and SetSuccessResultAsync helpers
- [[openiddict-token-controller]] invoked before signing in the principal at the token endpoint
- [[openiddict-authorize-controller]] invoked before issuing authorisation codes

## External dependencies

- OpenIddict.Abstractions
- Abp.Dependency

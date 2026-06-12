---
title: "ThinknInsurTechDomainServiceBase"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/ThinknInsurTechDomainServiceBase.cs"
updated: 2026-06-12
---

# ThinknInsurTechDomainServiceBase

Abstract base class inherited by every domain service in the project; sets the shared localisation source name on construction.

## Public interface

- Inherits `DomainService` from ABP — provides `LocalizationManager`, `CurrentUnitOfWork`, `Logger`, `AbpSession`

## Dependencies

- `ThinknInsurTechConsts.LocalizationSourceName` (compile-time constant from Core.Shared)

## External dependencies

- Abp.Zero (DomainService)

## Notes

- All concrete domain service managers (e.g. `BranchManager`, `StatusManager`) extend this class.
- The class body is intentionally sparse — add cross-cutting domain logic here rather than duplicating it in individual managers.

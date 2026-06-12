---
title: "ThinknInsurTechApplicationModule"
type: module
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/ThinknInsurTechApplicationModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechApplicationModule

ABP module class that wires up the application layer: registers the authorization provider and AutoMapper profile at startup.

## Public interface

void PreInitialize()
void Initialize()

## External dependencies

- Abp.Zero

## Notes

Depends on ThinknInsurTechApplicationSharedModule and ThinknInsurTechCoreModule. Registers AppAuthorizationProvider and CustomDtoMapper.

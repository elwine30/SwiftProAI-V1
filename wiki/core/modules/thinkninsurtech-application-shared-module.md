---
title: "ThinknInsurTechApplicationSharedModule"
type: module
language: csharp
layer: shared
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/ThinknInsurTechApplicationSharedModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechApplicationSharedModule

ABP module class that registers all shared application interfaces and DTOs via assembly convention scanning at startup.

## Public interface

void Initialize()

## External dependencies

- Abp.Core

## Notes

Declares DependsOn(ThinknInsurTechCoreSharedModule). Entry point for IoC registration of the entire Application.Shared assembly.

---
title: "EfCoreOpenIddictDbConcurrencyExceptionHandler"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/OpenIddict/EfCoreOpenIddictDbConcurrencyExceptionHandler.cs"
updated: 2026-06-12
---

# EfCoreOpenIddictDbConcurrencyExceptionHandler

Handles EF Core DbUpdateConcurrencyException raised by OpenIddict operations by resetting affected entity states to Unchanged, preventing cascading failures on subsequent saves.

## Public interface

virtual Task HandleAsync(AbpDbConcurrencyException exception)

## External dependencies

- Abp.EntityFrameworkCore
- Microsoft.EntityFrameworkCore

## Notes

Registered as a transient dependency. Implements the IOpenIddictDbConcurrencyExceptionHandler interface from the domain layer. Resets entity state to Unchanged rather than detaching, which preserves the entity in the context.

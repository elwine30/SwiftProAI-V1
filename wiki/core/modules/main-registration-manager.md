---
title: "MainRegistrationManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/MainRegistrationManager.cs"
updated: 2026-06-12
---

# MainRegistrationManager

Domain service responsible for persisting new claim registrations within an explicit unit of work.

## Public interface

- `Task<int> CreateMainRegistrationAsync(MainRegistration registration)` — inserts and returns the new record's primary key

## Dependencies

- [[main-registration]] entity being persisted
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

## Notes

- Uses `_unitOfWorkManager.WithUnitOfWorkAsync` to ensure the insert and `SaveChangesAsync` run inside a single transaction even if called outside an existing UoW scope.
- Interface `IMainRegistrationManager` is defined separately for DI registration.

---
title: "ViewThirdPartyCasesManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Organizations/ViewThirdPartyCasesManager.cs"
updated: 2026-06-12
---

# ViewThirdPartyCasesManager

Domain service managing the cross-OU case visibility grants — creating, updating and revoking access records.

## Public interface

- `Task CreateMainRegistrationOU(ViewThirdPartyCases input)`
- `Task<ViewThirdPartyCases> GetMainRegistrationOUByAssignedOUIdAsync(long assignedOUId, int registerId)`
- `Task UpdateMainRegistrationOU(long assignedOUId, int registerId, long newAssignedOUId)`
- `Task DeleteMainRegistrationOU(long? ouId, int registerId)`

## Dependencies

- [[view-third-party-cases]] entity managed
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

## Notes

- `DeleteMainRegistrationOU` deletes all matching rows (there can be more than one) within a single UoW — deletion is hard delete, not soft delete.

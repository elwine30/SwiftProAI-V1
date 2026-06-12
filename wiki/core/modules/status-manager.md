---
title: "StatusManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Case/StatusManager.cs"
updated: 2026-06-12
---

# StatusManager

Domain service providing creation and retrieval of case workflow statuses.

## Public interface

- `Task<int> CreateStatusAsync(Status casetype)`
- `Task<List<Status>> GetAllStatusAsync()` — ordered by descending ID
- `Task<Status> GetStatusbyIdAsync(int statusId)`

## Dependencies

- [[status]] entity
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

## Notes

- Parameter named `casetype` in `CreateStatusAsync` is a copy-paste naming artefact consistent with other managers in this codebase.

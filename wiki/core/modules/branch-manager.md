---
title: "BranchManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Branches/BranchManager.cs"
updated: 2026-06-12
---

# BranchManager

Domain service providing creation and retrieval operations for branch records.

## Public interface

- `Task<int> CreateBranchAsync(Branch casetype)` — note parameter is named `casetype` by convention
- `Task<List<Branch>> GetAllBranchAsync()` — all branches ordered by descending ID
- `Task<Branch> GetBranchbyIdAsync(int branchId)`

## Dependencies

- [[branch]] entity
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

## Notes

- Parameter naming (`casetype`) in `CreateBranchAsync` appears to be a copy-paste artefact from another manager — it still accepts a `Branch` object.

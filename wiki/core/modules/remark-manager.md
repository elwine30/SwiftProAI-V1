---
title: "RemarkManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Remarks/RemarkManager.cs"
updated: 2026-06-12
---

# RemarkManager

Domain service for creating and retrieving case remarks, including filtering by registration ID.

## Public interface

- `Task<int> CreateRemarkAsync(Remark remark)`
- `Task<List<Remark>> GetAllRemarkAsync()`
- `Task<Remark> GetRemarkbyIdAsync(int remarkId)`
- `Task<List<Remark>> GetAllRemarksByRegistrationId(int registrationId)` — filtered query for a specific case

## Dependencies

- [[remark]] entity
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

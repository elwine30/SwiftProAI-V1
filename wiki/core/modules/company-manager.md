---
title: "CompanyManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Companies/CompanyManager.cs"
updated: 2026-06-12
---

# CompanyManager

Domain service providing CRUD operations for insurance companies.

## Public interface

- `Task<int> CreateCompanyAsync(InsuranceCompany company)`
- `Task<List<InsuranceCompany>> GetAllCompanyAsync()` — returns all records ordered by descending ID
- `Task<InsuranceCompany> GetCompanybyIdAsync(int companyId)`

## Dependencies

- [[insurance-company]] entity
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager)
- Microsoft.EntityFrameworkCore

---
title: "UserOrganizationUnitRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/Organizations/UserOrganizationUnitRepository.cs"
updated: 2026-06-12
---

# UserOrganizationUnitRepository

Repository that resolves all user identifiers within an organisation-unit subtree by walking the OU code hierarchy, returning distinct UserIdentifiers.

## Public interface

Task<List<UserIdentifier>> GetAllUsersInOrganizationUnitHierarchical(long[] organizationUnitIds)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing EF Core UoW and DbContext access
- [[thinkn-insur-tech-db-context]] EF Core DbContext used to query UserOrganizationUnit records
- [[predicate-builder]] builds the OR chain of StartsWith predicates over OU codes for subtree traversal

## Used by

- [[user-organization-unit-repository]] (no callers registered in this index)

## External dependencies

- Abp.EntityFrameworkCore
- Castle.Core

## Notes

Uses PredicateBuilder to build an OR chain of StartsWith predicates over OU codes, enabling hierarchical subtree traversal without recursive SQL. Returns empty list immediately if no OUs are passed.

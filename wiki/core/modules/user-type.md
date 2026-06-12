---
title: "UserType"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Types/UserType.cs"
updated: 2026-06-12
---

# UserType

GraphQL ObjectGraphType that defines the full shape of user objects in the API, including nested role and organisation unit list fields, and exposes field-selector path constants used by UserQuery.

## Public interface

- `UserType()`
- `static string ChildFields.GetFieldSelector(string childField)`
- `static const string ChildFields.Items`
- `static const string ChildFields.Roles`
- `static const string ChildFields.OrganizationUnits`

## Dependencies

- [[graphql-user-dto]] DTO providing the scalar field definitions mapped into this graph type
- [[role-type]] nested graph type used for the roles list field
- [[organization-unit-type]] nested graph type used for the organisation units list field

## Used by

- [[user-query]]
- [[user-paged-result-graph-type]]

## External dependencies

- GraphQL

## Notes

Contains two inner ObjectGraphType classes (UserType.RoleType named 'UserRoleType' and UserType.OrganizationUnitType named 'UserOrganizationUnitType') to avoid name clashes with top-level RoleType and OrganizationUnitType. The static ChildFields class provides the path strings consumed by UserQuery's HasSelectionField checks.

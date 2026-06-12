---
title: "Workshop"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Workshops/Workshop.cs"
updated: 2026-06-12
---

# Workshop

Entity representing a vehicle repair workshop that can be assigned to a case.

## Public interface

- `string WorkshopNo`
- `string WorkshopName`
- `string Address`
- `double ClaimRate`
- `bool IsActive`
- `string BusinessRegistrationNo`
- `long? OrganizationUnitId` / `long? AssignOUId`
- `bool AllowToViewAssignedCases`
- `int? ViewThirdPartyCaseRequestId` / `ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk`

## Dependencies

- [[view-third-party-case-request]] optional approval request FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Both ABP `[Audited]` and custom `[Auditable]` attributes are applied — this means changes are tracked by both the ABP audit log and the custom `AuditTrail` mechanism.
- `IMayHaveTenant` (nullable `TenantId`) allows shared workshop records across tenants when `TenantId` is null.

---
title: "CaseThirdPartyVehicle"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseThirdPartyVehicle.cs"
updated: 2026-06-12
---

# CaseThirdPartyVehicle

Entity storing registration, policy and coverage details of a third-party vehicle involved in the incident.

## Public interface

- `string VehicleNo` (required)
- `string RegisteredOwner` / `string VehicleMake` / `int? VehicleYear`
- `string PolicyNo` / `string TypeCover` / `DateTime? CoverStartDate` / `DateTime? CoverEndDate`
- `Guid? DriverCarGrant` — BinaryObject reference for the car grant document
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? CompanyId` / `InsuranceCompany CompanyFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[insurance-company]] optional insurer of the third-party vehicle

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

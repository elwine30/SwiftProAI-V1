---
title: "CaseInsuredPerson"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseInsuredPerson.cs"
updated: 2026-06-12
---

# CaseInsuredPerson

Entity recording the full personal, vehicle and document details of an insured person (owner, driver or third party) on a claim.

## Public interface

- `bool IsOwner` / `bool IsDriver` / `bool IsThirdParty`
- `string Relationship` / `string Name` / `string IdenticationType` / `string IdenticationNo`
- `string Make` / `string Model` / `string Specs` / `short? Year` / `double? Valuation`
- `string PolicyNo` / `string Coverage`
- `string LicenseNo` / `string LicenseClasses` / `DateTime? LicenseDateFrom` / `DateTime? LicenseDateTo`
- `Guid? DriverICFront` / `Guid? DriverICBack` / `Guid? DriverLicenseFront` / `Guid? DriverLicenseBack` (BinaryObject IDs)
- `int? RegisterId` / `MainRegistration RegisterFk`
- `int? HospitalId` / `Hospital HospitalFk`
- `int? CountryLocationId` / `int? StateLocationId` / `Location` FKs

## Dependencies

- [[main-registration]] parent registration FK
- [[hospital]] optional hospital FK
- [[location]] country and state location FKs

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- File attachments (IC, licence, car grant, employment, hospital detail) are stored as `Guid` references pointing to `BinaryObject` records — not direct file paths.
- `JpjRegisterNo` / `JpjRegisterDate` store Malaysian JPJ (road transport) registration details.

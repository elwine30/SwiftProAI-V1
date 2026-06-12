---
title: "CaseThirdPartyInfo"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseThirdPartyInfo.cs"
updated: 2026-06-12
---

# CaseThirdPartyInfo

Entity capturing the personal, medical and employment details of a third-party claimant involved in the case.

## Public interface

- `int? Age` / `string Sex` / `string MaritalStatus` / `string ThirdPartyType`
- `DateTime? AdmittedDate1..3` / `DateTime? DischargeDate1..3` — up to three hospital admissions
- `string InjuriesSustained` / `string MedicalLeave` / `string PresentCondition` / `string CurrentDisabilities`
- `string EmployerPrior` / `DateTime? EmployedDateFrom` / `DateTime? EmployedDateTo` / `double? IncomeLoss`
- `string SolicitorName` / `string SolicitorAddress` / `string SolicitorContact` / `string SolicitorReferenceNo`
- `bool FatalCaseCheck`
- `int? RegisterId` / `MainRegistration RegisterFk`
- `int? HospitalId1..3` / `Hospital` FKs (three possible hospitals)
- `int? CaseInsuredPersonId` / `CaseInsuredPerson CaseInsuredPersonFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[hospital]] up to three hospital FKs
- [[case-insured-person]] optional link to insured person

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `FatalCaseCheck` marks fatal accident scenarios for elevated handling.
- Three sets of admission/discharge dates reflect the pattern of third-party claimants being admitted to multiple hospitals.

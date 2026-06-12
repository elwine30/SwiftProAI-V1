---
title: "CasePoliceReport"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CasePoliceReport.cs"
updated: 2026-06-12
---

# CasePoliceReport

Entity recording police report details filed in relation to a claim incident.

## Public interface

- `string IPD` / `string PoliceStation` / `string ReportNo`
- `DateTime ReportTime` / `DateTime IncidentTime`
- `string LateReport` / `string LateReason`
- `string OfficerName` / `string ServiceNo` / `string OfficerContact`
- `string Type` / `string PoliceFinding` / `string PoliceOutcome`
- `string Statement` / `string ComplainantIdentityNo`
- `bool IsDataConsistent`
- `Guid? ReportFileUpload` — BinaryObject reference for uploaded report PDF
- `int RegisterId` / `MainRegistration RegisterFk`

## Dependencies

- [[main-registration]] parent registration FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `IsDataConsistent` flag indicates whether the statement data was verified against the original report.
- `ReportType` is a free-text discriminator separate from `Type`.

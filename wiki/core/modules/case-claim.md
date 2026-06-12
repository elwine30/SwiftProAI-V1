---
title: "CaseClaim"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseClaim.cs"
updated: 2026-06-12
---

# CaseClaim

Entity recording the financial claim settlement amounts, fees and approval state for a registered case.

## Public interface

- `decimal Total` / `decimal SD` (statutory declaration) / `decimal SearchFee`
- `decimal FileCharges` / `string FileChargesRemark`
- `decimal Hotel` / `decimal Police` / `decimal AirFare` / `decimal CharteredTransport` / `decimal Toll`
- `decimal MileageKM` / `decimal MileageUnitPrice` / `decimal MileageTotal`
- `bool Fraud` / `decimal FraudAmount`
- `bool Approved` / `bool Rejected`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? StatusId` / `Lookup StatusFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[lookup]] status FK (uses generic lookup table)

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `Fraud` flag and `FraudAmount` support fraud detection recording at the claim level.
- `StatusId` references the `Lookup` table rather than `Status` — status here is a configurable lookup value, not a workflow status.

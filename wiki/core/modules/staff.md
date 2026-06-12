---
title: "Staff"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/Staff.cs"
updated: 2026-06-12
---

# Staff

Entity extending a `User` account with insurance-adjuster-specific profile data such as NRIC, fee percentages and group membership.

## Public interface

- `string NRIC` / `string Passport` / `string Address`
- `decimal? ServiceFeePercent` / `decimal? FraudFeePercent`
- `long UserId` / `User UserFk`
- `int? GroupId` / `Group GroupFk`

## Dependencies

- [[user]] linked ABP user account
- [[group]] optional staff group membership

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `ServiceFeePercent` and `FraudFeePercent` are validated by range constants in `StaffConsts`.
- Acts as the bridge between the ABP user identity system and the insurance domain's adjuster model.

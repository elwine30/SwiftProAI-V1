---
title: "CaseClaimsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseClaimsAppService.cs"
updated: 2026-06-12
---

# CaseClaimsAppService

Manages adjuster expense claims per case including mileage, tolls, airfare, hotel, police fees and fraud amounts, auto-aggregating search fees and computing totals on save.

## Public interface

Task<GetCaseClaimForEditOutput> GetCaseClaimForEdit(EntityDto input)
Task<PagedResultDto<CreateOrEditCaseClaimDto>> GetAll(CaseClaimMainRegistrationInput input)
Task CreateOrEdit(CreateOrEditCaseClaimDto input)
Task<bool> GetCaseClaimsByRegisterId(int registerId)
Task<decimal> GetClaimRateByRegisterId(int registerId)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero
- AutoMapper

## Notes

Claim total is calculated server-side by summing all expense fields. Mileage unit price is fetched from the insurance company's ClaimRate at time of edit. Requires Pages_CaseClaims permission.

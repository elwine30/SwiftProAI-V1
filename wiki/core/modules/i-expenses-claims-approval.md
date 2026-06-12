---
title: "IExpensesClaimsApproval"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IExpensesClaimsApproval.cs"
updated: 2026-06-12
---

# IExpensesClaimsApproval

Contract for the approval workflow that allows supervisors to review, approve or reject adjuster expenses and claims in bulk.

## Public interface

Task<PagedResultDto<GetExpensesApprovalForViewDTO>> GetAllExpensesApproval(GetExpensesClaimsApprovalInput input)
Task<PagedResultDto<GetClaimsApprovalForViewDTO>> GetAllClaimsForApproval(GetExpensesClaimsApprovalInput input)
Task UpdateExpenses(List<ExpensesClaimsApprovalDto> dtoList)
Task UpdateClaims(List<ExpensesClaimsApprovalDto> dtoList)

## Notes

Does not inherit IApplicationService — may be an internal domain interface rather than an ABP-exposed app service.

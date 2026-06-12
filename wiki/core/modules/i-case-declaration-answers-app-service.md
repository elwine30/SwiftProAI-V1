---
title: "ICaseDeclarationAnswersAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseDeclarationAnswersAppService.cs"
updated: 2026-06-12
---

# ICaseDeclarationAnswersAppService

Contract for saving and retrieving the answers a claimant provides to declaration questions on a specific case.

## Public interface

Task<GetCaseDeclarationAnswerForEditOutput> GetCaseDeclarationAnswerForEdit(EntityDto input)
Task CreateOrEdit(List<CreateOrEditCaseDeclarationAnswerDto> input)

## External dependencies

- Abp.Core

## Notes

CreateOrEdit accepts a list — answers are submitted as a batch for a single case.

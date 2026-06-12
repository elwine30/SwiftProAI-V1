---
title: "IDeclarationQuestionsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IDeclarationQuestionsAppService.cs"
updated: 2026-06-12
---

# IDeclarationQuestionsAppService

CRUD contract for configurable declaration question templates displayed to users during case submission.

## Public interface

Task<PagedResultDto<GetDeclarationQuestionForViewDto>> GetAll(GetAllDeclarationQuestionsInput input)
Task<GetDeclarationQuestionForViewDto> GetDeclarationQuestionForView(int id)
Task<GetDeclarationQuestionForEditOutput> GetDeclarationQuestionForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditDeclarationQuestionDto input)
Task Delete(EntityDto input)

## External dependencies

- Abp.Core

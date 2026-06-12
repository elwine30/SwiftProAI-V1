---
title: "IEditionAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Editions/IEditionAppService.cs"
updated: 2026-06-12
---

# IEditionAppService

Contract for managing SaaS edition definitions, including feature sets, tenant migration between editions and edition combo box data.

## Public interface

Task<ListResultDto<EditionListDto>> GetEditions()
Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input)
Task CreateEdition(CreateEditionDto input)
Task UpdateEdition(UpdateEditionDto input)
Task DeleteEdition(EntityDto input)
Task MoveTenantsToAnotherEdition(MoveTenantsToAnotherEditionDto input)
Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId, bool addAllItem, bool onlyFree)
Task<int> GetTenantCount(int editionId)

## External dependencies

- Abp.Core

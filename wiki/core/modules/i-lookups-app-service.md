---
title: "ILookupsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Common/ILookupsAppService.cs"
updated: 2026-06-12
---

# ILookupsAppService

CRUD contract for key-value lookup entries used to populate categorised dropdown lists across the application.

## Public interface

Task<PagedResultDto<GetLookupForViewDto>> GetAll(GetAllLookupsInput input)
Task<GetLookupForViewDto> GetLookupForView(int id)
Task CreateOrEdit(CreateOrEditLookupDto input)
Task Delete(EntityDto input)
List<GetLookupByGroupDto> GetByGroup(string group)

## External dependencies

- Abp.Core

## Notes

GetByGroup is synchronous — callers must not await it. Intended for in-memory filtered lookups.

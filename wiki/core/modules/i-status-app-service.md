---
title: "IStatusAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Case/IStatusAppService.cs"
updated: 2026-06-12
---

# IStatusAppService

Minimal contract for retrieving and creating case status reference values used in status transitions.

## Public interface

Task<StatusDto> GetStatusDetailsbyId(int id)
Task<ListResultDto<StatusDto>> GetAllStatusDetails()
Task<int> CreateStatus(StatusDto input)

## External dependencies

- Abp.Core

## Notes

No update or delete — statuses appear to be append-only reference data.

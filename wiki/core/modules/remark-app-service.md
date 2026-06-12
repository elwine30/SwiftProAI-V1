---
title: "RemarkAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Remarks/RemarkAppService.cs"
updated: 2026-06-12
---

# RemarkAppService

Manages case remarks (comments/notes), supporting creation with automatic timestamp and creator tracking, and paginated retrieval per registration.

## Public interface

Task<RemarkDto> GetRemarkDetailsbyId(int id)
Task<ListResultDto<RemarkDto>> GetAllRemarkDetails()
Task<int> CreateRemark(RemarkDto input)
Task<PagedResultDto<RemarkDto>> GetAllRemarkByRegistrationId(RemarkInputDto remarkInput)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

GetAllRemarkByRegistrationId enforces a hard page size of 5 regardless of input. Returns results ordered by CreationTime descending. Joins to User to include CreatorUserName.

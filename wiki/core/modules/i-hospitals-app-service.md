---
title: "IHospitalsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IHospitalsAppService.cs"
updated: 2026-06-12
---

# IHospitalsAppService

CRUD contract for hospital reference records used when linking medical treatment to insured persons in a claim.

## Public interface

Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input)
Task<GetHospitalForViewDto> GetHospitalForView(int id)
Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditHospitalDto input)
Task Delete(EntityDto input)

## External dependencies

- Abp.Core

## Notes

File contains duplicate `using System.Collections.Generic;` — likely a code generation artefact.

---
title: "ICommonDropdownAppService"
type: interface
language: csharp
layer: shared
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Common/ICommonDropdownAppService.cs"
updated: 2026-06-12
---

# ICommonDropdownAppService

Aggregated contract providing dropdown data for branches, companies, adjusters, locations, hospitals and vehicles used across multiple forms.

## Public interface

Task<List<CommonDropdownDto>> GetAllBranchForTableDropdown()
Task<List<CommonDropdownDto>> GetAllCompanyForTableDropdown()
Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterForTableDropdown()
Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterByBranchForTableDropdown(int branchId)
Task<List<CommonDropdownDto>> GetAllLocationByCountryForTableDropdown(int parentLocationId)
Task<List<CommonDropdownDto>> GetAllHospitalForTableDropdown()
Task<List<CommonDropdownDto>> GetAllMakerVehicle()
Task<List<CommonDropdownDto>> GetAllModelByMakerVehicle(string maker)
Task<List<CommonDropdownDto>> GetAllSpecsByModelAndMakerVehicle(string maker, string model)

## Used by

- [[common-dropdown-app-service]]

## External dependencies

- Abp.Core

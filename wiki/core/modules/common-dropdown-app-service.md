---
title: "CommonDropdownAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Common/CommonDropdownAppService.cs"
updated: 2026-06-12
---

# CommonDropdownAppService

Centralised service providing dropdown data for branches, companies, adjusters, locations, hospitals, groups, law firms, case types, vehicles, organisation units and lookup values across the application.

## Public interface

Task<List<CommonDropdownDto>> GetAllBranchForTableDropdown()
Task<List<CommonDropdownDto>> GetAllCompanyForTableDropdown()
Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterForTableDropdown()
Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterByBranchForTableDropdown(int branchId)
Task<List<CommonDropdownDto>> GetAllLocationByCountryForTableDropdown(int parentLocationId)
Task<List<CommonDropdownDto>> GetAllHospitalForTableDropdown()
Task<List<CommonDropdownDto>> GetAllLawFirmForTableDropdown()
Task<List<CommonDropdownDto>> GetAllMakerVehicle()
Task<List<CommonDropdownDto>> GetAllModelByMakerVehicle(string maker)
Task<List<CommonDropdownDto>> GetAllSpecsByModelAndMakerVehicle(string maker, string model)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

GetAllAdjusterForTableDropdown applies the same role-based OU visibility rules as MainRegistrationAppService: superadmin sees all; adjuster company sees own OU; third party uses ViewThirdPartyCases to find visible adjusters. Location queries disable MayHaveTenant to support shared location reference data.

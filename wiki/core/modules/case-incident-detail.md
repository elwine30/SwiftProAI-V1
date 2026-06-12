---
title: "CaseIncidentDetail"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseIncidentDetail.cs"
updated: 2026-06-12
---

# CaseIncidentDetail

Entity storing the full scene-of-incident details for a claim — location, road conditions, weather, circumstances and a supporting file upload.

## Public interface

- `DateTime TimeFrom` / `DateTime TimeTo`
- `string Country` / `string State` / `string Postcode` / `string City` / `string Address1` / `string Address2`
- `string DirectionFrom` / `string DirectionTo` / `string DirectionVia`
- `string TypeOfRoad` / `double? WidthOfRoad` / `string RoadCondition`
- `string WeatherCondition` / `string Visibility` / `string SurroundingArea`
- `double? SpeedPrior` / `string Circumstances`
- `int? PassengerNo` / `string DriverDrivingWith`
- `Guid? CircumstancesFileUpload` — BinaryObject reference
- `int RegisterId` / `MainRegistration RegisterFk`

## Dependencies

- [[main-registration]] parent registration FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Highly detailed road-scene entity capturing adjuster field observations — used as source data for the AI-assisted incident report generation.

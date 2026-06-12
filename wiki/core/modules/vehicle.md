---
title: "Vehicle"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Vehicles/Vehicle.cs"
updated: 2026-06-12
---

# Vehicle

Reference entity for vehicle make, model and specification used as a lookup within the platform.

## Public interface

- `string Make`
- `string Model`
- `string Specification`

## External dependencies

- Abp.Zero (FullAuditedEntity)

## Notes

- Decorated with `[Auditable]` and `[AuditedTrail]` on all properties.
- This is a reference/master-data table — individual case vehicles are stored directly on `CaseInsuredPerson` and `CaseThirdPartyVehicle`.

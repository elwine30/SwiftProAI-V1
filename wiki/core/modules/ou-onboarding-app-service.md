---
title: "OUOnboardingAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Organizations/OUOnboardingAppService.cs"
updated: 2026-06-12
---

# OUOnboardingAppService

Orchestrates the complete onboarding of a new organisation unit: creates the OU, assigns the correct role, persists document settings, creates the admin user and seeds ten default declaration questions in a single transaction.

## Public interface

Task CreateOnboardingOu(CreateOUOnboardingInput input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Wraps the entire multi-step creation in a UnitOfWork with rollback on exception. Validates uniqueness of DisplayName, businessRegistrationNo and caseRefNoPrefix before saving. Seeds 10 hard-coded declaration questions on each OU creation. Role mapping: Insurance->Insurer, Law Firm->Lawyer, Workshop->Workshop, Adjuster->Adjuster.

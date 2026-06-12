---
title: "ThinknInsurTechEntityFrameworkCoreModule"
type: module
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/ThinknInsurTechEntityFrameworkCoreModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechEntityFrameworkCoreModule

ABP module that registers the DbContext with the IoC container, optionally decrypts the connection string, registers the organisation-unit UoW filter, and triggers database seed on PostInitialize.

## Public interface

bool SkipDbContextRegistration { get; set; }
bool SkipDbSeed { get; set; }
override void PreInitialize()
override void Initialize()
override void PostInitialize()

## Dependencies

- [[thinkn-insur-tech-db-context]] registers and configures the DbContext
- [[thinkn-insur-tech-db-context-configurer]] applies SQL Server provider options
- [[database-check-helper]] verifies database connectivity before seeding

## Used by

- [[thinkn-insur-tech-db-context]]
- [[thinkn-insur-tech-db-context-factory]]
- [[abp-zero-db-migrator]]
- [[database-check-helper]]

## External dependencies

- Abp.Zero.EntityFrameworkCore
- Abp.Modules

## Notes

ABP EntityHistory is explicitly disabled (IsEnabled = false). A hard-coded pass-phrase ('9c6103b656914cb48e0eb40fe779e4e6') is used with SimpleStringCipher for optional connection-string decryption — this should be rotated per environment. SkipDbContextRegistration and SkipDbSeed exist for test scenarios.

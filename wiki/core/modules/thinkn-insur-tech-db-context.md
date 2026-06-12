---
title: "ThinknInsurTechDbContext"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/ThinknInsurTechDbContext.cs"
updated: 2026-06-12
---

# ThinknInsurTechDbContext

The central EF Core DbContext for the application, exposing all entity DbSets and implementing custom organisation-unit query filters, OpenIddict integration, and an inline audit trail that captures field-level changes on entities marked with [Auditable].

## Public interface

DbSet<MainRegistration> MainRegistrations
DbSet<CasePoliceReport> CasePoliceReports
DbSet<AuditTrail> AuditTrails
DbSet<AuditEntry> AuditEntries
DbSet<OpenIddictApplication> Applications
DbSet<SubscriptionPayment> SubscriptionPayments
DbSet<Branch> Branches
DbSet<OrganizationUnit> OrganisationUnits
override Task<int> SaveChangesAsync(bool, CancellationToken)
override Expression<Func<TEntity,bool>> CreateFilterExpression<TEntity>()

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository binding
- [[i-open-iddict-db-context]] OpenIddict entity set contract
- [[thinkn-insur-tech-data-filters]] organisation-unit UoW filter

## Used by

- [[thinkn-insur-tech-repository-base]]
- [[thinkn-insur-tech-entity-framework-core-module]]
- [[thinkn-insur-tech-db-context-configurer]]
- [[thinkn-insur-tech-db-context-factory]]
- [[abp-zero-db-migrator]]
- [[database-check-helper]]
- [[user-repository]]
- [[subscription-payment-repository]]
- [[user-organization-unit-repository]]
- [[ef-core-open-iddict-application-repository]]
- [[ef-core-open-iddict-authorization-repository]]
- [[ef-core-open-iddict-scope-repository]]
- [[ef-core-open-iddict-token-repository]]

## External dependencies

- Abp.Zero.EntityFrameworkCore
- Microsoft.EntityFrameworkCore
- PayPalCheckoutSdk

## Notes

SaveChangesAsync is overridden to record audit trails; it calls base.SaveChangesAsync twice when auditable entities change — once to persist domain changes and again to persist the AuditTrail records. Duplicate HasIndex calls exist for several entities (e.g. CaseLawyer, CaseWorkshop, CaseInsuredPerson, CaseInsurer) which is a minor code smell. Max-length conversion normalises a PostgreSQL max-length (67108864) to SQL Server equivalent (10485760) — a remnant of the Postgres-to-SQL-Server migration. The catch block in SaveChangesAsync swallows nothing and just re-throws, serving no purpose.

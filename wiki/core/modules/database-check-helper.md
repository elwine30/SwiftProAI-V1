---
title: "DatabaseCheckHelper"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/DatabaseCheckHelper.cs"
updated: 2026-06-12
---

# DatabaseCheckHelper

Transient helper that tests whether the configured database is reachable before seeding, by opening a host-context connection inside a unit of work.

## Public interface

bool Exist(string connectionString)

## Dependencies

- [[thinkn-insur-tech-db-context]] used to resolve a connection for the connectivity check

## Used by

- [[thinkn-insur-tech-entity-framework-core-module]]

## External dependencies

- Abp.EntityFrameworkCore

## Notes

Returns true when connectionString is null or empty (unit-test shortcut). Swallows all exceptions and returns false if the connection cannot be opened.

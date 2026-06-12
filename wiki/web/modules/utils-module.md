---
title: "UtilsModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/utils.module.ts"
updated: 2026-06-12
---

# UtilsModule

Barrel NgModule that declares and exports all shared directives, pipes and utility components used across the application.

## Dependencies

- [[auto-focus-directive]] auto-focuses form inputs on render
- [[busy-if-directive]] conditionally shows a busy overlay
- [[button-busy-directive]] disables and shows spinner on buttons during async operations
- [[file-download-service]] triggers browser downloads for backend-generated files
- [[friend-profile-picture-component]] renders a user's profile picture with fallback
- [[local-storage-service]] async localForage wrapper for persistent client-side storage
- [[luxon-format-pipe]] formats Luxon DateTime objects using a named format
- [[luxon-from-now-pipe]] renders relative time strings from Luxon DateTimes
- [[validation-messages-component]] displays Angular form validation error messages
- [[equal-validator]] cross-field equality validator (e.g. confirm password)
- [[password-complexity-validator]] validates password against ABP complexity rules
- [[null-default-value-directive]] substitutes null with a default display value
- [[script-loader-service]] dynamically injects JS files into the document head
- [[style-loader-service]] dynamically injects CSS link elements into the document head
- [[array-to-tree-converter-service]] converts flat arrays to hierarchical tree structures
- [[tree-data-helper-service]] utilities for traversing and manipulating tree data
- [[localize-pipe]] pipe wrapper around ABP l() localisation
- [[number-to-words-pipe]] converts numeric values to their word representation
- [[permission-pipe]] filters content by a single ABP permission
- [[permission-any-pipe]] filters content if any of a set of permissions is granted
- [[permission-all-pipe]] filters content only if all listed permissions are granted
- [[feature-checker-pipe]] filters content by ABP feature flag
- [[date-picker-luxon-modifier-directive]] patches Bootstrap datepicker to work with Luxon
- [[date-range-picker-luxon-modifier-directive]] patches Bootstrap date range picker to work with Luxon
- [[guid-generator-service]] generates RFC 4122 UUIDs client-side

## External dependencies

- `@angular/core`
- `@angular/common`

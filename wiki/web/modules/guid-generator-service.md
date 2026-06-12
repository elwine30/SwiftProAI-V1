---
title: "GuidGeneratorService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/guid-generator.service.ts"
updated: 2026-06-12
---

# GuidGeneratorService

Generates RFC 4122-style GUID strings for use as unique identifiers in client-side logic.

## Public interface

- `guid(): string`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Uses Math.random() — not cryptographically secure. Suitable only for client-side uniqueness requirements such as spinner names.

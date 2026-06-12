---
title: "FormattedStringValueExtracter"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/FormattedStringValueExtracter.ts"
updated: 2026-06-12
---

# FormattedStringValueExtracter

Parses a format-string template (e.g. 'http://{TENANCY_NAME}.app.com') and extracts the dynamic-segment values from a matching real URL.

## Public interface

- `Extract(str: string, format: string): ExtractionResult`
- `IsMatch(str: string, format: string): string[]`

## Used by

- [[subdomain-tenancy-name-finder]]

## Notes

Self-contained tokeniser with no external dependencies. Throws descriptive errors for malformed format strings. Used exclusively by SubdomainTenancyNameFinder.

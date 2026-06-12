---
title: "InputTypeConfigurationService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/input-types/input-type-configuration.service.ts"
updated: 2026-06-12
---

# InputTypeConfigurationService

Registry that maps ABP dynamic-property input type names to their corresponding Angular component classes and whether they support value lists.

## Public interface

- `InputTypeConfigurationDefinitions: InputTypeConfigurationDefinition[]`
- `getByName(name: string): InputTypeConfigurationDefinition`
- `getByInputType(inputType: IInputType): InputTypeConfigurationDefinition`

## Dependencies

- [[single-line-string-input-type-component]] registered as the handler for single-line text input types
- [[checkbox-input-type-component]] registered as the handler for boolean/checkbox input types
- [[combobox-input-type-component]] registered as the handler for single-select dropdown input types
- [[multiple-select-combobox-input-type-component]] registered as the handler for multi-select input types

## Used by

- [[input-type-configuration-service]]

## External dependencies

- `@angular/core`

## Notes

Registered as providedIn: 'root'. Provides a simple plugin pattern: add a new InputTypeConfigurationDefinition to extend supported field types.

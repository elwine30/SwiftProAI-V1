---
title: "DomHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/DomHelper.ts"
updated: 2026-06-12
---

# DomHelper

Provides static DOM utility methods for polling until elements exist, creating elements with attributes and querying elements by attribute value.

## Public interface

- `static waitUntilElementIsReady(selector, callback, checkPeriod?): void`
- `static createElement(tag, attributes): any`
- `static getElementByAttributeValue(tag, attribute, value): any`

## Notes

waitUntilElementIsReady uses setInterval polling — callers must ensure they eventually match to avoid memory leaks.

---
title: "XmlHttpRequestHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/XmlHttpRequestHelper.ts"
updated: 2026-06-12
---

# XmlHttpRequestHelper

Wraps raw XMLHttpRequest for fire-and-forget JSON AJAX calls used during pre-bootstrap when Angular's HttpClient is not yet available.

## Public interface

- `static ajax(type, url, customHeaders, data, success, error?): void`

## Notes

Appends a cache-busting timestamp to every request URL. Falls back to abp.localization.localize for error messages. Used only in the pre-bootstrap phase (AppPreBootstrap) — not in components.

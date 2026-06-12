---
title: "DefaultHtmlSanitizer"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Xss/DefaultHtmlSanitizer.cs"
updated: 2026-06-12
---

# DefaultHtmlSanitizer

Minimal HTML sanitiser that strips all HTML tags and HTML entities from user-supplied strings using a regex replacement.

## Public interface

string Sanitize(string html)

## Notes

Uses a simple regex <.*?>|&.*?; which strips all tags but does not perform allowlist-based sanitisation. This is less sophisticated than the Abp.HtmlSanitizer configured in the module, which is used for specific service methods.

---
title: "SwaggerExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Swagger/SwaggerExtensions.cs"
updated: 2026-06-12
---

# SwaggerExtensions

Provides Swagger UI and generation extension methods to inject the ABP base URL into the Swagger index page and to customise generic schema ID generation.

## Public interface

static void InjectBaseUrl(this SwaggerUIOptions options, string pathBase)
static void CustomDefaultSchemaIdSelector(this SwaggerGenOptions options)

## External dependencies

- Swashbuckle.AspNetCore

## Notes

CustomDefaultSchemaIdSelector flattens generic type names using an 'Of' separator (e.g. PagedResultDtoOfUserDto) to avoid Swagger schema ID collisions.

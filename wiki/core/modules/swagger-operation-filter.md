---
title: "SwaggerOperationFilter"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Swagger/SwaggerOperationFilter.cs"
updated: 2026-06-12
---

# SwaggerOperationFilter

Swashbuckle IOperationFilter that replaces enum parameter schemas with a properly generated enum schema so that Swagger UI renders enum dropdowns correctly.

## Public interface

void Apply(OpenApiOperation operation, OperationFilterContext context)

## External dependencies

- Swashbuckle.AspNetCore

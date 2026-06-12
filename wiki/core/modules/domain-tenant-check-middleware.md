---
title: "DomainTenantCheckMiddleware"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/MultiTenancy/DomainTenantCheckMiddleware.cs"
updated: 2026-06-12
---

# DomainTenantCheckMiddleware

ASP.NET Core middleware that extracts a tenant name from the request host using configured domain format patterns and redirects to a 404 error page if no matching tenant exists.

## Public interface

Task InvokeAsync(HttpContext context, RequestDelegate next)

## External dependencies

- Abp.Web.MultiTenancy
- Abp.Domain.Uow

## Notes

Supports semicolon-delimited domain format strings. Ignores requests where hostname matches 'www'. Uses FormattedStringValueExtracter from ABP for pattern matching.

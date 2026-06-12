---
title: "Startup"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/Startup.cs"
updated: 2026-06-12
---

# Startup

Registers all ASP.NET Core services and configures the middleware pipeline, including ABP, CORS, JWT, SignalR, Hangfire, Swagger, GraphQL, health checks, Stripe and reCAPTCHA.

## Public interface

- `Startup(IWebHostEnvironment env)`
- `IServiceProvider ConfigureServices(IServiceCollection services)`
- `void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)`

## Dependencies

- [[auth-configurer]] configures JWT Bearer authentication scheme
- [[thinkn-insur-tech-web-host-module]] ABP module that initialises background workers and OCR registration
- [[angular-app-url-service]] provides Angular SPA route paths for email links
- [[web-url-service]] resolves client and server root addresses from appsettings

## Used by

- [[program]]

## External dependencies

- Abp.AspNetCore
- Abp.AspNetZeroCore.Web
- Abp.Castle.Logging.Log4Net
- Abp.Hangfire
- Abp.HtmlSanitizer
- Hangfire
- Stripe.net
- GraphQL.Server.Ui.Playground
- HealthChecks.UI
- Owl.reCAPTCHA
- Humanizer

## Notes

SIT environment uses friendly error pages rather than developer exception page. A custom middleware rewrites 404s without extensions to index.html (SPA fallback). OpenIddict integration is conditional on appsettings. IgnoreAntiforgeryTokenAttribute is applied globally.

---
title: "ApplicationBuilderExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Extensions/ApplicationBuilderExtensions.cs"
updated: 2026-06-12
---

# ApplicationBuilderExtensions

Provides UseThinknInsurTechForwardedHeaders extension that configures X-Forwarded-For and X-Forwarded-Proto header forwarding with all known-network restrictions cleared for reverse-proxy deployments.

## Public interface

static IApplicationBuilder UseThinknInsurTechForwardedHeaders(this IApplicationBuilder builder)

## External dependencies

- Microsoft.AspNetCore.HttpOverrides

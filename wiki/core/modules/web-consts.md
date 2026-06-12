---
title: "WebConsts"
type: class
language: csharp
layer: shared
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Common/WebConsts.cs"
updated: 2026-06-12
---

# WebConsts

Static constants and mutable flags controlling Swagger UI endpoint path, Hangfire dashboard endpoint and visibility, GraphQL endpoint paths, and reCAPTCHA user-agent whitelist.

## Public interface

const string SwaggerUiEndPoint
const string HangfireDashboardEndPoint
static bool SwaggerUiEnabled
static bool HangfireDashboardEnabled
static List<string> ReCaptchaIgnoreWhiteList
class GraphQL { const string PlaygroundEndPoint; const string EndPoint; static bool PlaygroundEnabled; static bool Enabled; }

## Used by

- [[web-core-module]] sets HangfireDashboardEnabled and SwaggerUiEnabled during module initialisation
- [[token-auth-controller]] reads ReCaptchaIgnoreWhiteList to skip reCAPTCHA validation for whitelisted agents

## Notes

HangfireDashboardEnabled defaults to false. Mutable static fields (SwaggerUiEnabled, HangfireDashboardEnabled) are read by the module and Startup but could be overwritten accidentally at runtime.

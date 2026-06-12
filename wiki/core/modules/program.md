---
title: "Program"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/Program.cs"
updated: 2026-06-12
---

# Program

Application entry point that builds and runs the Kestrel/IIS web host, suppressing EF Core SQL command logging below Warning level.

## Public interface

static void Main(string[] args)
static IWebHostBuilder CreateWebHostBuilder(string[] args)

## Dependencies

- [[startup]] passed as the type argument to UseStartup<Startup>() when building the web host

## External dependencies

- Microsoft.AspNetCore.Hosting

## Notes

Server header is suppressed via opt.AddServerHeader = false. MaxRequestLineSize is raised to 16 KB.

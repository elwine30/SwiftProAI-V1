---
title: "ProfileControllerBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/ProfileControllerBase.cs"
updated: 2026-06-12
---

# ProfileControllerBase

Abstract base controller providing shared profile picture upload (5 MB limit with image validation) and retrieval helpers used by both the MVC and public web entry points.

## Public interface

void UploadProfilePicture(FileDto input)
FileResult GetDefaultProfilePicture()
Task<FileResult> GetProfilePictureByUser(long userId)

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers

## External dependencies

- Abp.AspNetZeroCore.Net

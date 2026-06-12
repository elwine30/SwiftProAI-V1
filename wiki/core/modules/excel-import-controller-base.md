---
title: "ExcelImportControllerBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/ExcelImportControllerBase.cs"
updated: 2026-06-12
---

# ExcelImportControllerBase

Abstract base controller that accepts an Excel file upload (100 MB limit), saves it as a BinaryObject, and enqueues a background import job via a subclass-defined method.

## Public interface

abstract string ImportExcelPermission { get; }
Task<JsonResult> ImportFromExcel()
abstract Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers

## External dependencies

- Abp.BackgroundJobs

## Notes

Uses primary constructor syntax (C# 12). Permission check is performed before reading the file stream.

---
title: "CreateOrEditCasePoliceReportComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/casePoliceReports/create-or-edit-casePoliceReport.component.ts"
updated: 2026-06-12
---

# CreateOrEditCasePoliceReportComponent

Police report entry form that handles file upload, late-report detection, AI-powered report summary generation via OpenAI, and incident time validation.

## Public interface

show(casePoliceReportId?): void
save(): void
saveAndNew(): void — saves and resets form for another entry
generateCasePoliceReportSummary(): void — calls AI summary endpoint and parses JSON response
saveReportSummary(): void
updateLateReport(): void — auto-sets lateReport flag based on time diff
addPoliceReport(policeReport): void
showFileUpload(): void
canDeactivate(): Promise<boolean>|boolean

## Dependencies

- [[case-police-report-file-upload-modal-component]] modal for uploading files to a police report
- [[create-or-edit-case-investigation-officer-component]] sub-form for adding investigation officer details
- [[view-case-police-report-modal-component]] modal for viewing an existing police report

## Used by

- [[case-police-report-file-upload-modal-component]]
- [[create-or-edit-case-investigation-officer-component]]
- [[view-case-police-report-modal-component]]

## External dependencies

- @angular/core
- @angular/forms
- @angular/router
- @angular/common/http
- rxjs/operators
- luxon
- ng2-file-upload
- abp-ng2-module
- sweetalert2

## Notes

Most complex component in the area. Directly parses OpenAI JSON response in the component (extracting via regex), which is a fragile pattern. Uses abp.notify.success (global abp.* namespace) alongside Angular notify service. Fetches allowed file types from a bespoke REST endpoint rather than a proxy service.

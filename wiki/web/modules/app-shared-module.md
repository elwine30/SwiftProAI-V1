---
title: "AppSharedModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/app-shared.module.ts"
updated: 2026-06-12
---

# AppSharedModule

Top-level barrel module for the app shell that re-exports all UI, form and third-party modules consumed by every feature module under app/.

## Public interface

exports: CommonModule, FormsModule, HttpClientModule, AppCommonModule, ServiceProxyModule, TableModule, PaginatorModule, NgxSpinnerModule, AppBsModalModule, IMaskModule, ImageCropperModule, AutoCompleteModule, BsDatepickerModule, FileUploadModule

## Dependencies

- [[app-common-module]] shared UI components, directives and form helpers for the app shell
- [[themes-layout-base-component]] base class providing theme-aware layout scaffolding

## External dependencies

- @angular/core
- @angular/common
- @angular/common/http
- @angular/forms
- ngx-bootstrap/modal
- ngx-bootstrap/datepicker
- ngx-bootstrap/dropdown
- ngx-bootstrap/tabs
- ngx-bootstrap/popover
- primeng/table
- primeng/paginator
- primeng/progressbar
- primeng/autocomplete
- ng2-file-upload
- ngx-image-cropper
- ngx-spinner
- angular-imask
- @craftsjs/perfect-scrollbar

---
title: "FileUploadModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/common/fileOrgs/file-upload-modal.component.ts"
updated: 2026-06-12
---

# FileUploadModalComponent

Drag-and-drop file upload modal that previews images and PDFs, converts the selected file to base64, and submits via FileOrgsServiceProxy.uploadFile.

## Public interface

@Output() fileUploaded: EventEmitter<FileMetadataDto>
open(selectedField): void
close(): void
uploadFile(): Promise<void>
handleFileInput(event): void
onDrop(event): void
onDragOver(event): void
onDragLeave(event): void

## Used by

- [[file-orgs-component]]

## External dependencies

- @angular/core
- @angular/platform-browser
- @angular/router
- ng2-file-upload
- ngx-bootstrap/modal

## Notes

Uses DomSanitizer.bypassSecurityTrustResourceUrl for the preview URL — correct for FileReader data: URLs but must not be used with user-supplied remote URLs. The ng2-file-upload queue is used for UI tracking but the actual upload uses a manual FileReader + base64 approach via the proxy service.

---
title: "FileOrgsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/common/fileOrgs/fileOrgs.component.ts"
updated: 2026-06-12
---

# FileOrgsComponent

File organiser page with a tree-view of folders by entity and field, file list for the selected folder, and inline rename, upload and view actions.

## Public interface

loadFolder(): void — builds tree from FoldersServiceProxy.getAllInDictionary
selectField(field): void — loads file list for selected folder-field
uploadFile(field): void — opens FileUploadModal
viewFile(file): void — opens FileViewerModal
editFile(file): void — enters inline rename mode
saveFileName(file): void — persists renamed filename
cancelEdit(file): void
deleteFile(file): void
getFileTypeIcon(fileType): string
getFileTypeColor(fileType): any

## Dependencies

- [[file-upload-modal-component]] modal for uploading files to a folder
- [[file-viewer-modal-component]] modal for viewing a file inline

## Used by

- [[file-upload-modal-component]]
- [[file-viewer-modal-component]]

## External dependencies

- @angular/core
- @angular/router

## Notes

Tree data is constructed client-side from a dictionary response keyed by entity name then field name. isFileViewOnly flag exists on the component but is not wired to template logic in the read code.

---
title: "UsersComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/users/users.component.ts"
updated: 2026-06-12
---

# UsersComponent

Paginated user management list that supports filtering by role and permissions, Excel import/export, user deletion with guard for the default admin account, lock status display, and launching the create/edit and impersonation flows.

## Public interface

getUsers(event?: LazyLoadEvent): void
deleteUser(user: UserListDto): void
unlockUser(record): void
createUser(): void
exportToExcel($event): void
uploadExcel(data: { files: File }): void
showDynamicProperties(user: UserListDto): void
setUsersProfilePictureUrl(users: UserListDto[]): void
isUserLocked(user: UserListDto): boolean

## Dependencies

- [[create-or-edit-user-modal-component]] modal form for creating or editing a user
- [[edit-user-permissions-modal-component]] modal for editing user-level permission overrides
- [[impersonation-service]] handles browser redirect for impersonating a user
- [[permission-tree-modal-component]] modal filter picker for permission-based user search
- [[users-data-service]] event bus coordinating the user modal and staff sub-modal

## Used by

- [[users-module]]

## External dependencies

- @angular/core
- @angular/common/http
- @angular/router
- primeng/api
- primeng/table
- primeng/paginator
- primeng/fileupload
- rxjs/operators

## Notes

Profile picture URL is constructed by appending an encrypted auth token as a query parameter — reads directly from LocalStorageService. Uses abp-inherited AppComponentBase for l(), notify, and permission helpers.

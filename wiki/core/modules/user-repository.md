---
title: "UserRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/Authorization/Users/UserRepository.cs"
updated: 2026-06-12
---

# UserRepository

Custom repository for the User entity that implements password-expiry queries and bulk-updates users to force a password change on next login.

## Public interface

List<long> GetPasswordExpiredUserIds(DateTime passwordExpireDate)
void UpdateUsersToChangePasswordOnNextLogin(List<long> userIdsToUpdate)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing DbContext access
- [[thinkn-insur-tech-db-context]] direct DbSet access for password expiry queries

## Used by

- [[thinkn-insur-tech-db-context]]

## External dependencies

- Abp.EntityFrameworkCore
- Z.EntityFramework.Plus

## Notes

Uses Z.EntityFramework.Plus Update() for a set-based bulk update without loading entities into memory. Password expiry logic checks RecentPasswords and falls back to User.CreationTime when no recent password record exists.

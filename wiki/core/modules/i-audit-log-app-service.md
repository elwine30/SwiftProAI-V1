---
title: "IAuditLogAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Auditing/IAuditLogAppService.cs"
updated: 2026-06-12
---

# IAuditLogAppService

ABP platform-level audit log service contract for querying user action logs, entity change history and property-level diffs.

## Public interface

Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input)
Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input)
Task<PagedResultDto<EntityChangeListDto>> GetEntityTypeChanges(GetEntityTypeChangeInput input)
Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input)
Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId)
List<NameValueDto> GetEntityHistoryObjectTypes()

## External dependencies

- Abp.Core

## Notes

Distinct from IAuditTrailsAppService — this covers ABP framework-level action/entity audit whereas the other is domain-level business trails.

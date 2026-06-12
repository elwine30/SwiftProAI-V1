---
title: "DateTimeService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/timing/date-time.service.ts"
updated: 2026-06-12
---

# DateTimeService

Centralises date and time calculations using Luxon, respecting the ABP multi-timezone setting to return dates in the correct user timezone.

## Public interface

- `getDate(): DateTime` — current date in user timezone
- `getUTCDate(): DateTime`
- `createDateRangePickerOptions(): any`
- `formatDate(date, format): string`
- `fromISODateString(date): DateTime`
- `fromJSDate(date): DateTime`
- `plusDays/minusDays/plusSeconds(date, n): DateTime`
- `toUtcDate(date): DateTime`
- `changeTimeZone(date, ianaTimezoneId): Date`
- `getDiffInSeconds(maxDate, minDate)`

## Dependencies

- [[app-localization-service]] provides localised strings for date range picker labels

## Used by

- [[app-common-module]]
- [[themes-layout-base-component]]
- [[default-layout-component]]
- [[chat-bar-component]]
- [[user-notification-helper]]

## External dependencies

- `@angular/core`
- `luxon`

## Notes

Checks abp.clock.provider.supportsMultipleTimezone before applying timezone offset — callers must use this service rather than Luxon directly to stay timezone-aware.

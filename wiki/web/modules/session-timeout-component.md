---
title: "SessionTimeoutComponent"
type: component
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/session-timeout/session-timeout.component.ts"
updated: 2026-06-12
---

# SessionTimeoutComponent

Monitors user inactivity across mouse, keyboard and scroll events and triggers the session-timeout warning modal when the configured idle period has elapsed.

## Dependencies

- [[session-timeout-modal-component]] the countdown modal displayed when the idle threshold is reached

## External dependencies

- `@angular/core`
- `rxjs`

## Notes

Shares last-activity timestamp via localStorage (or ABP cookie fallback) so multiple tabs detect inactivity. Reads timeout threshold from ABP setting App.UserManagement.SessionTimeOut.TimeOutSecond.

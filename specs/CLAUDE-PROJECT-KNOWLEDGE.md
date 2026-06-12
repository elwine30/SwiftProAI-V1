# SwiftProAI — Claude AI project knowledge

*This file is the project memory for Claude AI. Read this before answering any question about SwiftProAI.*

---

## What SwiftProAI is

SwiftProAI is a multi-tenant SaaS platform purpose-built for motor insurance claims adjusting firms in Malaysia. It manages the end-to-end lifecycle of a motor claim investigation: from case intake and adjuster field investigation through evidence collection, OCR document digitisation, expense management, report generation, editor quality review, invoicing and final document delivery to the instructing insurer. The primary client is Jaya Adjusters Sdn Bhd, operated through the internal company Thinkn Insurtech Services Sdn Bhd.

---

## Business context

- **Industry**: Motor insurance claims adjusting (loss adjusting), Malaysia
- **Primary client**: Jaya Adjusters Sdn Bhd
- **Internal operating entity**: Thinkn Insurtech Services Sdn Bhd
- **Business model**: Multi-tenant SaaS; subscription billing via Stripe and PayPal; one tenant per adjusting firm
- **Geographic focus**: Malaysia; Bahasa Malaysia language pack added in v2
- **Key external partner**: Merimen (Malaysia's dominant insurance case portal — cases are currently re-keyed manually; Merimen API integration is the highest-value v2 feature)

---

## Six user roles

| Role | Description |
|---|---|
| Superadmin | Platform host administrator; manages tenants, editions and host-level settings |
| OU Admin / CEO / Manager / Ops Manager | Adjusting company management; assigns cases, approves expenses, views all reports for their OU |
| Support Staff | Registers cases, updates case information, prepares and delivers invoices |
| Adjuster | Conducts field investigations, uploads evidence, records expenses, generates investigation reports |
| Editor | Reviews investigation reports; approves, requests amendments or progresses cases (workflow formalised in v2) |
| Accounting | Approves expenses, updates payment status, manages billing records |
| Insurer / Lawyer / Workshop (third party) | Read-only portal access to cases assigned to their organisation, gated by an admin approval workflow |

---

## Core domain concepts

| Term | Definition |
|---|---|
| Case / Registration | A single motor insurance claim investigation. Primary aggregate: `MainRegistration`. Identified by an auto-generated CaseNo (e.g. `JA-001234`) using OU-configured prefix and length. |
| Case status | Sequential workflow states: New (v2) → Under Investigation → Awaiting Editor Review (v2) → Amendment Required (v2) → Pending Invoice → Completed Invoices. Cancellation applies from any state. |
| Adjuster | The field investigator assigned to a case. In v1 this assignment happened by phone/email before registration; v2 adds a formal in-system assignment screen. |
| Editor | A quality reviewer who reads the adjuster's investigation report. In v1 the Editor role was passive and had no dedicated workflow; v2 adds Editor assignment, an Editor Queue and an approval/amendment cycle. |
| WIP (Work in Progress) | Open cases not yet invoiced. The WIP Report shows aging days against a due date threshold. In v1 this was hardcoded at 14 days; v2 makes it configurable per case type and per insurer. |
| Merimen | Malaysia's dominant insurance case portal. Insurers send assignment letters via Merimen. v1 requires manual re-keying; v2 targets full API integration for automated inbound case import and outbound delivery. |
| OU (Organisation Unit) | An ABP Framework organisation unit representing one adjusting firm branch or division. All data is OU-scoped. DocumentSettings (case ref number prefix, invoice prefix etc.) are per OU. |
| ViewThirdPartyCases | The join table that grants a third-party OU (insurer, law firm, workshop) read access to specific cases. Created by the AllowToViewAssignedCases approval workflow. |
| BinaryObject / BlobFile | v1 stored file attachments in the ABP BinaryObject table (SQL Server). v2 migrates to Azure Blob Storage via a new `BlobFile` entity (Guid PK, BlobUri). |
| OCR | Document digitisation via OpenAI GPT-4o Vision API. Triggered on file upload; in v1 this was synchronous; v2 decouples it to an async Hangfire job pipeline with SignalR push of results. |
| CaseNo | The human-readable case reference (e.g. `JA-001234`). Auto-generated from OU DocumentSettings. Distinct from the integer `Id`. |

---

## v1 modules (what exists today)

| Module | Purpose | Status |
|---|---|---|
| Dashboard | Centralised case overview with Under Investigation, Pending Invoices, Completed Invoices and Cancelled tabs | Delivered |
| Registration | New case registration, duplicate case registration, case search and list | Delivered |
| Case detail sub-tabs | 14 sub-tabs per case: Adjuster, Insured Owner, Insured Driver, Insurer, Lawyers, Workshop, Incident Details, Police Report, Third-Party Vehicle, Third-Party Personal Details, Claims/Expenses, Search Fees, Declaration Answers, Remarks, File Organiser | Delivered |
| OCR integration | OpenAI GPT-4o document digitisation; synchronous; prompt creation blocked in Admin | Delivered (partial) |
| Invoicing | Invoice, debit note and credit note generation; preview and print | Delivered |
| Reports | WIP, WIP Summary, Payment Update, Invoice, Case, Adjuster, Compliance; Excel export | Delivered |
| DOCX report generator | Open XML SDK investigation report from template with image embedding | Delivered |
| Claims and expenses | Adjuster expense entry; bulk approve/reject; payment status update | Delivered |
| Admin | Branch, Group, Staff, Insurance Company, Lawyer, Workshop, CaseType, Users, Audit Trails, OCR Logs, Prompts, Declaration Questions, Lookups, OU onboarding | Delivered |
| Third-party portal | Read-only case visibility for insurers, law firms and workshops | Delivered |
| Platform / multi-tenancy | ABP Zero multi-tenancy, RBAC, 2FA (Twilio SMS), SignalR chat, Hangfire, Stripe and PayPal billing | Delivered |

---

## v2 objectives

v2 is a targeted evolution of v1, not a greenfield rewrite. The ABP Framework foundation and Angular SPA are retained.

1. **Close the four documented workflow gaps**: formal in-system adjuster assignment (Gap 1); Editor assignment and queue (Gap 2); Editor approval and amendment workflow with in-system decisions (Gap 3); removal of the manual editor-record-update step before invoicing (Gap 4).
2. **Merimen API integration**: automated inbound case import and outbound investigation delivery, eliminating manual re-keying.
3. **Async OCR pipeline**: decouple OCR from the synchronous HTTP request path using Hangfire and SignalR; add OCR result correction workflow; enable prompt creation via Admin UI.
4. **Configurable WIP due date thresholds**: replace the hardcoded 14-day rule with per-case-type and per-insurer configuration.
5. **Financial and performance analytics**: Adjuster Performance Report and Financial Dashboard (accounts receivable aging, cash flow, unpaid invoices) for management and accounting.
6. **Mobile-first adjuster field app**: photo capture, GPS mileage auto-calculation and offline expense entry (scope confirmed; delivery timeline TBD).
7. **Tech debt and security remediation**: fix commented-out permission decorators, `double` → `decimal` financial type corrections, fire-and-forget async methods that suppress exceptions, duplicate query logic, missing navigation properties, hardcoded pagination.
8. **Containerisation and cloud-readiness**: Docker + Docker Compose for local; Kubernetes or Azure Container Apps for higher environments; Azure Blob Storage for files; Redis enabled (was commented out in v1).

---

## Codebase orientation

- **Repo**: https://github.com/elwine30/SwiftProAI-V1
- **Backend root**: `SwiftProAI.Core/` — ASP.NET Core 8, ABP Framework 9.1 (AspNetZero), SQL Server 2022, Entity Framework Core 8
- **Frontend root**: `SwiftProAI.Web/` — Angular 17, PrimeNG 17, Metronic theme (13 variants), NSwag-generated TypeScript proxies, Luxon date handling
- **Internal namespace (v1)**: `ThinknInsurTech` (all C# projects, migrations, entities)
- **Internal namespace (v2)**: `SwiftProAI` (namespace rename applies to all new and refactored code)
- **Wiki**: `wiki/core/` and `wiki/web/` — Obsidian-format module notes from reverse engineering
- **Specs**: `specs/functional-spec-v2.md` and `specs/technical-spec-v2.md` — authoritative v2 requirements

### Backend project structure (v2 target)

```
SwiftProAI.sln
├── SwiftProAI.Core                    # Domain entities, managers, enums, IAbpSession override
├── SwiftProAI.Application             # App services, DTOs, AutoMapper profiles
├── SwiftProAI.Application.Shared      # Shared DTOs and interfaces for NSwag proxy generation
├── SwiftProAI.EntityFrameworkCore     # DbContext, EF Core config, migrations, custom repos
├── SwiftProAI.Web.Core                # JWT handler, OpenIddict, OCR controllers, SignalR wiring
├── SwiftProAI.Web.Host                # Startup, Swagger, Hangfire, CORS, SPA host
├── SwiftProAI.GraphQL                 # Read-only GraphQL schema (Users, Roles, OUs)
└── SwiftProAI.Migrator                # Standalone migration runner for deployment pipelines
```

---

## Key architectural decisions

- **API-first, Angular SPA**: the backend is a REST API; there are no server-rendered views. The Angular SPA is a first-party consumer of the same versioned API available to third-party integrators.
- **ABP Framework 9.1 (AspNetZero) retained**: multi-tenancy, identity, RBAC, audit logging and background job infrastructure are production-grade and not being rewritten.
- **SQL Server (not MySQL)**: the v1 architecture document incorrectly states MySQL. The actual implementation and all v2 work targets SQL Server. Do not use the v1 architecture document as reference.
- **Redis must be enabled in v2**: Redis was present in v1 code but commented out. v2 requires it for distributed cache, SignalR backplane (multi-node), security stamp validation and 2FA code storage.
- **File storage migrating to Azure Blob Storage**: v1 stored files in the ABP `BinaryObject` SQL table. v2 introduces the `BlobFile` entity (Guid PK, `BlobUri`) backed by Azure Blob Storage. File GUIDs on case entities become foreign keys to `BlobFile`.
- **OCR pipeline is async in v2**: file uploads return immediately; OCR runs as a Hangfire background job; results are pushed via SignalR. `CasePoliceReport` and related entities carry `OcrJobId` and `OcrStatus` fields for tracking.
- **Modular monolith, not microservices**: v2 retains the monolith with enforced module boundaries. High-intensity AI concerns are decoupled internally via Hangfire queues, not via separate services.
- **GraphQL is read-only**: no mutations or subscriptions. Covers Users, Roles and OUs only. Not in scope for expansion in v2.
- **ngx-bootstrap removed**: v2 standardises on PrimeNG 17 for all UI components.
- **OpenIddict production certificates**: v1 used development certificates. v2 requires real signing and encryption certificates before go-live.
- **Financial types corrected**: `CaseExpense.Amount` and `ApprovedAmount` were `double` in v1; corrected to `decimal` in v2. All monetary fields must use `decimal`.

---

## Active feature backlog (top items)

1. Formal adjuster assignment workflow — new "New" case status, assignment screen, in-system notifications to adjuster and Support Staff.
2. Editor assignment and Editor Queue tab — "Send to Editor" action from Adjuster, dedicated queue on Dashboard.
3. Editor approval and amendment workflow — Approve/Request Amendment/Override Submit actions; amendment notes; multiple cycles; audit trail.
4. Merimen API integration — automated case ingest from Merimen portal; outbound delivery of investigation results.
5. Async OCR pipeline — Hangfire job queue, SignalR result push, OCR result correction UI before save.
6. OCR prompt creation via Admin UI — unblock the commented-out create endpoint and wire the Admin CRUD screen.
7. Configurable WIP due date thresholds — per-case-type and per-insurer overrides; fallback to OU default (14 days).
8. Financial Dashboard — accounts receivable aging buckets, cash flow by period, unpaid invoices list.
9. Adjuster Performance Report — turnaround days, cases per month, amendment rate, on-time rate; Excel export.
10. Batch invoice generation and direct email delivery to insurer.
11. Scheduled report delivery via Hangfire (WIP report emailed to nominated recipients on cron schedule).
12. Azure Blob Storage migration — replace ABP BinaryObject DB storage.
13. Security remediation — restore `AbpAuthorize` on `ViewThirdPartyCaseRequestsAppService`; add CSRF protection; replace development certificates.
14. Tech debt — fix `double` → `decimal`, fire-and-forget async, duplicate GetAll/GetToExcel query logic, missing `CaseInsurer` navigation property.
15. Branding and UI amendments — logo, favicon, copyright year, footer URL, remove Traccar/JPJ Search/Commercial Value Check menu items, fix white space on Dashboard and Change Password.

---

## Integration landscape

| System | Purpose | Protocol | Direction |
|---|---|---|---|
| Merimen | Insurance case portal; inbound assignment letters and outbound results | REST API (v2; manual re-key in v1) | Bidirectional |
| OpenAI (GPT-4o) | Document OCR via Vision API; PDF-to-PNG conversion before forwarding | REST / HTTPS | Outbound |
| Stripe | SaaS subscription billing | REST API + webhook | Bidirectional |
| PayPal | SaaS subscription billing (incomplete in v1; targeted for completion in v2) | REST API | Bidirectional |
| Twilio | Two-factor authentication via SMS | REST API | Outbound |
| Azure Blob Storage | File attachment storage (v2; replaces ABP BinaryObject) | Azure SDK v12 | Bidirectional |
| Redis | Distributed cache, SignalR backplane, security stamp, 2FA codes | Redis protocol | Internal |
| Hangfire | Background job processing (OCR pipeline, scheduled reports) | In-process | Internal |
| SignalR | Real-time chat, notifications, OCR result push | WebSocket | Server-to-client push |
| GraphQL | Read-only data query surface (Users, Roles, OUs) | HTTP/GraphQL | Inbound |
| SMTP / MailKit | Email notifications and invoice delivery | SMTP | Outbound |
| Google reCAPTCHA | Bot protection on public forms | REST API | Outbound |
| Azure AD / OAuth | Social and enterprise login | OAuth 2.0 / OIDC | Inbound |

---

## Data model summary

The central entity is **MainRegistration** (the case). Every case sub-entity carries a `RegisterId` foreign key back to it. Key relationships:

- **One-to-one**: MainRegistration has one CaseIncidentDetail, one CasePoliceReport, one CaseAdjuster, one CaseClaim.
- **One-to-many**: MainRegistration has many CaseInsuredPersons (IsOwner/IsDriver flags distinguish roles), CaseThirdPartyInfos, CaseThirdPartyVehicles, CaseInvoices, CaseExpenses, CaseRemarks, FileOrg entries, CaseLawyers, CaseWorkshops, CaseDeclarationAnswers.
- **Third-party access**: ViewThirdPartyCases is a join table granting a third-party OU (insurer/lawyer/workshop) read access to specific RegisterIds. Populated by the AllowToViewAssignedCases approval workflow.
- **Reference data** (not tenant-scoped): Location (countries/states, self-referencing), Hospital, Vehicle make/model/spec.
- **Tenant-scoped master data**: InsuranceCompany, LawFirm, Workshop, Branch, Group, Staff, CaseType, ScopeAssignment, DeclarationQuestion, Lookup (generic picklist).
- **AI/OCR**: OpenAIIntegrationLog (per API call with token counts and TotalCost, linked by CaseNo); Prompt (OCR template, host-level shared).
- **Audit**: AuditTrail (one per save operation) with child AuditEntry rows (one per changed field).
- **File storage (v2)**: BlobFile entity (Guid PK, BlobUri, OcrStatus, OcrJobId). All document file references on case entities point to BlobFile GUIDs.
- **Financial precision**: all monetary fields use `decimal`. v1 used `double` on CaseExpense (corrected in v2 migration).

---

## Known constraints for v2

- The ABP Framework 9.1 (AspNetZero) scaffolding and conventions must be followed. Do not bypass ABP's DI container, repository pattern, permission system or audit logging.
- All monetary fields must use `decimal`, not `double` or `float`.
- The `ThinknInsurTech` namespace applies to all v1 code. New v2 code uses the `SwiftProAI` namespace. Do not mix namespaces within a project.
- Data visibility rules must be enforced in the query layer (not post-fetch filtering). Adjusters see only own-assigned cases; Editors see only formally assigned cases; third parties see only ViewThirdPartyCases-linked cases.
- The New and Amendment Required case statuses must not be exposed to third-party portal users. Their view shows Under Investigation until the case reaches Pending Invoice.
- Case status transitions must follow the defined sequence: New → Under Investigation → Pending Invoice → Completed Invoices. Transitions cannot be skipped except by Superadmin override. Cancellation applies from any status.
- DocumentSettings (CaseRefNoPrefix, InvoicePrefix etc.) must be unique across all tenants.
- Do not use the v1 architecture documents (dated 01/10/2023 and 29/04/2024) as technical reference — they contain material inaccuracies (MySQL stated, SQL Server used; MVC stated, API-first implemented; "no batch jobs" stated, Hangfire present).
- All app service endpoints must have `[AbpAuthorize]` decorators active. The commented-out decorator on `ViewThirdPartyCaseRequestsAppService` is a security defect to be corrected before v2 go-live.
- The OCR endpoint commented out for prompt creation in v1 Admin must be enabled in v2 — do not leave it commented out.

---

## How to work on v2 with Claude

**Always provide these files as context when asking about v2:**
- `specs/functional-spec-v2.md` — business requirements, user stories, acceptance criteria per module
- `specs/technical-spec-v2.md` — architecture, domain model with full field tables, app service signatures, tech debt list

**For entity or database questions**: reference Section 3.2 of the technical spec (domain model). Field types, FK relationships and v1-to-v2 type corrections are all documented there.

**For workflow questions**: reference Section 5 of the functional spec (workflow diagrams) and Sections 4.2–4.4 (registration, investigation, editor workflow).

**For permission questions**: reference Section 3 of the functional spec (permission matrix and data visibility rules).

**Structuring requests effectively:**
- State which module you are working on (e.g. "Editor workflow — Section 4.4 of functional spec").
- State whether you need backend (C#/ABP), frontend (Angular/PrimeNG) or both.
- For new entities or migrations, specify the target v2 namespace (`SwiftProAI`) and confirm whether the entity is tenant-scoped (`IMustHaveTenant`) or not.
- For financial fields, always confirm `decimal` type is used.
- For permission checks, reference the permission matrix in Section 3.2 of the functional spec.

**Key files in the codebase to orient quickly:**
- Domain entities: `SwiftProAI.Core/` — entities, enums, managers
- App services: `SwiftProAI.Application/` — CRUD orchestration and business logic
- DB context and migrations: `SwiftProAI.EntityFrameworkCore/`
- Angular components: `SwiftProAI.Web/src/app/`
- NSwag-generated proxies: `SwiftProAI.Web/src/shared/service-proxies/`

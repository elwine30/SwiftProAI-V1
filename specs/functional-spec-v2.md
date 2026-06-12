# SwiftProAI v2 — functional specification

**Version:** 1.0 Draft
**Date:** 12/06/2026
**Status:** For review
**Prepared by:** Business Analysis, ABOD Technology Services
**Client:** Thinkn Insurtech Services Sdn Bhd

---

## Table of contents

1. [Executive summary](#1-executive-summary)
2. [Background and context](#2-background-and-context)
3. [User roles and permissions](#3-user-roles-and-permissions)
4. [System modules](#4-system-modules)
5. [Workflows](#5-workflows)
6. [Data requirements](#6-data-requirements)
7. [Integration requirements](#7-integration-requirements)
8. [Non-functional requirements](#8-non-functional-requirements)
9. [v2 new features and enhancements](#9-v2-new-features-and-enhancements)
10. [Out of scope for v2](#10-out-of-scope-for-v2)
11. [Open questions](#11-open-questions)

---

## 1. Executive summary

SwiftProAI is a multi-tenant SaaS platform purpose-built for motor claims adjusting firms operating in Malaysia. The platform manages the end-to-end lifecycle of a motor insurance claim investigation: from case intake and field investigation through evidence collection, expense management, report generation, quality review, invoicing and final document delivery to the instructing insurer.

SwiftProAI v2 builds on the foundation delivered in v1 and addresses the four documented workflow gaps, a set of approved client amendment requests and a broader set of enhancement opportunities identified through reverse engineering of the v1 codebase and source documents.

The primary improvements in v2 are:

- A formal in-system adjuster assignment workflow that replaces the current manual phone/email process before case registration.
- A dedicated Editor assignment and approval workflow, replacing ad hoc verbal communication between Adjuster and Editor.
- An Editor approval gate with an in-system amendment queue, giving Editors the ability to approve, request amendments or progress a case without adjuster involvement.
- Merimen API integration for automated case import and output delivery, eliminating manual re-keying of assignment data.
- Configurable OCR prompt management so administrators can add new document types without code changes.
- Configurable WIP due date thresholds per case type or insurer, replacing the hardcoded 14-day rule.
- A suite of financial and performance analytics dashboards.
- A mobile-first adjuster field application for photo capture, GPS mileage auto-calculation and offline expense entry.
- Multiple targeted tech debt and security fixes identified in the v1 codebase.

**Target users:** Adjusting company staff (Support Staff, Adjusters, Editors, Accounting), management (CEO/Manager/Operations Manager), third-party partners (Insurance Companies, Law Firms, Workshops) and platform administrators.

**Technology stack:** ASP.NET Core (ABP Framework), Angular, SignalR, Hangfire, Redis, SQL Server. v2 extends v1; it does not replace the underlying platform.

---

## 2. Background and context

### 2.1 What SwiftProAI v1 delivered

SwiftProAI v1 delivered a comprehensive case management platform covering:

- Multi-tenant organisation unit (OU) onboarding for adjusting companies, insurers, law firms and workshops.
- End-to-end case lifecycle management with five status stages: Under Investigation, Pending Invoice, Completed Invoices and Cancelled.
- Structured data capture across 14 case sub-tabs: adjuster profile, insured owner, insured driver, insurer contact, lawyers, workshops, incident details, police report, third-party vehicle, third-party personal details, claims/expenses, search fees, declaration answers, remarks and a hierarchical file organiser.
- OpenAI Vision API integration for OCR of identity cards, driving licences, police reports and insurance documents, with token-level cost logging.
- Invoice, debit note and credit note generation with configurable reference number prefixes per OU.
- Seven report types: WIP, WIP Summary, Payment Update, Invoice, Case, Adjuster and Compliance, all exportable to Excel.
- A DOCX investigation report generator using Open XML SDK with image embedding.
- A read-only third-party portal for insurers, law firms and workshops with approval-gated case visibility.
- ABP Framework multi-tenancy, role-based access control, two-factor authentication via Twilio SMS, real-time SignalR chat and notifications, Hangfire background jobs, and Stripe and PayPal subscription billing.

### 2.2 Why v2 is needed

Four structural workflow gaps were documented during v1 acceptance testing and recorded in the System_Requirement_Gap.xlsx:

**Gap 1** — The adjuster assignment process occurs outside the system. The CEO/Manager communicates the assignment to Support Staff verbally or by email before the case can be registered. There is no in-system record of this decision, no notification and no accountability trail.

**Gap 2** — The Editor role is passive. Editors have no dedicated queue, receive no in-system notification when a case is ready for review and access the same Adjusters tab as the Adjuster. There is no formal "send to editor" action.

**Gap 3** — There is no Editor approval or amendment workflow. The Editor communicates decisions to the Adjuster manually. No in-system decision record exists. The Adjuster must initiate all status progression even when the Editor has approved the work.

**Gap 4** — Support Staff must manually update the editor record on a case before preparing an invoice. This step exists only because the editor was never formally assigned in the system.

Beyond these gaps, v1 contains tech debt (hardcoded rules, duplicated query logic, a security decorator commented out on an approval endpoint, fire-and-forget async methods that suppress exceptions) and a set of approved branding and UI amendments that were deferred from v1.

### 2.3 Key lessons from v1

- **Workflow state machines must be explicit.** Implicit status progressions driven by status codes on child records (CaseAdjuster.StatusCode = 5 propagates to MainRegistration) are fragile. v2 will use explicit workflow transitions with named triggers.
- **Role separation must be enforced in the UI.** Editors and Adjusters sharing a single tab caused confusion in v1. v2 will provide role-specific views.
- **Configurability reduces maintenance cost.** Hardcoded values (14-day due date, page size of 5, blocked OCR prompt creation) generated change requests that required code deployments. v2 will expose these as configuration parameters.
- **External integration is critical path.** The absence of Merimen API integration means all inbound case data is re-keyed manually. This is the single largest efficiency gain available in v2.
- **Security must be applied consistently.** Commented-out permission decorators on approval endpoints represent an unacceptable security posture that must be corrected before v2 goes live.

---

## 3. User roles and permissions

### 3.1 Role definitions

| Role | Description |
|---|---|
| Superadmin | Platform host administrator. Manages tenants, editions, subscriptions and host-level settings. |
| OU Admin | Organisation unit administrator. Full access within own OU. Equivalent to the CEO/Manager role for their organisation. |
| CEO/Manager | Senior management at an adjusting company. Assigns cases, approves expenses, views all reports. |
| Operations Manager | Operational oversight. Similar to CEO/Manager but without user management. |
| Support Staff | Registers cases, updates case information, prepares invoices and delivers documents. |
| Adjuster | Conducts field investigations, uploads evidence, records expenses, generates reports. |
| Editor | Reviews investigation reports, requests amendments or approves and progresses cases. |
| Accounting | Manages expense approval, payment status updates and billing records. |
| Insurer (third party) | Read-only access to cases assigned to their organisation via the third-party portal. |
| Lawyer (third party) | Read-only access to cases in which their firm is engaged. |
| Workshop (third party) | Read-only access to cases in which their workshop is engaged. |

### 3.2 Permission matrix

The following table defines the access rights for each role. Cells marked **Y** indicate full access, **RO** indicates read-only access, **Own** indicates access limited to own records, **OU** indicates access limited to own organisation unit and a dash indicates no access.

| Capability | Superadmin | OU Admin / CEO / Ops Mgr | Support Staff | Adjuster | Editor | Accounting | Insurer | Lawyer / Workshop |
|---|---|---|---|---|---|---|---|---|
| **Dashboard — all cases** | Y | Y | Y | Own | Own (assigned) | Y | — | — |
| **Dashboard — third-party portal** | — | — | — | — | — | — | Y | Y |
| **Register new case** | Y | Y | Y | — | — | — | — | — |
| **Register duplicate case** | Y | Y | Y | — | — | — | — | — |
| **Search cases** | Y | Y | Y | Own | Own (assigned) | Y | — | — |
| **Assign adjuster to case** | Y | Y | — | — | — | — | — | — |
| **Reassign adjuster** | Y | Y | — | — | — | — | — | — |
| **Reassign insurance company** | Y | Y | — | — | — | — | — | — |
| **Update case information (pre-investigation)** | Y | Y | Y | — | — | — | — | — |
| **View case details** | Y | Y | Y | Own | Own (assigned) | Y | RO | RO |
| **Update case sub-tabs (investigation)** | Y | Y | — | Own | — | — | — | — |
| **Upload documents and evidence** | Y | Y | — | Own | — | — | — | — |
| **Run OCR on documents** | Y | Y | — | Own | — | — | — | — |
| **Enter expenses and claims** | Y | Y | — | Own | — | — | — | — |
| **Enter search fees** | Y | Y | — | Own | — | — | — | — |
| **Answer declaration questions** | Y | Y | — | Own | — | — | — | — |
| **Add case remarks** | Y | Y | Y | Own | Own (assigned) | — | — | — |
| **Generate DOCX investigation report** | Y | Y | — | Own | — | — | — | — |
| **Assign case to Editor (new in v2)** | Y | Y | — | Own | — | — | — | — |
| **Download and review report (Editor)** | Y | Y | — | — | Own (assigned) | — | — | — |
| **Re-upload amended report** | Y | Y | — | — | Own (assigned) | — | — | — |
| **Approve case report (new in v2)** | Y | Y | — | — | Own (assigned) | — | — | — |
| **Request amendment (new in v2)** | Y | Y | — | — | Own (assigned) | — | — | — |
| **Submit case after Editor approval** | Y | Y | — | Own | Own (assigned) | — | — | — |
| **View Pending Invoices** | Y | Y | Y | — | — | Y | — | — |
| **Generate invoice** | Y | Y | Y | — | — | — | — | — |
| **Generate debit note** | Y | Y | Y | — | — | — | — | — |
| **Generate credit note** | Y | Y | Y | — | — | — | — | — |
| **View Completed Invoices** | Y | Y | Y | — | — | Y | — | — |
| **Approve/reject expenses (bulk)** | Y | Y | — | — | — | Y | — | — |
| **Update payment status** | Y | Y | — | — | — | Y | — | — |
| **Update billing records** | Y | Y | — | — | — | Y | — | — |
| **View WIP report** | Y | Y | Y | Own | — | Y | — | — |
| **View Invoice report** | Y | Y | Y | — | — | Y | — | — |
| **View Adjuster report** | Y | Y | Y | — | — | Y | — | — |
| **View Compliance report** | Y | Y | Y | — | — | — | — | — |
| **View financial dashboards (new in v2)** | Y | Y | — | — | — | Y | — | — |
| **Manage branches** | Y | OU | — | — | — | — | — | — |
| **Manage groups** | Y | OU | — | — | — | — | — | — |
| **Manage staff** | Y | OU | — | — | — | — | — | — |
| **Manage insurance companies** | Y | OU | — | — | — | — | — | — |
| **Manage law firms** | Y | OU | — | — | — | — | — | — |
| **Manage workshops** | Y | OU | — | — | — | — | — | — |
| **Manage case types** | Y | OU | — | — | — | — | — | — |
| **Manage users and roles** | Y | OU | — | — | — | — | — | — |
| **Manage declaration questions** | Y | OU | — | — | — | — | — | — |
| **Manage OCR prompts (new in v2)** | Y | — | — | — | — | — | — | — |
| **View audit trails** | Y | OU | — | — | — | — | — | — |
| **View OpenAI integration logs** | Y | OU | — | — | — | — | — | — |
| **Tenant management** | Y | — | — | — | — | — | — | — |
| **Edition management** | Y | — | — | — | — | — | — | — |
| **Host settings** | Y | — | — | — | — | — | — | — |
| **Third-party access request approval** | Y | OU | — | — | — | — | — | — |
| **OU onboarding** | Y | — | — | — | — | — | — | — |

### 3.3 Data visibility rules

- **Superadmin / Admin without OU**: sees all cases across all tenants and OUs.
- **OU Admin / CEO / Ops Mgr**: sees all cases within own OU.
- **Support Staff**: sees all cases within own OU.
- **Adjuster**: sees only cases where they are the assigned adjuster.
- **Editor**: sees only cases that have been formally assigned to them via the Editor assignment workflow.
- **Accounting**: sees all cases within own OU for expense and invoice purposes.
- **Insurer (third party)**: sees only cases linked via the ViewThirdPartyCases join table, controlled by the AllowToViewAssignedCases approval workflow.
- **Lawyer / Workshop (third party)**: sees only cases in which their organisation has been engaged and for which the AllowToViewAssignedCases workflow has been approved.

---

## 4. System modules

### 4.1 Dashboard

#### Purpose

Provide a centralised, role-appropriate overview of case workload, key performance indicators and action items for all internal users.

#### Key features

- **Under Investigation tab**: paginated list of active cases. Columns: case file number, case type, insured vehicle, assigned adjuster, status, aging days, action buttons (View, Add Expenses, Status History).
- **Pending Invoices tab**: cases where the Adjuster has submitted and the case is ready for invoicing. Invoice generator with line-item charges (mileage, tolls, airfare, hotel, police fees, fraud, miscellaneous, photograph charge, service fee). Total amount table. Invoice preview and print. Status progression to Completed Invoices on save.
- **Completed Invoices tab**: invoiced cases. Debit note and credit note generators. Preview and print.
- **Cancelled tab**: cancelled case listing.
- **Case Details sub-view**: read-only summary of any case accessible from any tab.
- **Status History panel**: chronological log of status changes per case with actor and timestamp.
- **Add Expenses dialog**: accessible from Under Investigation; fields for mileage, tolls, airfare, hotel, police fees, fraud amounts and miscellaneous.
- **Adjusters tab (v1 read-only view)**: v2 replaces this with the formal Adjuster Assignment workflow screen (see Section 4.2).
- **Editor Queue tab (new in v2)**: dedicated queue for the Editor role showing cases assigned for review, amendment requests and approval decisions.

#### User stories

- As a Support Staff user, I want to see all active cases in my OU on the Under Investigation tab so that I can quickly identify cases requiring action.
- As an Adjuster, I want to see only my assigned cases on the dashboard so that I am not distracted by cases I am not responsible for.
- As a CEO/Manager, I want to see the aging days for all active cases so that I can identify cases approaching or exceeding their due dates.
- As an Editor, I want to see my Editor Queue on the dashboard so that I know which cases are waiting for my review.
- As Accounting, I want to see the Pending Invoices tab so that I can monitor cases ready for financial processing.

#### Business rules

- Aging days are computed dynamically at query time as `DateTime.Now - AssignTime`.
- The Under Investigation tab excludes cases with status Completed Invoices (status 4) and Cancelled (status 5).
- The dashboard respects the data visibility rules defined in Section 3.3.
- The Editor Queue tab is visible only to users with the Editor role.
- Pagination defaults to 20 rows per page. Page size is configurable by the user up to 100.

#### Acceptance criteria

- The dashboard loads within two seconds for a user with up to 500 active cases.
- Aging days display correctly and update on each page load without caching.
- Role-based visibility is enforced: an Adjuster cannot see another Adjuster's cases.
- The Editor Queue displays only cases assigned to the logged-in Editor.
- Invoice generation from the Pending Invoices tab correctly calculates total amounts including all line items.

#### v2 enhancements over v1

- Editor Queue tab replaces the passive Adjusters tab for Editor users.
- Configurable page size replaces hardcoded pagination.
- KPI summary cards at the top of the dashboard (new: total active cases, cases due within 3 days, cases overdue, cases pending editor review).
- Amendment notification banner displayed to Adjusters when an Editor has requested amendments on one of their cases.

---

### 4.2 Case registration and assignment

#### Purpose

Provide a structured, auditable process for registering new cases and formally assigning them to an adjuster within the system, replacing the current manual pre-registration verbal assignment.

#### Key features

**New Registration form:**
- Fields: case type, vehicle number, insurance reference number, branch, assign date, insurer details (company, contact, policy number, claim number).
- Auto-generated case reference number using configurable prefix and length from DocumentSettings per OU.
- Duplicate detection on vehicle number and insurer reference combination.
- On save, case is created with status **New** (new in v2) pending adjuster assignment.

**Adjuster Assignment screen (new in v2):**
- Displays all cases with status New awaiting assignment.
- CEO/Manager/Ops Mgr selects an adjuster from the active adjuster list and assigns.
- On assignment, case status transitions to **Under Investigation**.
- In-system notification dispatched to Support Staff confirming registration can proceed to full data entry.
- In-system notification dispatched to the assigned Adjuster.

**Duplicate Case Registration:**
- Dedicated form linking a new case to an existing case file number.
- Separate listing screen for duplicate cases.

**List of Case File Records (In Processing):**
- Paginated list of registered cases with search and filter.
- Filters: case file number, vehicle number, insured name, IC number, policy number, insurance reference, date range, status, adjuster, branch, case type.

**Case reassignment:**
- ReassignCaseAdjuster: move a case to a different adjuster with audit entry.
- ReassignCompany: move a case to a different insurance company with audit entry.

#### User stories

- As Support Staff, I want to register a new case immediately upon receiving an assignment letter so that no time is lost waiting for verbal confirmation.
- As a CEO/Manager, I want to assign an adjuster to a newly registered case via the system so that the assignment is recorded and the adjuster is notified automatically.
- As an Adjuster, I want to receive an in-system notification when a case is assigned to me so that I can begin work without waiting for a phone call.
- As Support Staff, I want the system to warn me if I am registering a case that appears to be a duplicate so that I do not create duplicate records.

#### Business rules

- Case reference number: prefix and zero-padded length are read from DocumentSettings for the current OU. The prefix must be unique across all tenants. Example format: `JA-001234`.
- Duplicate detection criteria: same vehicle number AND same insurer reference number. A warning is displayed; the user may override and proceed.
- A case may not be assigned to an inactive adjuster.
- The New status is an internal staging state; it is not visible to third-party portal users.
- Case status must follow the defined sequence: New → Under Investigation → Pending Invoice → Completed Invoices. Transitions may not be skipped except by Superadmin override.
- Cancellation can be applied from any status and does not follow the sequence.
- All field-level changes are recorded in the audit trail.

#### Acceptance criteria

- A new case is saved with a correctly formatted, unique case reference number.
- Duplicate detection fires and displays a warning before the user can save.
- On adjuster assignment, the case status changes from New to Under Investigation within five seconds.
- The assigned adjuster receives an in-system notification visible in the header notifications component.
- Support Staff receive an in-system notification confirming the assignment.
- An audit trail entry is created for every status transition, recording the actor, old status and new status.

#### v2 enhancements over v1

- New **status: New** introduced before Under Investigation, formally capturing the period between registration and adjuster assignment.
- In-system adjuster assignment screen replaces verbal/manual pre-registration assignment.
- In-system notifications to adjuster and Support Staff on assignment (previously absent).
- Support Staff can register cases immediately without waiting for verbal instruction from management.

---

### 4.3 Case management — investigation sub-tabs

#### Purpose

Provide the Adjuster with a structured, document-rich workspace for conducting and recording the full motor claims investigation.

#### Key features

The following sub-tabs appear within the case detail view under the Investigation module.

**Adjusters sub-tab:**
- Scope of assignment selection.
- Completion date and extension remarks.
- Editor assignment (new in v2): Adjuster can formally assign the case to an Editor from this sub-tab using a "Send to Editor" action.

**Insured Owner sub-tab:**
- Upload: owner IC front/back, owner driving licence front/back, hospital detail file.
- Identity number recorded and cross-validated against police report data.

**Insured Driver sub-tab:**
- Same uploads as Insured Owner sub-tab.
- IsOwner flag distinguishes driver-as-owner from separate driver records.

**Insurer sub-tab:**
- Insurance company contact and adjuster contact record for the case.

**Lawyers sub-tab:**
- Law firm assignment, hearing date.
- ViewThirdPartyCases record created automatically on assignment if AllowToViewAssignedCases is approved for the firm.

**Workshop sub-tab:**
- Workshop assignment.
- Same third-party visibility record creation as Lawyers sub-tab.

**Incident Details sub-tab:**
- Narrative circumstance description.
- Circumstances file upload.

**Police Report sub-tab:**
- Police report number, date, police station.
- Report file upload.

**Third Party Vehicle Details sub-tab:**
- Third-party vehicle make, model, registration number, damage description.

**Third Party Personal Details sub-tab:**
- Third-party injury records.
- 8 typed document folders auto-created on first save (folder types: THP-EMP, THP-DET, THP-DL-DET-B and 5 additional typed folders as defined in the v1 seeding logic).
- IC number cross-validated against police report data.
- Fields: injuries sustained, current disabilities, date range (server-side validation enforced).
- Folder renamed automatically if identity number is subsequently changed.

**Claims/Expenses sub-tab:**
- Mileage (unit rate sourced from InsuranceCompany.ClaimRate at time of edit).
- Tolls, airfare, hotel, police fees, fraud amounts, miscellaneous.
- Total auto-calculated server-side.

**Search Fees sub-tab:**
- Search fee charges billed during investigation.
- Auto-aggregated into the claim total.

**Declaration Answers sub-tab:**
- Answers to 10 configurable declaration questions seeded on OU creation.
- Batch save of all answers.

**Remarks sub-tab:**
- Case notes and comments with creator name and timestamp.
- Ordered newest-first.
- Page size configurable (default 10, configurable per user; v1 hard-coded this at 5).

**File Organiser sub-tab:**
- Hierarchical folder-and-file structure per case.
- Upload, rename, delete files and folders.
- GUID-referenced file retrieval.
- Folder auto-created and renamed on identity number change.

#### User stories

- As an Adjuster, I want to upload IC card images and have the system extract data via OCR so that I do not need to type identity information manually.
- As an Adjuster, I want the system to warn me if the IC number I have entered does not match the police report data so that I can resolve discrepancies before submitting.
- As an Adjuster, I want to enter mileage and have the system apply the correct insurer rate automatically so that my expense claim is calculated correctly.
- As an Adjuster, I want to formally send the case to an Editor via the system so that the Editor receives a notification and there is an audit record of the handover.
- As an Editor, I want to see all documents and case details for cases assigned to me so that I can review the investigation thoroughly.

#### Business rules

- Mileage unit rate is sourced from InsuranceCompany.ClaimRate at the time the expense record is edited, not at submission time.
- Claim total is calculated server-side by summing all expense fields. The total field is not directly editable by the user.
- Search fees are auto-aggregated into the claim total.
- 8 typed document sub-folders are created for each CaseThirdPartyInfo record on first save.
- If a CaseThirdPartyInfo identity number is changed, all 8 sub-folders are renamed to reflect the new identity number.
- Date range fields on CaseThirdPartyInfo are validated server-side: end date must be after start date.
- IC cross-validation returns a nullable boolean warning to the front end. In v1 this was a warning only; in v2 it remains a warning but the discrepancy is recorded in the audit trail and flagged on the case summary.
- Declaration question answers are saved as a batch; partial saves are not permitted.
- The Remarks page size defaults to 10 and is configurable by the user. The v1 hard-coded value of 5 is removed.
- The "Send to Editor" action on the Adjusters sub-tab is only available when the case is in Under Investigation status and a report has been generated.
- An Adjuster may not send a case to Editor if no DOCX report exists in the file organiser.

#### Acceptance criteria

- OCR is triggered on document upload and extracted data is pre-populated into the relevant form fields for user confirmation.
- The IC cross-validation warning is displayed within two seconds of saving if a discrepancy is detected, and the discrepancy is recorded in the audit trail.
- The total on the Claims/Expenses sub-tab reflects all line items within one second of saving any individual field.
- Selecting "Send to Editor" opens an editor selection dialog, dispatches a notification to the selected Editor and creates an audit trail entry recording the handover.
- 8 typed sub-folders are visible in the File Organiser immediately after saving the first CaseThirdPartyInfo record.
- Remarks display in newest-first order with the correct creator name and timestamp.

#### v2 enhancements over v1

- "Send to Editor" action on Adjusters sub-tab (formalises Gap 2 resolution).
- Remarks page size configurable; removes v1 hard-coded limit of 5.
- IC cross-validation discrepancy recorded in audit trail (previously a transient front-end warning only).
- Report-must-exist guard before "Send to Editor" action is available.

---

### 4.4 Editor workflow

#### Purpose

Give Editors a dedicated, in-system workflow for reviewing investigation reports, requesting amendments and approving cases for progression, replacing the current verbal/manual communication channel.

#### Key features

**Editor Queue (dashboard tab):**
- Shows all cases assigned to the logged-in Editor.
- Columns: case file number, case type, insured vehicle, assigned adjuster, assignment date, status (Awaiting Review, Amendment Requested, Approved), aging since assignment.
- Action buttons: Open Case, Download Report.

**Editor case view:**
- Read-only access to all investigation sub-tabs.
- Download button for the DOCX report from the file organiser.
- Re-upload button for the amended report.

**Approve action:**
- Editor clicks Approve on the case.
- Case status transitions to Pending Invoice.
- In-system notification dispatched to Support Staff that the case is ready for invoicing.
- Audit trail entry created: actor, decision, timestamp.

**Request Amendment action:**
- Editor clicks Request Amendment and enters amendment notes in a required text field.
- Case status transitions to Amendment Required (new status in v2).
- In-system notification dispatched to the Adjuster with amendment notes.
- Amendment task appears in the Adjuster's dashboard under a new "Requires Attention" banner.
- Audit trail entry created: actor, decision, amendment notes, timestamp.

**Adjuster amendment response:**
- Adjuster views amendment notes, makes corrections, re-generates report if needed.
- Adjuster clicks "Resubmit to Editor" when corrections are complete.
- Case status transitions back to Awaiting Editor Review.
- In-system notification dispatched to the Editor.
- Audit trail entry created.

**Editor override submit:**
- If the Editor approves, they may directly progress the case to Pending Invoice without requiring the Adjuster to take any further action (resolves Gap 3).

#### User stories

- As an Editor, I want to receive an in-system notification when a case is assigned to me for review so that I do not miss any assignments.
- As an Editor, I want to request amendments with written notes that are delivered to the Adjuster in-system so that there is a clear, auditable record of what changes were required.
- As an Editor, I want to approve a case and have it move to Pending Invoice automatically so that I do not need to contact Support Staff manually.
- As an Adjuster, I want to see a clear notification and task in my dashboard when the Editor has requested amendments so that I can respond promptly.
- As a CEO/Manager, I want to see the amendment history for any case so that I can assess quality issues and adjuster performance.

#### Business rules

- A case can only be assigned to an Editor by an Adjuster (via "Send to Editor") or by OU Admin/CEO/Manager.
- A case must be in Under Investigation status (after report generation) before it can be assigned to an Editor.
- The Editor may not edit investigation data; the Editor role is read-only across all investigation sub-tabs except file upload (re-upload of amended report).
- Amendment notes are mandatory when requesting an amendment; a maximum of 2000 characters.
- A case may go through multiple amendment cycles. All cycles are recorded in the audit trail.
- Only the Editor assigned to a case may approve or request amendments on that case. OU Admin/CEO/Manager may override.
- On Editor Approval, the case status transitions directly to Pending Invoice. The Adjuster does not need to take any further action.
- The Amendment Required status is not visible to third-party portal users. Third-party users see the case as Under Investigation until it reaches Pending Invoice.

#### Acceptance criteria

- The Editor Queue tab is visible and populated correctly for users with the Editor role.
- Selecting Request Amendment without entering amendment notes displays a validation error.
- On Approve, the case status changes to Pending Invoice within five seconds and Support Staff receive a notification.
- On Request Amendment, the Adjuster receives a notification containing the amendment notes within five seconds.
- The amendment notes and all decisions are visible in the Status History panel.
- The case status visible to third-party portal users does not expose the Amendment Required status.

#### v2 enhancements over v1

- This entire module is new in v2. It resolves Gaps 2 and 3 from the documented gap analysis. There is no equivalent functionality in v1.

---

### 4.5 Invoicing

#### Purpose

Enable Support Staff and Accounting to generate, manage and deliver financial documents (invoices, debit notes and credit notes) for completed cases.

#### Key features

**Invoice Generator:**
- Triggered from the Pending Invoices tab.
- Line items: mileage, tolls, airfare, hotel, police fees, fraud amounts, miscellaneous, photograph charge, service fee.
- Photograph charge defaults to the value in appsettings but is overridden by InsuranceCompany.PhotographCharge if set.
- Total amount table.
- Invoice reference number auto-generated using configurable prefix and length from DocumentSettings.
- Invoice preview and print.
- On finalisation, case status transitions to Completed Invoices.

**Debit Note Generator:**
- Available from the Completed Invoices tab.
- Adds charges post-invoice.
- Debit note reference number auto-generated using configurable prefix from DocumentSettings.
- Preview and print.

**Credit Note Generator:**
- Available from the Completed Invoices tab.
- Reduces charges post-invoice.
- Credit note reference number auto-generated using configurable prefix from DocumentSettings.
- Preview and print.

**Batch invoice generation (new in v2):**
- Support Staff may select multiple Pending Invoice cases and generate invoices in batch.
- Each invoice is generated individually with its own reference number; the batch action is a UX convenience.

**Direct email delivery (new in v2):**
- After generating an invoice, the user may click "Send to Insurer" to email the invoice PDF directly to the insurer's contact email on record.
- The email includes the invoice as a PDF attachment.
- Delivery is logged with timestamp and recipient address.

#### User stories

- As Support Staff, I want to generate an invoice for a completed case so that we can bill the insurer without manually creating a document.
- As Support Staff, I want to preview the invoice before finalising it so that I can check for errors.
- As Support Staff, I want to email the invoice directly to the insurer from the system so that I do not need to download and send it manually.
- As Accounting, I want to issue a credit note when a charge adjustment is needed so that the insurer's account is corrected.
- As Support Staff, I want to generate invoices for multiple cases at once so that end-of-month invoicing is faster.

#### Business rules

- An invoice can only be generated for a case in Pending Invoice status.
- The invoice reference number uses a zero-padded sequential integer with the OU-configured prefix and length. The sequence is per OU and never resets.
- PhotographCharge is read from InsuranceCompany.PhotographCharge at invoice generation time. If that field is null or zero, the appsettings default is used.
- If the adjuster user no longer exists in the system at invoice generation time, the system raises a user-friendly exception and does not generate the invoice. (This matches v1 behaviour.)
- Debit notes may only be created after an invoice exists for the case.
- Credit notes may only be created after an invoice exists for the case.
- Debit and credit notes are for charge adjustments only; they do not change the case status.
- Direct email delivery requires a valid email address on the InsuranceCompany record. If no email is recorded, the Send to Insurer button is disabled and a tooltip explains why.
- In v2, Support Staff no longer need to update the editor record before invoice generation, as the editor is formally assigned via the system (resolves Gap 4).

#### Acceptance criteria

- Invoices are generated with correctly formatted, unique reference numbers.
- PhotographCharge uses the InsuranceCompany value when set and falls back to appsettings when not.
- The invoice preview renders all line items correctly before finalisation.
- On finalisation, the case status changes to Completed Invoices immediately.
- The Send to Insurer action sends the email and logs the delivery with timestamp within 30 seconds.
- Batch invoice generation produces one invoice per selected case, each with a unique reference number, within 60 seconds for up to 20 cases.

#### v2 enhancements over v1

- Batch invoice generation for multiple cases.
- Direct email delivery of invoice to insurer from within the system.
- Gap 4 resolved: editor record update step before invoicing is no longer required.

---

### 4.6 Claims and expense management

#### Purpose

Provide Adjusters with structured expense entry and give Accounting and management the tools to review, approve or reject expense claims and update payment status.

#### Key features

**Adjuster expense entry:**
- Fields: mileage (auto-rated), tolls, airfare, hotel, police fees, fraud amounts, miscellaneous.
- Search fees entered separately on the Search Fees sub-tab; auto-aggregated into total.
- Total calculated server-side.

**Expense/Claims Status view:**
- List of expense and claim records by case.
- Per-item status: pending, approved, rejected.
- Bulk approve/reject for Accounting and supervisor roles.

**Expense Report:**
- Aggregated expense report across cases.
- Filterable by adjuster, date range, expense type.
- Excel export.

**Payment Status update:**
- Accounting updates payment status after processing: in process, completed, pending.

#### User stories

- As an Adjuster, I want to record all field expenses against a case so that I am reimbursed accurately.
- As Accounting, I want to bulk-approve a list of expense claims so that I can process end-of-month payments efficiently.
- As Accounting, I want to update the payment status of a processed claim so that records reflect the actual payment state.
- As a CEO/Manager, I want to export an expense report to Excel so that I can analyse costs by adjuster or period.

#### Business rules

- Mileage unit rate is sourced from InsuranceCompany.ClaimRate at the time of edit.
- The total field is calculated server-side and is not directly editable.
- Bulk approve/reject is available to Accounting and CEO/Manager/Ops Mgr roles.
- Payment status values: Pending, In Process, Completed.
- A rejected expense is excluded from invoice calculations.
- An approved expense cannot be modified by the Adjuster after approval.

#### Acceptance criteria

- Mileage total updates correctly when ClaimRate and mileage quantity are saved.
- Bulk approve/reject applies the selected status to all checked records in a single server call.
- The expense report Excel export contains accurate totals matching the on-screen view.
- A rejected expense item is excluded from the invoice line items.

#### v2 enhancements over v1

- No major structural changes. Improvements are limited to the configurable page size fix and bug fixes identified during v1 code review.

---

### 4.7 Reports

#### Purpose

Provide operational and financial reporting for management, Support Staff and Accounting, with Excel export capability.

#### Key features

**WIP Report:**
- Work-in-progress open cases.
- Columns: adjuster, case file number, vehicle, insurer reference, lawyer reference, assign date, due date, aging days.
- Due date: AssignTime plus the configurable due date threshold (v2; was hardcoded at 14 days in v1).
- Aging days computed dynamically at query time.
- Excludes statuses Completed Invoices (4) and Cancelled (5).
- Filterable by adjuster, group, company and date range.
- Excel export.

**WIP Summary Report:**
- Aggregated WIP summary by adjuster, group or insurer.

**Payment Update Report:**
- Tracks payment status updates across cases.

**Invoice Report:**
- Monthly cross-tabulation of invoiced cases by insurer, case type or state.
- Filters to Completed Invoices status only.
- Excel export.

**Case Report:**
- Monthly cross-tabulation of case counts by insurer, case type or state.
- Excel export.

**Adjuster Report:**
- Completed-invoice cases per adjuster.
- Columns: case file number, vehicle, insurer reference, service fee.
- Filterable per adjuster.
- Excel export.

**Payment Pending Report:**
- Cases with outstanding payment status.

**Invoice-Adhoc:**
- Ad hoc invoice generation outside the standard invoice flow.

**InvoiceEditor-Adhoc:**
- Ad hoc invoice editing.

**Compliance Report:**
- Compliance-oriented report with fields defined by regulatory requirements.

**Adjuster Performance Report (new in v2):**
- Per-adjuster KPIs: average case turnaround days, cases completed per month, amendment rate (cases returned by Editor), on-time completion rate (cases closed before due date).
- Filterable by adjuster, date range and case type.
- Excel export.

**Financial Dashboard (new in v2):**
- Accounts receivable aging: invoiced cases grouped by 0-30, 31-60, 61-90, 90-plus days outstanding.
- Cash flow by period: monthly invoiced amounts vs payments received.
- Unpaid invoices: list of invoices with no recorded payment.
- Accessible to Accounting and CEO/Manager roles.

**Scheduled report delivery (new in v2):**
- Administrators can configure a schedule (e.g., daily at 07:00) to email the WIP report as a PDF to nominated recipients.
- Scheduling is managed via the Admin module using Hangfire background jobs.

#### User stories

- As a CEO/Manager, I want to view the WIP report filtered by adjuster so that I can assess individual workload.
- As Accounting, I want a financial dashboard showing accounts receivable aging so that I can prioritise collections.
- As a CEO/Manager, I want to receive the WIP report by email each morning so that I have an up-to-date workload view without logging in.
- As Support Staff, I want to export the Invoice Report to Excel so that I can share it with the finance team.

#### Business rules

- The WIP due date threshold is configurable per case type and per insurer in v2. If neither is set, the system falls back to the OU-level default (which defaults to 14 days for backward compatibility).
- Report query logic for screen display and Excel export must share the same underlying query method. Duplicate query implementations identified in v1 must be consolidated.
- Scheduled report delivery uses Hangfire; failure is logged and an alert is sent to the OU Admin.
- Financial dashboard data is refreshed on page load and does not use cached results older than one hour.

#### Acceptance criteria

- The WIP report due date reflects the configured threshold for the relevant case type or insurer, not a hardcoded value.
- The on-screen WIP report and the Excel export contain identical data for the same filter criteria.
- The Adjuster Performance Report calculates amendment rate correctly as (cases returned by Editor / total cases submitted) per adjuster per period.
- The financial dashboard accounts receivable aging buckets are correct and update on page refresh.
- Scheduled report email delivery succeeds and is logged; a Hangfire job failure creates an alert visible in the Admin module.

#### v2 enhancements over v1

- Configurable WIP due date threshold replaces hardcoded 14-day rule.
- Consolidation of duplicate GetAll/GetToExcel query logic (tech debt fix).
- Adjuster Performance Report is new.
- Financial Dashboard is new.
- Scheduled report delivery is new.

---

### 4.8 OCR integration

#### Purpose

Automate data extraction from identity documents, driving licences, police reports and insurance documents using the OpenAI Vision API, reducing manual data entry by Adjusters.

#### Key features

- File upload on any document field triggers an OCR call using the named prompt template associated with that document type.
- Extracted data is returned to the front end for user review and confirmation before saving.
- OCR component: document digitisation UI with side-by-side image and extracted data fields.
- Integration logs: paginated view of all OpenAI API calls with PromptToken, CompletionToken and TotalCost per call, linked to case via CaseNo.
- Prompt management (admin): CRUD for OCR prompt templates (v2 enables prompt creation; v1 blocked this).
- OCR result correction workflow (new in v2): user reviews each extracted field and confirms or corrects before data is committed to the case record.

#### User stories

- As an Adjuster, I want to photograph an IC card and have the system extract the identity number, name and address automatically so that I do not need to type them.
- As a Superadmin, I want to create a new OCR prompt template for a new document type so that OCR can be extended without a code deployment.
- As a CEO/Manager, I want to see the cost of each OCR call in the integration logs so that I can monitor AI usage costs.

#### Business rules

- Prompt templates are host-level records; they are shared across all tenants. The MayHaveTenant filter is disabled for prompt queries.
- In v2, prompt creation via the Admin UI is enabled. Previously the create endpoint was commented out.
- Each OCR call logs: CaseNo, PromptToken, CompletionToken, TotalCost and CreationTime.
- The user must confirm or correct extracted fields before the data is written to the case record. Unconfirmed data is discarded.
- If the OpenAI API call fails, the error is surfaced to the user and the failure is logged. The user may retry or enter data manually.

#### Acceptance criteria

- OCR extracts identity number, name and date of birth from an IC card image with accuracy above 90% under normal lighting conditions.
- Extracted data is displayed in the correction workflow UI within five seconds of upload.
- A new prompt template created via the Admin UI is immediately available for use in OCR calls without a deployment.
- Integration log entries appear within 10 seconds of a completed OCR call and display accurate token counts and cost.

#### v2 enhancements over v1

- OCR prompt creation enabled via Admin UI (v1 had this endpoint commented out).
- OCR result correction workflow (user review before save).
- Retry on failure with user feedback.

---

### 4.9 Administration

#### Purpose

Provide OU administrators and platform superadmins with the configuration and master data management tools required to operate the platform.

#### Key features

**Branch Details**: CRUD for branches; geographic grouping for adjusters.
**Group Details**: CRUD for adjuster groups linked to branches.
**Staff Details**: CRUD for internal staff; group membership and role assignment.
**Insurance Company Details**: CRUD for insurer master; ClaimRate, PhotographCharge, AllowToViewAssignedCases flag; contact details including email for invoice delivery.
**Lawyer Company Details**: CRUD for law firm master; AllowToViewAssignedCases workflow; contact details.
**Workshop Company Details**: CRUD for workshop master; AllowToViewAssignedCases workflow; contact details.
**CaseType Details**: CRUD for case types; active/inactive flag; configurable due date threshold (new in v2).
**Scope of Work Details**: CRUD for adjuster scope assignments.
**Vehicle Details**: Vehicle make, model and specifications; cascading dropdowns.
**Manage Users**: User creation and edit; role assignment; OU/branch/group membership.
**Data Changes (Audit Trails)**: Query and Excel export of business-level audit trail with field-level change history.
**OpenAI Integration Logs**: Paginated view of API calls with cost breakdown; filterable by case and date range.
**Prompts**: Full CRUD for OCR prompt templates (create enabled in v2).
**Declaration Questions**: CRUD for configurable declaration questions; 10 seeded on OU creation.
**Lookups**: Key-value management for all categorised dropdown lists.
**Scheduled Reports (new in v2)**: Configure email delivery schedule for WIP and other reports; Hangfire job management.
**WIP Due Date Settings (new in v2)**: Configure default due date threshold at OU level, with optional per-case-type and per-insurer overrides.
**Third-party Access Requests**: Review and approve/reject ViewThirdPartyCaseRequest records.
**Organisation Unit onboarding**: Create and configure new OUs with DocumentSettings.

#### User stories

- As an OU Admin, I want to configure the WIP due date threshold for a specific case type so that the WIP report reflects our agreed SLA for that type.
- As a Superadmin, I want to create a new OCR prompt template so that the system can process a new document type without requiring a developer.
- As an OU Admin, I want to approve a third-party access request so that the insurer can view their assigned cases via the portal.
- As an OU Admin, I want to schedule the WIP report to be emailed to me each morning so that I have daily visibility without logging in.

#### Business rules

- DocumentSettings: one record per OU. BusinessRegistrationNo and CaseRefNoPrefix must be unique across all tenants.
- Third-party access approval: approving a ViewThirdPartyCaseRequest triggers a bulk insert of ViewThirdPartyCases records for all existing assigned cases and updates AssignOUId on the master entity.
- Cancellation of third-party access: deletes ViewThirdPartyCases records, nullifies AssignOUId and requires a CancelRemark.
- Declaration questions: 10 seeded automatically on OU onboarding; configurable thereafter.
- All permission decorators (AbpAuthorize) must be present and active on all app service endpoints. The security gap identified in v1 (AbpAuthorize commented out on ViewThirdPartyCaseRequestsAppService) is corrected in v2.

#### Acceptance criteria

- A new OCR prompt template is saved and immediately usable without a deployment.
- Approving a third-party access request inserts the correct ViewThirdPartyCases records for all existing assigned cases within 10 seconds.
- The WIP Due Date Settings screen saves per-case-type and per-insurer overrides correctly and these values are reflected in the WIP report.
- All admin CRUD operations produce audit trail entries.
- AbpAuthorize is enforced on all app service endpoints; unauthorised calls return HTTP 403.

#### v2 enhancements over v1

- OCR prompt creation enabled.
- WIP Due Date Settings screen (new).
- Scheduled Reports configuration screen (new).
- Security fix: AbpAuthorize restored on ViewThirdPartyCaseRequestsAppService.
- Traccar, JPJ Search and Commercial Value Check menu items removed from Admin (per client request).

---

### 4.10 Third-party portal

#### Purpose

Provide read-only case visibility to instructing insurers, engaged law firms and engaged workshops via a dedicated portal, controlled by an administrator-approved access workflow.

#### Key features

- Dashboard3rdParty component: role-appropriate portal landing page.
- Cases visible are limited to those linked via ViewThirdPartyCases (join table managed by the AllowToViewAssignedCases workflow).
- Read-only access to case details, Adjusters sub-tab, Insurers sub-tab, Lawyers sub-tab and Workshops sub-tab.
- Case status displayed excludes internal statuses New and Amendment Required; third-party users see Under Investigation until Pending Invoice.
- Real-time status updates via SignalR when a case the third party can view changes status.

#### User stories

- As an Insurer, I want to log into the portal and see the current status of all cases I have assigned to this adjusting firm so that I have visibility without making phone calls.
- As an OU Admin, I want to control which cases an insurer can view by approving their access request so that confidential information is not shared without authorisation.

#### Business rules

- A third-party user may only view cases linked to their OU via ViewThirdPartyCases.
- Access is granted by the AllowToViewAssignedCases approval workflow. Pending requests do not grant access.
- The Amendment Required status is not exposed to third-party users.
- The New status is not exposed to third-party users.
- Third-party users cannot create, edit or delete any records.

#### Acceptance criteria

- A third-party user logging in sees only their assigned cases.
- A third-party user cannot access the URL of a case not assigned to their organisation.
- Status changes are reflected in the portal within five seconds via SignalR.

#### v2 enhancements over v1

- Internal statuses (New, Amendment Required) masked from third-party users.
- Real-time SignalR status push confirmed and documented (was implied in v1 but not formally specified).

---

### 4.11 Platform and multi-tenancy

#### Purpose

Provide the host-level platform infrastructure for multi-tenant SaaS operation, including tenant management, subscription billing, security and real-time communication.

#### Key features

- Tenant management: create, edit, activate/deactivate.
- Edition management: subscription tiers.
- Subscription lifecycle: trial, upgrade, extend, trial-to-paid.
- Payment gateways: Stripe and PayPal for SaaS subscription billing.
- Host settings: global platform configuration.
- Language management: multi-language UI support (English and Bahasa Malaysia in v2).
- ABP system audit logs (separate from custom business audit trails).
- Login attempt monitoring.
- Webhook subscriptions.
- Two-factor authentication via Twilio SMS.
- Social login: Facebook, Google, Twitter/X, Microsoft Azure AD, OpenID Connect, WS-Federation.
- Session lock screen and timeout.
- User delegation.
- Profile management: my settings, change password, change email.
- Mass notifications and version release notifications.
- SignalR real-time chat with friend list, message history and mark-as-read.
- Chat bar component (persistent UI element).
- Header notifications component.

#### v2 enhancements over v1

- Branding updates: correct logo, secondary digital logo, favicon, page title ("SwiftProAi"), copyright year (2024), footer URL, help icon URL (all per client request list).
- White space removed from top banner on Dashboard and Change Password pages.
- "Permission Token Detail" downloadable document updated with correct content and branding.
- Legacy menu items removed: Traccar (Admin), JPJ Search Request (case tab) and Commercial Value Check (case tab) as confirmed by client request.
- Bahasa Malaysia language pack added (partial; see Section 8.4).

---

## 5. Workflows

### 5.1 Case intake workflow

This workflow covers the period from when an insurer sends an assignment case to when the Adjuster begins field investigation.

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Insurance Company | Sends assignment case via Merimen, email, fax or portal | Insurer decides to instruct an adjusting company | Assignment received by adjusting company | None (external) |
| 2 | CEO/Manager / Ops Mgr | Reviews assignment. In v2 with Merimen integration: assignment data is auto-imported. Without integration: reviews email/fax and opens New Registration manually | Assignment received | Case registered in system | None |
| 3 | Support Staff | Opens New Registration form. Enters case type, vehicle number, insurer details, branch, assign date | CEO/Manager has initiated registration (or Merimen auto-import has created draft) | Case created with status New; unique case reference number generated | None |
| 4 | CEO/Manager / Ops Mgr | Opens Adjuster Assignment screen. Selects adjuster. Saves assignment | Case in status New | Case status transitions to Under Investigation | In-system notification to assigned Adjuster; in-system notification to Support Staff |
| 5 | Support Staff | Updates remaining case fields from assignment letter (policy number, claim number, insurer contact, incident date etc.) | Case in Under Investigation | Case data complete enough for handover to Adjuster | None |
| 6 | Adjuster | Views dashboard; opens newly assigned case; begins populating investigation sub-tabs | Case visible in Adjuster's dashboard | Investigation in progress | None |

**Decision points:**
- At Step 3, if duplicate detection fires (same vehicle number and insurer reference), Support Staff must confirm intent before proceeding.
- At Step 4, if the selected adjuster is inactive, the assignment is blocked and an error is displayed.

---

### 5.2 Investigation workflow

This workflow covers the Adjuster's field investigation and the handover to the Editor.

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Adjuster | Uploads identity documents; runs OCR; confirms or corrects extracted data | Case in Under Investigation | Identity data recorded | None |
| 2 | Adjuster | Completes insured owner and insured driver sub-tabs; records insurer contact; records law firm/workshop if applicable | Step 1 complete | All known parties recorded | Third-party access records created if AllowToViewAssignedCases is approved |
| 3 | Adjuster | Records incident details and police report | Step 2 complete | Narrative and police report on file | None |
| 4 | Adjuster | Records third-party vehicle and personal details; 8 typed folders auto-created for each third party | Step 3 complete | All third-party records saved | None |
| 5 | Adjuster | Records mileage, tolls and all other expenses; records search fees; answers declaration questions; adds case remarks | Step 4 complete | Expenses and declaration recorded | None |
| 6 | Adjuster | Generates DOCX investigation report | Step 5 complete; all mandatory fields populated | Report appears in file organiser | None |
| 7 | Adjuster | Selects Editor from the "Send to Editor" action on the Adjusters sub-tab | Report exists in file organiser | Case sub-status: Awaiting Editor Review | In-system notification to selected Editor |

**Decision points:**
- At Step 7, if no report exists in the file organiser, the "Send to Editor" action is disabled. Adjuster must generate the report first.
- At any step, if the IC number entered does not match police report data, a discrepancy warning is displayed and recorded in the audit trail.

---

### 5.3 Editor review and approval workflow

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Editor | Views Editor Queue on dashboard; opens assigned case | Case in Awaiting Editor Review sub-status | Case open for review | None |
| 2 | Editor | Downloads DOCX report; reviews for accuracy, language and completeness | Case open | Review complete | None |
| 3a (Approve path) | Editor | Clicks Approve | Review complete; no amendments required | Case status transitions to Pending Invoice | In-system notification to Support Staff: case ready for invoicing |
| 3b (Amendment path) | Editor | Clicks Request Amendment; enters amendment notes | Review complete; amendments required | Case sub-status transitions to Amendment Required | In-system notification to Adjuster with amendment notes |
| 4 (Amendment path) | Adjuster | Views amendment notes in dashboard banner; makes corrections; re-generates report if needed; clicks Resubmit to Editor | Case in Amendment Required sub-status | Case sub-status transitions back to Awaiting Editor Review | In-system notification to Editor |
| 5 (Amendment path) | Editor | Reviews corrected report; approves or requests further amendments | Case in Awaiting Editor Review (second cycle) | As per Step 3a or 3b | As per Step 3a or 3b |

**Decision points:**
- Multiple amendment cycles are permitted. Each cycle is recorded in the audit trail.
- The Editor may approve after any amendment cycle. The Editor may also re-upload an amended report themselves before approving.
- At Step 3a, the Editor approves directly; the Adjuster does not need to take any further action (Gap 3 resolution).

---

### 5.4 Invoicing workflow

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Support Staff | Views Pending Invoices tab; selects case | Case in Pending Invoice status | Invoice Generator opened | None |
| 2 | Support Staff | Reviews line items; adjusts service fee if required; previews invoice | Invoice Generator open | Invoice previewed | None |
| 3 | Support Staff | Finalises invoice | Preview confirmed | Invoice created; invoice reference number assigned; case status transitions to Completed Invoices | None |
| 4 | Support Staff | Optionally sends invoice to insurer via email (new in v2) or downloads/prints for manual delivery | Invoice created | Insurer receives invoice | Email delivery logged if sent via system |
| 5 | Accounting | Updates payment status when payment is received | Invoice delivered | Payment status set to Completed | None |

**Decision points:**
- At Step 1, if the assigned adjuster user no longer exists in the system, invoice generation is blocked with a user-friendly error.
- Step 4 email delivery requires a valid email address on the InsuranceCompany record.
- Debit and credit note generation may occur at any time after Step 3 and do not change the case status.

---

### 5.5 Expense approval workflow

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Adjuster | Submits expense records for a case | Case in Under Investigation | Expenses in Pending status | None |
| 2 | Accounting / CEO/Manager | Views GetAllExpensesApproval list; selects records for bulk action | Expenses in Pending status | Bulk approve or reject applied | None |
| 3 | Accounting | Updates payment status after processing | Expenses approved | Payment status updated to In Process or Completed | None |

**Decision points:**
- Rejected expenses are excluded from invoice calculations.
- Approved expenses are locked for editing by the Adjuster.

---

### 5.6 Third-party access workflow

| Step | Actor | Action | Entry condition | Exit condition | Notifications triggered |
|---|---|---|---|---|---|
| 1 | Insurance Company / Law Firm / Workshop | Requests access to view assigned cases | AllowToViewAssignedCases flag enabled by OU Admin | ViewThirdPartyCaseRequest created with status Pending Approval | None |
| 2 | OU Admin | Reviews request; approves | Request in Pending Approval | AssignOUId set on master entity; ViewThirdPartyCases records bulk-created for all existing assigned cases | None |
| 2b | OU Admin | Reviews request; rejects | Request in Pending Approval | Request remains with rejection reason | None |
| 3 | OU Admin | Cancels access at any time | Access previously approved | ViewThirdPartyCases records deleted; AssignOUId nullified; CancelRemark required | None |

---

## 6. Data requirements

### 6.1 Core case entities

#### MainRegistration

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| RegisterId | int (PK) | Auto | — | System-generated primary key |
| CaseFileNo | string | Auto | Unique per OU; format: prefix + zero-padded integer | Generated from DocumentSettings |
| CaseTypeId | int (FK) | Yes | Must reference active CaseType record | |
| VehicleNo | string(20) | Yes | Non-empty | Malaysian vehicle registration format |
| StatusId | int (FK) | Auto | Valid status code (1-5) | Managed by workflow engine |
| AssignedAdjusterId | long (FK) | Yes (after assignment) | Must reference active user with Adjuster role | Nullable until assignment step |
| EditorUserId | long (FK) | No | Must reference active user with Editor role when set | Nullable until Editor assigned |
| BranchId | int (FK) | Yes | Must reference active Branch | |
| InsuranceCompanyId | int (FK) | Yes | Must reference active Company | |
| AssignTime | DateTime | Yes | Must be a valid date; not in future | |
| InsuranceReference | string(100) | Yes | Non-empty | Insurer's own reference number |
| PolicyNumber | string(100) | No | — | |
| ClaimNumber | string(100) | No | — | |
| TenantId | int | Auto | — | ABP multi-tenancy |
| CreationTime | DateTime | Auto | — | |
| CreatorUserId | long | Auto | — | |

#### CaseAdjuster

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| RegisterId | int (FK) | Yes | References MainRegistration | |
| ScopeAssignmentId | int (FK) | No | References active ScopeAssignment | |
| CompletionDate | DateTime | No | Must be after AssignTime | |
| ExtensionRemarks | string(500) | No | — | |
| EditorUserId | long (FK) | No | References active Editor user | Set via "Send to Editor" action |
| StatusCode | int | Auto | 1-5 | StatusCode 5 triggers parent case status update |

#### InsuredPerson

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| RegisterId | int (FK) | Yes | References MainRegistration | |
| IsOwner | bool | Yes | — | True = insured owner; False = insured driver |
| IdentityNumber | string(50) | Yes | Non-empty; cross-validated against police report | IC number or passport number |
| ICFrontFileGuid | Guid | No | Valid GUID when set | |
| ICBackFileGuid | Guid | No | — | |
| LicenceFrontFileGuid | Guid | No | — | |
| LicenceBackFileGuid | Guid | No | — | |
| HospitalDetailFileGuid | Guid | No | — | |

#### CaseClaim

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| RegisterId | int (FK) | Yes | References MainRegistration | |
| Mileage | decimal(10,2) | No | >= 0 | |
| Tolls | decimal(10,2) | No | >= 0 | |
| Airfare | decimal(10,2) | No | >= 0 | |
| Hotel | decimal(10,2) | No | >= 0 | |
| PoliceFees | decimal(10,2) | No | >= 0 | |
| FraudAmount | decimal(10,2) | No | >= 0 | |
| Miscellaneous | decimal(10,2) | No | >= 0 | |
| TotalAmount | decimal(10,2) | Auto | Calculated server-side | Not directly editable |
| ClaimRate | decimal(10,4) | Auto | Sourced from InsuranceCompany.ClaimRate at edit time | Stored for historical accuracy |

#### CaseInvoice

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| RegisterId | int (FK) | Yes | References MainRegistration | |
| InvoiceRefNo | string | Auto | Unique per OU; sequential | |
| ServiceFee | decimal(10,2) | Yes | >= 0 | |
| PhotographCharge | decimal(10,2) | Auto | From InsuranceCompany or appsettings | |
| TotalAmount | decimal(10,2) | Auto | Calculated server-side | |
| InvoiceDate | DateTime | Auto | — | |
| AdjusterUserId | long (FK) | Yes | Must reference existing user | Exception thrown if user deleted |
| DeliveredByEmail | bool | No | — | True if sent via system email |
| DeliveryTimestamp | DateTime | No | — | Populated on email delivery |

#### DocumentSettings

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| OUId | long (FK) | Yes | One per OU | |
| LegalName | string(200) | Yes | Non-empty | |
| BusinessRegistrationNo | string(50) | Yes | Unique across all tenants | |
| TaxNo | string(50) | No | — | |
| CaseRefNoPrefix | string(10) | Yes | Unique across all tenants; alphanumeric; no spaces | |
| CaseRefNoLength | int | Yes | 4-10 | |
| InvoiceRefNoPrefix | string(10) | Yes | Non-empty | |
| InvoiceRefNoLength | int | Yes | 4-10 | |
| DebitNotePrefix | string(10) | Yes | Non-empty | |
| CreditNotePrefix | string(10) | Yes | Non-empty | |
| CompanyType | enum | Yes | Adjuster, Insurer, Lawyer, Workshop | |
| DefaultWipDueDays | int | Yes | 1-365; default 14 | New in v2 |

#### WipDueDateOverride (new in v2)

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| Id | int (PK) | Auto | — | |
| OUId | long (FK) | Yes | References OU | |
| CaseTypeId | int (FK) | No | Null = applies to all case types | |
| InsuranceCompanyId | int (FK) | No | Null = applies to all insurers | |
| DueDays | int | Yes | 1-365 | |

#### EditorAssignment (new in v2)

| Field | Type | Mandatory | Validation | Notes |
|---|---|---|---|---|
| Id | int (PK) | Auto | — | |
| RegisterId | int (FK) | Yes | References MainRegistration | |
| EditorUserId | long (FK) | Yes | References active Editor user | |
| AssignedByUserId | long (FK) | Yes | Actor who triggered assignment | |
| AssignedAt | DateTime | Auto | — | |
| Status | enum | Auto | AwaitingReview, AmendmentRequested, Approved | |
| AmendmentNotes | string(2000) | Conditional | Required when Status = AmendmentRequested | |
| DecisionAt | DateTime | No | — | Populated on Approve or Request Amendment |

### 6.2 Validation rules summary

| Rule | Scope | Severity |
|---|---|---|
| CaseRefNoPrefix must be unique across all tenants | DocumentSettings.CaseRefNoPrefix | Hard block |
| BusinessRegistrationNo must be unique across all tenants | DocumentSettings.BusinessRegistrationNo | Hard block |
| Vehicle number and insurer reference duplicate check | MainRegistration | Warning (overridable) |
| IC number must match police report IC data | InsuredPerson.IdentityNumber | Warning + audit trail entry |
| Third-party info date range: end date must be after start date | CaseThirdPartyInfo | Hard block |
| Amendment notes required when requesting amendment | EditorAssignment.AmendmentNotes | Hard block |
| Invoice cannot be generated if adjuster user is deleted | CaseInvoice | Hard block |
| "Send to Editor" requires report in file organiser | CaseAdjuster | Hard block |
| Adjuster assignment requires active adjuster user | MainRegistration.AssignedAdjusterId | Hard block |
| All expense fields must be >= 0 | CaseClaim | Hard block |
| WipDueDateOverride.DueDays must be 1-365 | WipDueDateOverride | Hard block |

### 6.3 Mandatory vs optional fields by sub-tab

| Sub-tab | Mandatory fields | Optional fields |
|---|---|---|
| New Registration | CaseType, VehicleNo, InsuranceReference, BranchId, AssignTime | PolicyNumber, ClaimNumber, InsuranceCompanyId (can be set later) |
| Insured Owner | IdentityNumber | All file uploads |
| Insured Driver | IdentityNumber, IsOwner | All file uploads |
| Police Report | PoliceReportNo, ReportDate, PoliceStation | ReportFileGuid |
| Incident Details | NarrativeDescription | CircumstancesFileGuid |
| Third Party Personal | IdentityNumber, InjuriesSustained | CurrentDisabilities, DateRange fields |
| Editor Assignment | EditorUserId | — |
| Request Amendment | AmendmentNotes (max 2000 chars) | — |
| Invoice | ServiceFee | (other fields auto-populated) |
| Debit Note | Amount, Reason | — |
| Credit Note | Amount, Reason | — |

### 6.4 Data retention requirements

- Case records: retained indefinitely or per client contract. No automatic deletion.
- Audit trail records: retained for a minimum of 7 years to support regulatory requirements under the Financial Services Act 2013 (Malaysia).
- OpenAI integration logs: retained for 2 years.
- File organiser documents: retained for the life of the case plus 7 years.
- Login attempt logs: retained for 12 months.
- Deleted users: user records are soft-deleted (ABP IsDeleted flag); historical case and audit records referencing the user ID are preserved.

---

## 7. Integration requirements

### 7.1 Merimen

| Attribute | Detail |
|---|---|
| System name | Merimen |
| Purpose | Malaysia's dominant insurance case portal; used by insurers to dispatch assignment cases to adjusting firms |
| Direction | Inbound (case import) and outbound (report and invoice delivery) |
| Data exchanged (inbound) | Assignment case: insurer reference, policy number, claim number, vehicle number, insured name, incident date, case type, insurer contact |
| Data exchanged (outbound) | Investigation report (PDF/DOCX), invoice (PDF), case status updates |
| Frequency and trigger | Inbound: polled on a configurable schedule (default: every 15 minutes) via Hangfire background job; also triggerable manually. Outbound: triggered by user action (Send to Insurer button) |
| Error handling | If the Merimen API returns an error, the job logs the error, retries up to 3 times with exponential back-off and alerts the OU Admin via in-system notification. Failed imports are queued in a review list for manual processing |
| Authentication | API key per tenant, stored in encrypted OU settings |
| Notes | This is the highest-priority v2 integration. Without it, all inbound case data is manually re-keyed. Merimen API documentation must be obtained from Merimen before integration development begins. See Section 11 (Open Questions) |

### 7.2 OpenAI Vision API (OCR)

| Attribute | Detail |
|---|---|
| System name | OpenAI Vision API |
| Purpose | Extract structured data from document images (IC cards, driving licences, police reports, insurance documents) |
| Direction | Outbound (image and prompt sent); inbound (extracted data returned) |
| Data exchanged | Outbound: image bytes (base64) and named prompt template. Inbound: structured JSON with extracted field values |
| Frequency and trigger | Per document upload; user-triggered |
| Error handling | On API error: surface error to user; log failure; allow manual data entry. On partial extraction: return available fields and flag missing fields for manual completion |
| Authentication | API key stored in host-level encrypted settings |
| Logging | Every call logged: CaseNo, PromptToken, CompletionToken, TotalCost, timestamp |
| Notes | Model selection (GPT-4o or successor) is configurable at host level. Prompt templates are managed in the Admin module. Token cost rates must be updated in configuration when OpenAI pricing changes |

### 7.3 Stripe

| Attribute | Detail |
|---|---|
| System name | Stripe |
| Purpose | SaaS subscription billing for platform tenants |
| Direction | Outbound (payment initiation); inbound (payment confirmation webhook) |
| Data exchanged | Subscription plan, amount, payment method token; confirmation event |
| Frequency and trigger | On subscription creation, upgrade or renewal |
| Error handling | Payment failure displays error to tenant admin; failed webhook delivery is retried by Stripe per its standard retry policy |
| Notes | Used for platform billing only; not for claim settlements |

### 7.4 PayPal

| Attribute | Detail |
|---|---|
| System name | PayPal |
| Purpose | Alternative SaaS subscription billing gateway |
| Direction | Outbound (payment initiation); inbound (confirmation) |
| Data exchanged | Same as Stripe |
| Frequency and trigger | Same as Stripe |
| Error handling | Same as Stripe |
| Notes | Alternative to Stripe; tenant chooses at subscription time |

### 7.5 Twilio SMS

| Attribute | Detail |
|---|---|
| System name | Twilio |
| Purpose | Two-factor authentication code delivery |
| Direction | Outbound |
| Data exchanged | OTP code and recipient mobile number |
| Frequency and trigger | On login when 2FA is enabled for the user |
| Error handling | If SMS delivery fails, user is prompted to retry or use an alternative 2FA method |
| Notes | Platform security feature only; not used for case notifications |

### 7.6 SignalR

| Attribute | Detail |
|---|---|
| System name | SignalR (ASP.NET Core) |
| Purpose | Real-time browser push for chat, case status updates and notifications |
| Direction | Bidirectional (server push to browser; browser sends chat messages) |
| Data exchanged | Chat messages, case status change events, mass notification payloads, new version release notifications, Editor assignment and amendment notifications (new in v2) |
| Frequency and trigger | Event-driven; no polling |
| Error handling | SignalR reconnection is handled by the client library with exponential back-off |
| Notes | Editor assignment notifications and amendment request notifications are added to the SignalR event set in v2 |

### 7.7 Hangfire

| Attribute | Detail |
|---|---|
| System name | Hangfire |
| Purpose | Background job scheduling and execution |
| Direction | Internal |
| Jobs | Merimen inbound poll, scheduled report email delivery, background document generation for large DOCX exports |
| Error handling | Failed jobs are retried per Hangfire's default retry policy; failures logged in Hangfire dashboard accessible to Superadmin |
| Notes | Hangfire dashboard to be secured behind Superadmin role in v2 |

### 7.8 Redis

| Attribute | Detail |
|---|---|
| System name | Redis |
| Purpose | Per-request session cache; external login options cache |
| Direction | Internal |
| Notes | Cache invalidation strategy must be reviewed for financial dashboard data to ensure accuracy (maximum staleness: 1 hour) |

---

## 8. Non-functional requirements

### 8.1 Performance targets

| Metric | Target |
|---|---|
| Dashboard page load (up to 500 active cases) | < 2 seconds |
| Case detail page load (all sub-tabs) | < 2 seconds |
| OCR result returned after document upload | < 5 seconds (95th percentile) |
| Invoice generation (single case) | < 3 seconds |
| Invoice generation (batch of 20 cases) | < 60 seconds |
| Excel report export (up to 1000 rows) | < 10 seconds |
| Merimen inbound poll processing (up to 50 new cases) | < 2 minutes |
| SignalR notification delivery | < 5 seconds from event trigger |
| API response time (general CRUD operations) | < 1 second (95th percentile) |

### 8.2 Availability and uptime

| Metric | Target |
|---|---|
| Platform availability | 99.5% per calendar month |
| Planned maintenance window | Off-peak hours (22:00-02:00 MYT); 48 hours advance notice |
| Recovery Time Objective (RTO) | 4 hours |
| Recovery Point Objective (RPO) | 1 hour |
| Database backups | Daily full backup; hourly transaction log backup |

### 8.3 Security requirements

- All API endpoints must be decorated with appropriate AbpAuthorize permission attributes. The gap identified in v1 (ViewThirdPartyCaseRequestsAppService) is corrected in v2.
- All data in transit encrypted using TLS 1.2 or higher.
- All data at rest encrypted using AES-256 or database-native encryption.
- Passwords stored as salted hashes using ASP.NET Core Identity (BCrypt).
- Two-factor authentication (Twilio SMS) available and recommended for all admin roles.
- Session timeout: 30 minutes of inactivity (configurable per tenant).
- Login attempt monitoring with lockout after 5 failed attempts within 10 minutes.
- File uploads scanned for malware before storage.
- Role-based access control enforced at the API layer, not only the UI layer.
- Personal data handling compliant with the Malaysia Personal Data Protection Act 2010 (PDPA).
- OpenAI API key and other integration credentials stored in encrypted host-level settings; never logged in plain text.
- Hangfire dashboard accessible to Superadmin role only; not accessible to tenant users.
- Audit trail records are immutable; no update or delete operations are permitted on audit trail entries.
- Third-party portal users are restricted at the query layer; URL manipulation must not expose unauthorised case data.

### 8.4 Localisation

- Primary language: English (Australian English for UI copy, consistent with organisation language standards).
- Secondary language: Bahasa Malaysia.
- Language toggle available in the user profile settings.
- Date format: DD/MM/YYYY throughout the application.
- Currency: Malaysian Ringgit (MYR); formatted as RM#,##0.00.
- Bahasa Malaysia translation scope for v2: core navigation, dashboard labels and case status labels. Full UI translation is deferred to a future release. See Section 10.

### 8.5 Accessibility

- Web application to comply with WCAG 2.1 Level AA.
- All form fields to have visible labels and ARIA attributes.
- Keyboard navigation supported throughout the application.
- Colour contrast ratios meet WCAG 2.1 AA minimums.
- Error messages are associated with the triggering form field via ARIA describedby.

### 8.6 Browser compatibility

- Chrome 120 and later.
- Microsoft Edge 120 and later.
- Firefox 120 and later.
- Safari 17 and later (macOS; for third-party portal users who may use any browser).
- Mobile browsers (Chrome and Safari) for the third-party portal read-only view.

### 8.7 Mobile application (new in v2)

A dedicated mobile application for Adjusters is specified as a v2 deliverable. Requirements:

- iOS 16 and later; Android 12 and later.
- Field photo capture with GPS tagging; images uploaded directly to the case file organiser.
- Offline capability: expense entry, remarks and document capture are queued locally and synced when connectivity is restored.
- Push notifications for case assignments, Editor feedback (amendment requests and approvals) and case status changes.
- GPS mileage auto-calculation: Adjuster starts and stops a trip; mileage is calculated from GPS track and pre-populated into the expense field.
- Biometric authentication (Face ID / fingerprint) as an alternative to password entry.
- The mobile application must interact exclusively with the existing SwiftProAI v2 REST API; no separate data store.

---

## 9. v2 new features and enhancements

The following backlog items are derived from the documented gap analysis (System_Requirement_Gap.xlsx), the client request lists and the codebase analysis. Each item is assigned a priority: **High** (must be delivered in v2), **Medium** (targeted for v2; may slip to early v3 if constrained) or **Low** (targeted for v2 but may be deferred).

### 9.1 Workflow gaps (High priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| WF-01 | In-system adjuster assignment | New status (New) before Under Investigation; CEO/Manager assigns adjuster via UI; in-system notifications dispatched | Eliminates manual verbal assignment; creates audit record; removes dependency on phone/email for case initiation | High |
| WF-02 | Editor assignment workflow | "Send to Editor" action on Adjusters sub-tab; Editor Queue dashboard tab; in-system notification to Editor on assignment | Formalises editor handover; gives Editor a managed task queue; removes verbal communication dependency | High |
| WF-03 | Editor approval gate | Editor Approve and Request Amendment actions; Amendment Required case sub-status; in-system notification to Adjuster with amendment notes; Editor can directly progress case to Pending Invoice | Removes the last manual handover in the investigation workflow; creates full decision audit trail; enables Editor-driven progression | High |
| WF-04 | Eliminate pre-invoice editor record update | With formal editor assignment (WF-02), Support Staff no longer need to manually update the editor record before invoicing | Removes unnecessary manual step; reduces invoicing errors | High |

### 9.2 Integration (High priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| INT-01 | Merimen API inbound integration | Auto-import assignment cases from Merimen via scheduled Hangfire job; map Merimen fields to MainRegistration; review queue for failed imports | Eliminates manual re-keying of all inbound case data; estimated to save 10-15 minutes per case | High |
| INT-02 | Merimen API outbound delivery | Submit completed investigation report and invoice to Merimen from within the system | Eliminates manual document upload to Merimen; reduces delivery errors | High |
| INT-03 | Direct email invoice delivery | Send generated invoice as PDF email to insurer directly from the system | Reduces manual steps for Support Staff; provides delivery timestamp for accounts receivable follow-up | High |

### 9.3 Configurability (High priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| CFG-01 | Configurable WIP due date | OU-level default, per-case-type override and per-insurer override for WIP due date threshold; removes hardcoded 14-day rule | Allows each client to reflect their actual SLA commitments in the WIP report | High |
| CFG-02 | OCR prompt creation | Enable create endpoint for Prompt entity in Admin module (was commented out in v1) | Allows new document types to be added without a code deployment | High |
| CFG-03 | Configurable remarks page size | Remove hardcoded page size of 5 on RemarkAppService; expose as user preference (default 10) | Improves usability for cases with many notes | Medium |

### 9.4 Mobile application (High priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| MOB-01 | Adjuster mobile app | iOS and Android app for field use: photo capture with GPS tagging, offline expense entry, document upload, push notifications, GPS mileage auto-calculation | Enables Adjusters to work entirely in the field without a laptop; reduces time between site visit and data entry | High |

### 9.5 Reporting and analytics (Medium priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| RPT-01 | Adjuster Performance Report | Per-adjuster KPIs: average turnaround, cases per month, amendment rate, on-time rate; Excel export | Gives management objective data for performance reviews | Medium |
| RPT-02 | Financial Dashboard | Accounts receivable aging, cash flow by period, unpaid invoices; accessible to Accounting and management | Supports credit control and financial planning | Medium |
| RPT-03 | Scheduled report email delivery | Hangfire-driven scheduled email delivery of WIP report (and other reports) to nominated recipients | Eliminates the need for managers to log in daily for routine reports | Medium |
| RPT-04 | Deduplicate report query logic | Consolidate GetAll and GetToExcel into a single shared query method per report service | Removes risk of screen/export data inconsistency; reduces maintenance cost | Medium |

### 9.6 OCR enhancements (Medium priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| OCR-01 | OCR result correction workflow | User reviews each extracted field before data is committed; correction UI with side-by-side image and fields | Reduces data entry errors caused by over-reliance on OCR output | Medium |
| OCR-02 | OCR retry on failure | If OpenAI API call fails, user is presented with retry option and clear error message | Improves resilience; reduces support calls for OCR failures | Medium |

### 9.7 Security fixes (High priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| SEC-01 | Restore AbpAuthorize on approval endpoint | Re-enable the AbpAuthorize attribute on ViewThirdPartyCaseRequestsAppService (commented out in v1) | Closes a security gap where the approval endpoint is accessible without proper role validation | High |
| SEC-02 | Permission audit | Review all app service endpoints for missing or commented-out AbpAuthorize attributes; remediate any gaps | Ensures consistent authorisation enforcement across all endpoints | High |
| SEC-03 | PDPA compliance review | Review personal data fields, storage, access and retention against PDPA requirements; implement any required changes | Required for legal compliance in Malaysia | High |

### 9.8 Tech debt (Medium priority)

| ID | Feature | Description | Business value | Priority |
|---|---|---|---|---|
| TD-01 | Fix async void in SyncThirdPartyCases | Replace fire-and-forget async void with proper async Task and exception handling | Prevents silent failures in third-party case sync; improves reliability | Medium |
| TD-02 | Normalise IFileOrgsAppService async | Resolve mixed sync/async method signatures | Reduces threading bugs; improves code maintainability | Medium |
| TD-03 | Remove duplicate using statements | Clean up generated code files with duplicate using directives | Reduces compiler warnings; improves code readability | Low |
| TD-04 | Consolidate CaseTypeAppService dual API pattern | Unify old delegate methods and new CRUD methods into a single consistent pattern | Reduces confusion for future developers; removes legacy code path | Low |
| TD-05 | Explicit workflow state machine | Replace status code propagation (CaseAdjuster.StatusCode = 5 triggers parent) with explicit workflow transitions | Improves maintainability and auditability of status changes | Medium |

### 9.9 UX and branding (High priority — client-approved requests)

| ID | Feature | Description | Priority |
|---|---|---|---|
| UX-01 | Logo update | Replace current logo with SwiftProAI secondary digital logo | High |
| UX-02 | Favicon and page title | Update favicon and browser page title to "SwiftProAi" | High |
| UX-03 | Copyright year | Update copyright year to 2024 | High |
| UX-04 | Footer URL | Update footer URL (currently pointing to legacy domain) | High |
| UX-05 | Help icon URL | Update help icon URL (currently pointing to legacy helpdesk URL) | High |
| UX-06 | File Organiser branding | Replace "Snk Market Data Research" text with "Thinkn Insurtech Services Sdn Bhd." | High |
| UX-07 | Top banner white space | Remove excess white space at top of Dashboard and Change Password pages | High |
| UX-08 | Permission Token Detail document | Update content and branding of the downloadable Permission Token Detail document | Medium |
| UX-09 | Remove legacy menu items | Remove Traccar (Admin), JPJ Search Request (case tab) and Commercial Value Check (case tab) | High |

### 9.10 Planned integrations (Low priority — deferred from v1)

| ID | Feature | Description | Priority |
|---|---|---|---|
| INT-04 | JPJ Search integration | Real-time vehicle ownership, summons, road tax and insurance status lookup from Road Transport Department (JPJ) API; was specced in v1 and removed | Low |
| INT-05 | Commercial Value Check | Vehicle commercial value lookup for claims assessment; was specced in v1 and removed | Low |
| INT-06 | Traccar GPS integration | Track adjuster field movements; auto-calculate mileage from GPS track; time-stamp site visits. Dependent on MOB-01 mobile app | Low |

---

## 10. Out of scope for v2

The following items are explicitly deferred from v2:

| Item | Reason for deferral |
|---|---|
| Full Bahasa Malaysia UI translation | Partial translation (navigation, status labels) is in scope. Full translation requires content sign-off from the client and a translation service engagement. Deferred to v3. |
| JPJ Search (Road Transport Department) integration | API access and data agreement with JPJ not yet obtained. Low immediate ROI relative to other integrations. Deferred pending JPJ API availability. |
| Commercial Value Check integration | Dependent on identifying and contracting with a suitable vehicle valuation data provider. Deferred pending commercial evaluation. |
| Traccar GPS integration (server-side) | Low priority. Dependent on mobile app (MOB-01) being delivered first. Deferred to v3 alongside mobile app maturation. |
| Multi-OU per user support | Current architecture assumes one OU per user. Supporting multiple OUs per user requires a significant architectural change to DocumentSettings and role resolution. Deferred to v3. |
| Configurable workflow steps per OU | Allowing tenants to define their own status names and workflow sequences requires a rules engine investment not proportionate for v2. Deferred to v3. |
| Configurable expense approval thresholds | Low usage demand identified in v1. Deferred to v3. |
| Batch debit/credit note generation | Low priority. Manual debit and credit note generation is adequate for current volumes. |
| Document watermarking | Moderate effort; low urgency. Deferred to v3. |
| Bulk ZIP download per case | Low demand identified. Deferred to v3. |
| Social login provider additions | The v1 set (Facebook, Google, Twitter/X, Microsoft, OpenID Connect, WS-Federation) is retained. No new providers in v2. |
| In-system subscription management for tenants | Tenants manage subscriptions via existing Stripe/PayPal flows. No changes required in v2. |

---

## 11. Open questions

The following questions require decisions from the client or technical stakeholders before development can begin on the affected features.

| ID | Question | Affects | Owner | Target resolution date |
|---|---|---|---|---|
| OQ-01 | Has Merimen provided API documentation and sandbox credentials for v2 integration? Without these, INT-01 and INT-02 cannot begin. | INT-01, INT-02 | Client (Thinkn) / Merimen | Before sprint 1 of Merimen integration |
| OQ-02 | What is the exact list of case types to be seeded in Production, UAT and Development environments? (Referenced in client request list item 1.) | CaseType master data | Client (Thinkn) | Before data migration |
| OQ-03 | What is the OU-level default WIP due date to use for existing tenants when CFG-01 is deployed? Should it remain 14 days or be updated? | CFG-01, WIP Report | Client (Thinkn) | Before CFG-01 development |
| OQ-04 | For the Editor approval workflow (WF-03): can a case have more than one Editor assigned simultaneously, or is it strictly one Editor per case? | WF-02, WF-03, EditorAssignment data model | Client (Thinkn) | Before WF-02 development begins |
| OQ-05 | What is the correct updated URL for the Help icon? What content should it point to? | UX-05 | Client (Thinkn) | Before UX sprint |
| OQ-06 | What is the correct updated content for the Permission Token Detail downloadable document? | UX-08 | Client (Thinkn) | Before UX sprint |
| OQ-07 | What is the target mobile platform for MOB-01? React Native (cross-platform) or separate native iOS/Android apps? This decision significantly affects cost and timeline. | MOB-01 | Client (Thinkn) / ABOD | Before mobile sprint planning |
| OQ-08 | Is the PDPA compliance review (SEC-03) to be conducted by ABOD or by an external legal/privacy specialist? Who bears the cost? | SEC-03 | Client (Thinkn) / ABOD | Before development starts |
| OQ-09 | For Merimen outbound delivery (INT-02): which documents should be submitted (report only, invoice only, or both)? In what format (PDF, DOCX, or both)? | INT-02 | Client (Thinkn) / Merimen | Before INT-02 development |
| OQ-10 | What are the retention requirements for case documents in the file organiser? The specification assumes 7 years, but this should be confirmed against client contracts and Malaysian regulatory requirements. | Section 6.4 | Client (Thinkn) / Legal | Before v2 release |
| OQ-11 | Should the Hangfire dashboard be exposed to OU Admins (with limited read-only access) or strictly to Superadmin only? | Section 7.7, Admin module | Client (Thinkn) | Before admin module development |
| OQ-12 | Are there any additional case types or insurer-specific WIP due date thresholds to be pre-configured at launch? | CFG-01, WipDueDateOverride | Client (Thinkn) | Before CFG-01 release |

---

*This document is general information only. Please confirm with the relevant team before use.*

*All AI-assisted content in this document must be reviewed and approved by a human before it is used to guide development decisions.*

---

**Document history**

| Version | Date | Author | Changes |
|---|---|---|---|
| 1.0 Draft | 12/06/2026 | Business Analysis, ABOD Technology Services | Initial draft based on v1 reverse engineering findings, gap analysis and client request lists |

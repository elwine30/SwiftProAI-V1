using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using ThinknInsurTech.Configuration;

namespace ThinknInsurTech.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled, IWebHostEnvironment env)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig, IWebHostEnvironment env)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var casePoliceReportSummaries = pages.CreateChildPermission(AppPermissions.Pages_CasePoliceReportSummaries, L("CasePoliceReportSummaries"));
            casePoliceReportSummaries.CreateChildPermission(AppPermissions.Pages_CasePoliceReportSummaries_Create, L("CreateNewCasePoliceReportSummary"));
            casePoliceReportSummaries.CreateChildPermission(AppPermissions.Pages_CasePoliceReportSummaries_Edit, L("EditCasePoliceReportSummary"));
            casePoliceReportSummaries.CreateChildPermission(AppPermissions.Pages_CasePoliceReportSummaries_Delete, L("DeleteCasePoliceReportSummary"));

            var viewThirdPartyCaseRequests = pages.CreateChildPermission(AppPermissions.Pages_ViewThirdPartyCaseRequests, L("ViewThirdPartyCaseRequests"), multiTenancySides: MultiTenancySides.Tenant);
            viewThirdPartyCaseRequests.CreateChildPermission(AppPermissions.Pages_ViewThirdPartyCaseRequests_Create, L("CreateNewViewThirdPartyCaseRequest"), multiTenancySides: MultiTenancySides.Tenant);
            viewThirdPartyCaseRequests.CreateChildPermission(AppPermissions.Pages_ViewThirdPartyCaseRequests_Edit, L("EditViewThirdPartyCaseRequest"), multiTenancySides: MultiTenancySides.Tenant);
            viewThirdPartyCaseRequests.CreateChildPermission(AppPermissions.Pages_ViewThirdPartyCaseRequests_Delete, L("DeleteViewThirdPartyCaseRequest"), multiTenancySides: MultiTenancySides.Tenant);

            var caseExpenses = pages.CreateChildPermission(AppPermissions.Pages_CaseExpenses, L("CaseExpenses"));
            caseExpenses.CreateChildPermission(AppPermissions.Pages_CaseExpenses_Create, L("CreateNewCaseExpense"));
            caseExpenses.CreateChildPermission(AppPermissions.Pages_CaseExpenses_Edit, L("EditCaseExpense"));
            caseExpenses.CreateChildPermission(AppPermissions.Pages_CaseExpenses_Delete, L("DeleteCaseExpense"));

            var fileOrgs = pages.CreateChildPermission(AppPermissions.Pages_FileOrgs, L("FileOrgs"));
            fileOrgs.CreateChildPermission(AppPermissions.Pages_FileOrgs_Create, L("CreateNewFileOrg"));
            fileOrgs.CreateChildPermission(AppPermissions.Pages_FileOrgs_Edit, L("EditFileOrg"));
            fileOrgs.CreateChildPermission(AppPermissions.Pages_FileOrgs_Delete, L("DeleteFileOrg"));

            var folders = pages.CreateChildPermission(AppPermissions.Pages_Folders, L("Folders"));
            folders.CreateChildPermission(AppPermissions.Pages_Folders_Create, L("CreateNewFolder"));
            folders.CreateChildPermission(AppPermissions.Pages_Folders_Edit, L("EditFolder"));
            folders.CreateChildPermission(AppPermissions.Pages_Folders_Delete, L("DeleteFolder"));

            var caseCreditNotes = pages.CreateChildPermission(AppPermissions.Pages_CaseCreditNotes, L("CaseCreditNotes"));
            caseCreditNotes.CreateChildPermission(AppPermissions.Pages_CaseCreditNotes_Create, L("CreateNewCaseCreditNote"));
            caseCreditNotes.CreateChildPermission(AppPermissions.Pages_CaseCreditNotes_Edit, L("EditCaseCreditNote"));
            caseCreditNotes.CreateChildPermission(AppPermissions.Pages_CaseCreditNotes_Delete, L("DeleteCaseCreditNote"));

            var creditNoteItems = pages.CreateChildPermission(AppPermissions.Pages_CreditNoteItems, L("CreditNoteItems"));
            creditNoteItems.CreateChildPermission(AppPermissions.Pages_CreditNoteItems_Create, L("CreateNewCreditNoteItem"));
            creditNoteItems.CreateChildPermission(AppPermissions.Pages_CreditNoteItems_Edit, L("EditCreditNoteItem"));
            creditNoteItems.CreateChildPermission(AppPermissions.Pages_CreditNoteItems_Delete, L("DeleteCreditNoteItem"));
            creditNoteItems.CreateChildPermission(AppPermissions.Pages_CreditNoteItems_Create_Adjuster, L("CreateNewCreditNoteItemAdjuster"));// Ticket TSN604 - To enhance
            creditNoteItems.CreateChildPermission(AppPermissions.Pages_CreditNoteItems_Create_Admin, L("CreateNewCreditNoteItemAdmin"));// Ticket TSN604 - To enhance

            var debitNoteItems = pages.CreateChildPermission(AppPermissions.Pages_DebitNoteItems, L("DebitNoteItems"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Create, L("CreateNewDebitNoteItem"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Edit, L("EditDebitNoteItem"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Delete, L("DeleteDebitNoteItem"));
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Create_Adjuster, L("CreateNewDebitNoteItemAdjuster"));// Ticket TSN604 - To enhance
            debitNoteItems.CreateChildPermission(AppPermissions.Pages_DebitNoteItems_Create_Admin, L("CreateNewDebitNoteItemAdmin"));// Ticket TSN604 - To enhance

            var caseDebitNotes = pages.CreateChildPermission(AppPermissions.Pages_CaseDebitNotes, L("CaseDebitNotes"));
            caseDebitNotes.CreateChildPermission(AppPermissions.Pages_CaseDebitNotes_Create, L("CreateNewCaseDebitNote"));
            caseDebitNotes.CreateChildPermission(AppPermissions.Pages_CaseDebitNotes_Edit, L("EditCaseDebitNote"));
            caseDebitNotes.CreateChildPermission(AppPermissions.Pages_CaseDebitNotes_Delete, L("DeleteCaseDebitNote"));

            var adjusterReports = pages.CreateChildPermission(AppPermissions.Pages_AdjusterReports, L("AdjusterReports"));
            adjusterReports.CreateChildPermission(AppPermissions.Pages_AdjusterReports_Create, L("CreateNewAdjusterReport"));
            adjusterReports.CreateChildPermission(AppPermissions.Pages_AdjusterReports_Edit, L("EditAdjusterReport"));
            adjusterReports.CreateChildPermission(AppPermissions.Pages_AdjusterReports_Delete, L("DeleteAdjusterReport"));

            var invoiceReports = pages.CreateChildPermission(AppPermissions.Pages_InvoiceReports, L("InvoiceReports"));
            invoiceReports.CreateChildPermission(AppPermissions.Pages_InvoiceReports_Create, L("CreateNewInvoiceReport"));
            invoiceReports.CreateChildPermission(AppPermissions.Pages_InvoiceReports_Edit, L("EditInvoiceReport"));
            invoiceReports.CreateChildPermission(AppPermissions.Pages_InvoiceReports_Delete, L("DeleteInvoiceReport"));

            var caseReports = pages.CreateChildPermission(AppPermissions.Pages_CaseReports, L("CaseReports"));
            caseReports.CreateChildPermission(AppPermissions.Pages_CaseReports_Create, L("CreateNewCaseReport"));
            caseReports.CreateChildPermission(AppPermissions.Pages_CaseReports_Edit, L("EditCaseReport"));
            caseReports.CreateChildPermission(AppPermissions.Pages_CaseReports_Delete, L("DeleteCaseReport"));

            var wipReports = pages.CreateChildPermission(AppPermissions.Pages_WIPReports, L("WIPReports"));
            wipReports.CreateChildPermission(AppPermissions.Pages_WIPReports_Create, L("CreateNewWIPReport"));
            wipReports.CreateChildPermission(AppPermissions.Pages_WIPReports_Edit, L("EditWIPReport"));
            wipReports.CreateChildPermission(AppPermissions.Pages_WIPReports_Delete, L("DeleteWIPReport"));

            var wipSummaryReports = pages.CreateChildPermission(AppPermissions.Pages_WIPSummaryReports, L("WIPSummaryReports"));
            wipSummaryReports.CreateChildPermission(AppPermissions.Pages_WIPSummaryReports_Create, L("CreateNewWIPSummaryReport"));
            wipSummaryReports.CreateChildPermission(AppPermissions.Pages_WIPSummaryReports_Edit, L("EditWIPSummaryReport"));
            wipSummaryReports.CreateChildPermission(AppPermissions.Pages_WIPSummaryReports_Delete, L("DeleteWIPSummaryReport"));

            var invoiceItems = pages.CreateChildPermission(AppPermissions.Pages_InvoiceItems, L("InvoiceItems"));
            invoiceItems.CreateChildPermission(AppPermissions.Pages_InvoiceItems_Create, L("CreateNewInvoiceItem"));
            invoiceItems.CreateChildPermission(AppPermissions.Pages_InvoiceItems_Edit, L("EditInvoiceItem"));
            invoiceItems.CreateChildPermission(AppPermissions.Pages_InvoiceItems_Delete, L("DeleteInvoiceItem"));
            invoiceItems.CreateChildPermission(AppPermissions.Pages_InvoiceItems_Create_Admin, L("CreateNewInvoiceItemAdmin")); // Ticket TSN604 - To enhance
            invoiceItems.CreateChildPermission(AppPermissions.Pages_InvoiceItems_Create_Adjuster, L("CreateNewInvoiceItemAdjuster"));// Ticket TSN604 - To enhance

            var caseInvoices = pages.CreateChildPermission(AppPermissions.Pages_CaseInvoices, L("CaseInvoices"), multiTenancySides: MultiTenancySides.Tenant);
            caseInvoices.CreateChildPermission(AppPermissions.Pages_CaseInvoices_Create, L("CreateNewCaseInvoice"), multiTenancySides: MultiTenancySides.Tenant);
            caseInvoices.CreateChildPermission(AppPermissions.Pages_CaseInvoices_Edit, L("EditCaseInvoice"), multiTenancySides: MultiTenancySides.Tenant);
            caseInvoices.CreateChildPermission(AppPermissions.Pages_CaseInvoices_Delete, L("DeleteCaseInvoice"), multiTenancySides: MultiTenancySides.Tenant);

            var caseClaims = pages.CreateChildPermission(AppPermissions.Pages_CaseClaims, L("CaseClaims"), multiTenancySides: MultiTenancySides.Tenant);
            caseClaims.CreateChildPermission(AppPermissions.Pages_CaseClaims_Create, L("CreateNewCaseClaim"), multiTenancySides: MultiTenancySides.Tenant);
            caseClaims.CreateChildPermission(AppPermissions.Pages_CaseClaims_Edit, L("EditCaseClaim"), multiTenancySides: MultiTenancySides.Tenant);
            caseClaims.CreateChildPermission(AppPermissions.Pages_CaseClaims_Delete, L("DeleteCaseClaim"), multiTenancySides: MultiTenancySides.Tenant);

            var caseSearchFees = pages.CreateChildPermission(AppPermissions.Pages_CaseSearchFees, L("CaseSearchFees"));
            caseSearchFees.CreateChildPermission(AppPermissions.Pages_CaseSearchFees_Create, L("CreateNewCaseSearchFee"));
            caseSearchFees.CreateChildPermission(AppPermissions.Pages_CaseSearchFees_Edit, L("EditCaseSearchFee"));
            caseSearchFees.CreateChildPermission(AppPermissions.Pages_CaseSearchFees_Delete, L("DeleteCaseSearchFee"));

            var branch = pages.CreateChildPermission(AppPermissions.Pages_Branch, L("Branch"), multiTenancySides: MultiTenancySides.Tenant);
            branch.CreateChildPermission(AppPermissions.Pages_Branch_Create, L("CreateNewBranch"), multiTenancySides: MultiTenancySides.Tenant);
            branch.CreateChildPermission(AppPermissions.Pages_Branch_Edit, L("EditBranch"), multiTenancySides: MultiTenancySides.Tenant);
            branch.CreateChildPermission(AppPermissions.Pages_Branch_Delete, L("DeleteBranch"), multiTenancySides: MultiTenancySides.Tenant);

            var lookups = pages.CreateChildPermission(AppPermissions.Pages_Lookups, L("Lookups"));
            lookups.CreateChildPermission(AppPermissions.Pages_Lookups_Create, L("CreateNewLookup"));
            lookups.CreateChildPermission(AppPermissions.Pages_Lookups_Edit, L("EditLookup"));
            lookups.CreateChildPermission(AppPermissions.Pages_Lookups_Delete, L("DeleteLookup"));

            var caseThirdPartyInfos = pages.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyInfos, L("CaseThirdPartyInfos"));
            caseThirdPartyInfos.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyInfos_Create, L("CreateNewCaseThirdPartyInfo"));
            caseThirdPartyInfos.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyInfos_Edit, L("EditCaseThirdPartyInfo"));
            caseThirdPartyInfos.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyInfos_Delete, L("DeleteCaseThirdPartyInfo"));

            var caseDeclarationAnswers = pages.CreateChildPermission(AppPermissions.Pages_CaseDeclarationAnswers, L("CaseDeclarationAnswers"), multiTenancySides: MultiTenancySides.Tenant);
            caseDeclarationAnswers.CreateChildPermission(AppPermissions.Pages_CaseDeclarationAnswers_Create, L("CreateNewCaseDeclarationAnswer"), multiTenancySides: MultiTenancySides.Tenant);
            caseDeclarationAnswers.CreateChildPermission(AppPermissions.Pages_CaseDeclarationAnswers_Edit, L("EditCaseDeclarationAnswer"), multiTenancySides: MultiTenancySides.Tenant);
            caseDeclarationAnswers.CreateChildPermission(AppPermissions.Pages_CaseDeclarationAnswers_Delete, L("DeleteCaseDeclarationAnswer"), multiTenancySides: MultiTenancySides.Tenant);

            var caseThirdPartyVehicles = pages.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyVehicles, L("CaseThirdPartyVehicles"));
            caseThirdPartyVehicles.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyVehicles_Create, L("CreateNewCaseThirdPartyVehicle"));
            caseThirdPartyVehicles.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyVehicles_Edit, L("EditCaseThirdPartyVehicle"));
            caseThirdPartyVehicles.CreateChildPermission(AppPermissions.Pages_CaseThirdPartyVehicles_Delete, L("DeleteCaseThirdPartyVehicle"));

            var casePoliceReports = pages.CreateChildPermission(AppPermissions.Pages_CasePoliceReports, L("CasePoliceReports"));
            casePoliceReports.CreateChildPermission(AppPermissions.Pages_CasePoliceReports_Create, L("CreateNewCasePoliceReport"));
            casePoliceReports.CreateChildPermission(AppPermissions.Pages_CasePoliceReports_Edit, L("EditCasePoliceReport"));
            casePoliceReports.CreateChildPermission(AppPermissions.Pages_CasePoliceReports_Delete, L("DeleteCasePoliceReport"));

            var caseIncidentDetails = pages.CreateChildPermission(AppPermissions.Pages_CaseIncidentDetails, L("CaseIncidentDetails"));
            caseIncidentDetails.CreateChildPermission(AppPermissions.Pages_CaseIncidentDetails_Create, L("CreateNewCaseIncidentDetail"));
            caseIncidentDetails.CreateChildPermission(AppPermissions.Pages_CaseIncidentDetails_Edit, L("EditCaseIncidentDetail"));
            caseIncidentDetails.CreateChildPermission(AppPermissions.Pages_CaseIncidentDetails_Delete, L("DeleteCaseIncidentDetail"));

            var insuredPersons = pages.CreateChildPermission(AppPermissions.Pages_InsuredPersons, L("InsuredPersons"));
            insuredPersons.CreateChildPermission(AppPermissions.Pages_InsuredPersons_Create, L("CreateNewInsuredPerson"));
            insuredPersons.CreateChildPermission(AppPermissions.Pages_InsuredPersons_Edit, L("EditInsuredPerson"));
            insuredPersons.CreateChildPermission(AppPermissions.Pages_InsuredPersons_Delete, L("DeleteInsuredPerson"));

            var hospitals = pages.CreateChildPermission(AppPermissions.Pages_Hospitals, L("Hospitals"), multiTenancySides: MultiTenancySides.Tenant);
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Create, L("CreateNewHospital"), multiTenancySides: MultiTenancySides.Tenant);
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Edit, L("EditHospital"), multiTenancySides: MultiTenancySides.Tenant);
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Delete, L("DeleteHospital"), multiTenancySides: MultiTenancySides.Tenant);

            var locations = pages.CreateChildPermission(AppPermissions.Pages_Locations, L("Locations"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Create, L("CreateNewLocation"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Edit, L("EditLocation"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Delete, L("DeleteLocation"));

            var caseInsurers = pages.CreateChildPermission(AppPermissions.Pages_CaseInsurers, L("CaseInsurers"));
            caseInsurers.CreateChildPermission(AppPermissions.Pages_CaseInsurers_Create, L("CreateNewCaseInsurer"));
            caseInsurers.CreateChildPermission(AppPermissions.Pages_CaseInsurers_Edit, L("EditCaseInsurer"));
            caseInsurers.CreateChildPermission(AppPermissions.Pages_CaseInsurers_Delete, L("DeleteCaseInsurer"));

            var caseStakeholders = pages.CreateChildPermission(AppPermissions.Pages_CaseStakeholders, L("CaseStakeholders"));
            caseStakeholders.CreateChildPermission(AppPermissions.Pages_CaseStakeholders_Create, L("CreateNewCaseStakeholder"));
            caseStakeholders.CreateChildPermission(AppPermissions.Pages_CaseStakeholders_Edit, L("EditCaseStakeholder"));
            caseStakeholders.CreateChildPermission(AppPermissions.Pages_CaseStakeholders_Delete, L("DeleteCaseStakeholder"));

            var caseWorkshops = pages.CreateChildPermission(AppPermissions.Pages_CaseWorkshops, L("CaseWorkshops"));
            caseWorkshops.CreateChildPermission(AppPermissions.Pages_CaseWorkshops_Create, L("CreateNewCaseWorkshop"));
            caseWorkshops.CreateChildPermission(AppPermissions.Pages_CaseWorkshops_Edit, L("EditCaseWorkshop"));
            caseWorkshops.CreateChildPermission(AppPermissions.Pages_CaseWorkshops_Delete, L("DeleteCaseWorkshop"));

            var caseLawyers = pages.CreateChildPermission(AppPermissions.Pages_CaseLawyers, L("CaseLawyers"));
            caseLawyers.CreateChildPermission(AppPermissions.Pages_CaseLawyers_Create, L("CreateNewCaseLawyer"));
            caseLawyers.CreateChildPermission(AppPermissions.Pages_CaseLawyers_Edit, L("EditCaseLawyer"));
            caseLawyers.CreateChildPermission(AppPermissions.Pages_CaseLawyers_Delete, L("DeleteCaseLawyer"));

            var caseAdjusters = pages.CreateChildPermission(AppPermissions.Pages_CaseAdjusters, L("CaseAdjusters"), multiTenancySides: MultiTenancySides.Tenant);
            caseAdjusters.CreateChildPermission(AppPermissions.Pages_CaseAdjusters_Create, L("CreateNewCaseAdjuster"), multiTenancySides: MultiTenancySides.Tenant);
            caseAdjusters.CreateChildPermission(AppPermissions.Pages_CaseAdjusters_Edit, L("EditCaseAdjuster"), multiTenancySides: MultiTenancySides.Tenant);
            caseAdjusters.CreateChildPermission(AppPermissions.Pages_CaseAdjusters_Delete, L("DeleteCaseAdjuster"), multiTenancySides: MultiTenancySides.Tenant);

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var documentSettings = administration.CreateChildPermission(AppPermissions.Pages_Administration_DocumentSettings, L("DocumentSettings"));
            documentSettings.CreateChildPermission(AppPermissions.Pages_Administration_DocumentSettings_Create, L("CreateNewDocumentSetting"));
            documentSettings.CreateChildPermission(AppPermissions.Pages_Administration_DocumentSettings_Edit, L("EditDocumentSetting"));
            documentSettings.CreateChildPermission(AppPermissions.Pages_Administration_DocumentSettings_Delete, L("DeleteDocumentSetting"));

            var vehicles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Vehicles, L("Vehicles"), multiTenancySides: MultiTenancySides.Tenant);
            vehicles.CreateChildPermission(AppPermissions.Pages_Administration_Vehicles_Create, L("CreateNewVehicle"), multiTenancySides: MultiTenancySides.Tenant);
            vehicles.CreateChildPermission(AppPermissions.Pages_Administration_Vehicles_Edit, L("EditVehicle"), multiTenancySides: MultiTenancySides.Tenant);
            vehicles.CreateChildPermission(AppPermissions.Pages_Administration_Vehicles_Delete, L("DeleteVehicle"), multiTenancySides: MultiTenancySides.Tenant);

            var prompts = administration.CreateChildPermission(AppPermissions.Pages_Administration_Prompts, L("Prompts"));
            //IF SHOWPROMPT IN APPSETTINGS IS TRUE, DISPLAY IN TENANT
            var isShowPrompts = Convert.ToBoolean(_appConfiguration["OCR:ShowAdminPrompt"]);

            if (isShowPrompts)
            {
                //prompts = administration.CreateChildPermission(AppPermissions.Pages_Administration_Prompts, L("Prompts"), multiTenancySides: MultiTenancySides.Tenant);
                prompts.CreateChildPermission(AppPermissions.Pages_Administration_Prompts_Edit, L("EditPrompt"));
            }
            else
            {
                prompts.CreateChildPermission(AppPermissions.Pages_Administration_Prompts_Edit, L("EditPrompt"), multiTenancySides: MultiTenancySides.Host);
            }

            var auditEntries = administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditEntries, L("AuditEntries"));
            auditEntries.CreateChildPermission(AppPermissions.Pages_Administration_AuditEntries_Create, L("CreateNewAuditEntry"));
            auditEntries.CreateChildPermission(AppPermissions.Pages_Administration_AuditEntries_Edit, L("EditAuditEntry"));
            auditEntries.CreateChildPermission(AppPermissions.Pages_Administration_AuditEntries_Delete, L("DeleteAuditEntry"));

            var auditTrails = administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditTrails, L("AuditTrails"));
            auditTrails.CreateChildPermission(AppPermissions.Pages_Administration_AuditTrails_Create, L("CreateNewAuditTrail"));
            auditTrails.CreateChildPermission(AppPermissions.Pages_Administration_AuditTrails_Edit, L("EditAuditTrail"));
            auditTrails.CreateChildPermission(AppPermissions.Pages_Administration_AuditTrails_Delete, L("DeleteAuditTrail"));

            var companies = administration.CreateChildPermission(AppPermissions.Pages_Administration_Companies, L("Companies"), multiTenancySides: MultiTenancySides.Tenant);
            companies.CreateChildPermission(AppPermissions.Pages_Administration_Companies_Create, L("CreateNewCompany"), multiTenancySides: MultiTenancySides.Tenant);
            companies.CreateChildPermission(AppPermissions.Pages_Administration_Companies_Edit, L("EditCompany"), multiTenancySides: MultiTenancySides.Tenant);
            companies.CreateChildPermission(AppPermissions.Pages_Administration_Companies_Delete, L("DeleteCompany"), multiTenancySides: MultiTenancySides.Tenant);

            var staffs = administration.CreateChildPermission(AppPermissions.Pages_Administration_Staffs, L("Staffs"));
            staffs.CreateChildPermission(AppPermissions.Pages_Administration_Staffs_Create, L("CreateNewStaff"));
            staffs.CreateChildPermission(AppPermissions.Pages_Administration_Staffs_Edit, L("EditStaff"));
            staffs.CreateChildPermission(AppPermissions.Pages_Administration_Staffs_Delete, L("DeleteStaff"));

            var groups = administration.CreateChildPermission(AppPermissions.Pages_Administration_Groups, L("Groups"), multiTenancySides: MultiTenancySides.Tenant);
            groups.CreateChildPermission(AppPermissions.Pages_Administration_Groups_Create, L("CreateNewGroup"), multiTenancySides: MultiTenancySides.Tenant);
            groups.CreateChildPermission(AppPermissions.Pages_Administration_Groups_Edit, L("EditGroup"), multiTenancySides: MultiTenancySides.Tenant);
            groups.CreateChildPermission(AppPermissions.Pages_Administration_Groups_Delete, L("DeleteGroup"), multiTenancySides: MultiTenancySides.Tenant);

            var workshops = administration.CreateChildPermission(AppPermissions.Pages_Administration_Workshops, L("Workshops"), multiTenancySides: MultiTenancySides.Tenant);
            workshops.CreateChildPermission(AppPermissions.Pages_Administration_Workshops_Create, L("CreateNewWorkshop"), multiTenancySides: MultiTenancySides.Tenant);
            workshops.CreateChildPermission(AppPermissions.Pages_Administration_Workshops_Edit, L("EditWorkshop"), multiTenancySides: MultiTenancySides.Tenant);
            workshops.CreateChildPermission(AppPermissions.Pages_Administration_Workshops_Delete, L("DeleteWorkshop"), multiTenancySides: MultiTenancySides.Tenant);

            var lawFirms = administration.CreateChildPermission(AppPermissions.Pages_Administration_LawFirms, L("LawFirms"), multiTenancySides: MultiTenancySides.Tenant);
            lawFirms.CreateChildPermission(AppPermissions.Pages_Administration_LawFirms_Create, L("CreateNewLawFirm"), multiTenancySides: MultiTenancySides.Tenant);
            lawFirms.CreateChildPermission(AppPermissions.Pages_Administration_LawFirms_Edit, L("EditLawFirm"), multiTenancySides: MultiTenancySides.Tenant);
            lawFirms.CreateChildPermission(AppPermissions.Pages_Administration_LawFirms_Delete, L("DeleteLawFirm"), multiTenancySides: MultiTenancySides.Tenant);

            var declarationQuestions = administration.CreateChildPermission(AppPermissions.Pages_Administration_DeclarationQuestions, L("DeclarationQuestions"), multiTenancySides: MultiTenancySides.Tenant);
            declarationQuestions.CreateChildPermission(AppPermissions.Pages_Administration_DeclarationQuestions_Create, L("CreateNewDeclarationQuestion"), multiTenancySides: MultiTenancySides.Tenant);
            declarationQuestions.CreateChildPermission(AppPermissions.Pages_Administration_DeclarationQuestions_Edit, L("EditDeclarationQuestion"), multiTenancySides: MultiTenancySides.Tenant);
            declarationQuestions.CreateChildPermission(AppPermissions.Pages_Administration_DeclarationQuestions_Delete, L("DeleteDeclarationQuestion"), multiTenancySides: MultiTenancySides.Tenant);

            var scopeAssignments = administration.CreateChildPermission(AppPermissions.Pages_Administration_ScopeAssignments, L("ScopeAssignments"), multiTenancySides: MultiTenancySides.Tenant);
            scopeAssignments.CreateChildPermission(AppPermissions.Pages_Administration_ScopeAssignments_Create, L("CreateNewScopeAssignment"), multiTenancySides: MultiTenancySides.Tenant);
            scopeAssignments.CreateChildPermission(AppPermissions.Pages_Administration_ScopeAssignments_Edit, L("EditScopeAssignment"), multiTenancySides: MultiTenancySides.Tenant);
            scopeAssignments.CreateChildPermission(AppPermissions.Pages_Administration_ScopeAssignments_Delete, L("DeleteScopeAssignment"), multiTenancySides: MultiTenancySides.Tenant);

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Roles, L("UserRoles"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            var caseType = administration.CreateChildPermission(AppPermissions.Pages_Administration_CaseType, L("CaseType"), multiTenancySides: MultiTenancySides.Tenant);
            caseType.CreateChildPermission(AppPermissions.Pages_Administration_CaseType_Create, L("CreateNewCaseType"), multiTenancySides: MultiTenancySides.Tenant);
            caseType.CreateChildPermission(AppPermissions.Pages_Administration_CaseType_Edit, L("EditCaseType"), multiTenancySides: MultiTenancySides.Tenant);
            caseType.CreateChildPermission(AppPermissions.Pages_Administration_CaseType_Delete, L("DeleteCaseType"), multiTenancySides: MultiTenancySides.Tenant);

            var reassign = pages.CreateChildPermission(AppPermissions.Pages_Case_Reassign_Adjuster, L("Reassign Adjuster"), multiTenancySides: MultiTenancySides.Tenant);
            reassign.CreateChildPermission(AppPermissions.Pages_Case_Reassign_Company, L("Reassign Company"), multiTenancySides: MultiTenancySides.Tenant);

            var expensesClaimsApproval = pages.CreateChildPermission(AppPermissions.Pages_Administration_ExpensesClaimsApproval, L("Expenses CLaims Approval"), multiTenancySides: MultiTenancySides.Tenant);
            expensesClaimsApproval.CreateChildPermission(AppPermissions.Pages_Administration_ExpensesClaimsApproval_Create, L("Create Expenses CLaims Approval"), multiTenancySides: MultiTenancySides.Tenant);
            expensesClaimsApproval.CreateChildPermission(AppPermissions.Pages_Administration_ExpensesClaimsApproval_Edit, L("Edit Expenses CLaims Approval"), multiTenancySides: MultiTenancySides.Tenant);
            expensesClaimsApproval.CreateChildPermission(AppPermissions.Pages_Administration_ExpensesClaimsApproval_Delete, L("Delete Expenses CLaims Approval"), multiTenancySides: MultiTenancySides.Tenant);

            var manageCaseClaims = pages.CreateChildPermission(AppPermissions.Pages_Manage_CaseClaims, L("Manage Case Claims"), multiTenancySides: MultiTenancySides.Tenant);

            var openAIIntegrationLogs = administration.CreateChildPermission(AppPermissions.Pages_Administration_OpenAIIntegrationLogs, L("OpenAIIntegrationLogs"), multiTenancySides: MultiTenancySides.Tenant);

            var thirdPartyViewApproval = administration.CreateChildPermission(AppPermissions.Pages_Administration_ThirdPartyViewApproval, L("ThirdPartyViewApproval"), multiTenancySides: MultiTenancySides.Tenant);

            var extendCompletion = administration.CreateChildPermission(AppPermissions.Pages_CaseAdjusters_ExtendCompletion, L("ExtendCompletion"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ThinknInsurTechConsts.LocalizationSourceName);
        }
    }
}
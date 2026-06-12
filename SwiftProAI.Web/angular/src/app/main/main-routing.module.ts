import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'registration/casePoliceReportSummaries',
                        loadChildren: () => import('./registration/casePoliceReportSummaries/casePoliceReportSummary.module').then(m => m.CasePoliceReportSummaryModule),
                        data: { permission: 'Pages.CasePoliceReportSummaries' }
                    },
                
                    
                    {
                        path: 'common/fileOrgs',
                        loadChildren: () => import('./common/fileOrgs/fileOrg.module').then(m => m.FileOrgModule),
                        data: { permission: 'Pages.FileOrgs' }
                    },
                
                    
                    {
                        path: 'registration/creditNoteItems',
                        loadChildren: () => import('./registration/creditNoteItems/creditNoteItem.module').then(m => m.CreditNoteItemModule),
                        data: { permission: 'Pages.CreditNoteItems' }
                    },
                
                	{
                        path: 'reports/invoiceReports',
                        loadChildren: () => import('./reports/invoiceReports/invoiceReport.module').then(m => m.InvoiceReportModule),
                        data: { permission: 'Pages.InvoiceReports' }
                    },
                    
                    {
                        path: 'registration/debitNoteItems',
                        loadChildren: () => import('./registration/debitNoteItems/debitNoteItem.module').then(m => m.DebitNoteItemModule),
                        data: { permission: 'Pages.DebitNoteItems' }
                    },
                
                    
                    {
                        path: 'registration/caseDebitNotes',
                        loadChildren: () => import('./registration/caseDebitNotes/caseDebitNote.module').then(m => m.CaseDebitNoteModule),
                        data: { permission: 'Pages.CaseDebitNotes' }
                    },
                
                    {
                        path: 'registration/caseCreditNotes',
                        loadChildren: () => import('./registration/caseCreditNotes/caseCreditNote.module').then(m => m.CaseCreditNoteModule),
                        data: { permission: 'Pages.CaseCreditNotes' }
                    },
                
                    
                    {
                        path: 'reports/adjusterReports',
                        loadChildren: () => import('./reports/adjusterReports/adjusterReport.module').then(m => m.AdjusterReportModule),
                        data: { permission: 'Pages.AdjusterReports' }
                    },
                    
                    
                    {
                        path: 'reports/caseReports',
                        loadChildren: () => import('./reports/caseReports/caseReport.module').then(m => m.CaseReportModule),
                        data: { permission: 'Pages.CaseReports' }
                    },
                
                    
                    {
                        path: 'reports/wipReports',
                        loadChildren: () => import('./reports/wipReports/wipReport.module').then(m => m.WIPReportModule),
                        data: { permission: 'Pages.WIPReports' }
                    },
                    
                    {
                        path: 'reports/wipSummaryReports',
                        loadChildren: () => import('./reports/wipSummaryReports/wipSummaryReport.module').then(m => m.WIPSummaryReportModule),
                        data: { permission: 'Pages.WIPSummaryReports' }
                    },
              
                    
                    {
                        path: 'registration/invoiceItems',
                        loadChildren: () => import('./registration/invoiceItems/invoiceItem.module').then(m => m.InvoiceItemModule),
                        data: { permission: 'Pages.InvoiceItems' }
                    },
                
                    
                    {
                        path: 'registration/caseInvoices',
                        loadChildren: () => import('./registration/caseInvoices/caseInvoice.module').then(m => m.CaseInvoiceModule),
                        data: { permission: 'Pages.CaseInvoices' }
                    },
                           
                    {
                        path: 'registration/caseClaims',
                        loadChildren: () => import('./registration/caseClaims/caseClaim.module').then(m => m.CaseClaimModule),
                        data: { permission: 'Pages.CaseClaims' }
                    },
                
                    
                    {
                        path: 'registration/caseSearchFees',
                        loadChildren: () => import('./registration/caseSearchFees/caseSearchFee.module').then(m => m.CaseSearchFeeModule),
                        data: { permission: 'Pages.CaseSearchFees' }
                    },
                    
                    {
                        path: 'branches/branch',
                        loadChildren: () => import('./branches/branch/branch.module').then(m => m.BranchModule),
                        data: { permission: 'Pages.Branch' }
                    },
                
                                        
                    {
                        path: 'registration/caseThirdPartyInfos',
                        loadChildren: () => import('./registration/caseThirdPartyInfos/caseThirdPartyInfo.module').then(m => m.CaseThirdPartyInfoModule),
                        data: { permission: 'Pages.CaseThirdPartyInfos' }
                    },
                

                    {
                        path: 'registration/caseDeclarationAnswers',
                        loadChildren: () => import('./registration/caseDeclarationAnswers/caseDeclarationAnswer.module').then(m => m.CaseDeclarationAnswerModule),
                        data: { permission: 'Pages.CaseDeclarationAnswers' }
                    },
                    {
                        path: 'registration/caseExpenses',
                        loadChildren: () => import('./registration/caseExpenses/caseExpense.module').then(m => m.CaseExpenseModule),
                        data: { permission: 'Pages.caseExpenses' }
                    },
                

                    {
                        path: 'registration/caseThirdPartyVehicles',
                        loadChildren: () => import('./registration/caseThirdPartyVehicles/caseThirdPartyVehicle.module').then(m => m.CaseThirdPartyVehicleModule),
                        data: { permission: 'Pages.CaseThirdPartyVehicles' }
                    },
                
                    
                    {
                        path: 'registration/casePoliceReports',
                        loadChildren: () => import('./registration/casePoliceReports/casePoliceReport.module').then(m => m.CasePoliceReportModule),
                        data: { permission: 'Pages.CasePoliceReports' }
                    },
                
                    
                    {
                        path: 'registration/caseInvestigationOfficers',
                        loadChildren: () => import('./registration/caseInvestigationOfficers/caseInvestigationOfficer.module').then(m => m.CaseInvestigationOfficerModule),
                        data: { permission: 'Pages.CaseInvestigationOfficers' }
                    },
                
                    
                    {
                        path: 'registration/caseIncidentDetails',
                        loadChildren: () => import('./registration/caseIncidentDetails/caseIncidentDetail.module').then(m => m.CaseIncidentDetailModule),
                        data: { permission: 'Pages.CaseIncidentDetails' }
                    },

                    {
                        path: 'registration/caseStakeholders',
                        loadChildren: () => import('./registration/caseStakeholders/caseStakeholder.module').then(m => m.CaseStakeholderModule),
                        data: { permission: 'Pages.CaseInsurers' }
                    },
                    
                    {
                        path: 'registration/caseInsuredDrivers',
                        loadChildren: () => import('./registration/caseInsuredDrivers/caseInsuredDriver.module').then(m => m.CaseInsuredDriverModule),
                        data: { permission: 'Pages.InsuredPersons' }
                    },
                
                    {
                        path: 'registration/insuredPersons',
                        loadChildren: () => import('./registration/insuredPersons/insuredPerson.module').then(m => m.InsuredPersonModule),
                        data: { permission: 'Pages.InsuredPersons' }
                    },
                
                    
                    {
                        path: 'registration/hospitals',
                        loadChildren: () => import('./registration/hospitals/hospital.module').then(m => m.HospitalModule),
                        data: { permission: 'Pages.Hospitals' }
                    },
                
                    {
                        path: 'registration/caseInsurers',
                        loadChildren: () => import('./registration/caseInsurers/caseInsurer.module').then(m => m.CaseInsurerModule),
                        data: { permission: 'Pages.CaseInsurers' }
                    },
                
                    {
                        path: 'registration/caseWorkshops',
                        loadChildren: () => import('./registration/caseWorkshops/caseWorkshop.module').then(m => m.CaseWorkshopModule),
                        data: { permission: 'Pages.CaseWorkshops' }
                    },
                
                    
                    {
                        path: 'registration/caseLawyers',
                        loadChildren: () => import('./registration/caseLawyers/caseLawyer.module').then(m => m.CaseLawyerModule),
                        data: { permission: 'Pages.CaseLawyers' }
                    },

                    {
                        path: 'registration/caseAdjusters',
                        loadChildren: () => import('./registration/caseAdjusters/caseAdjuster.module').then(m => m.CaseAdjusterModule),
                        data: { permission: 'Pages.CaseAdjusters' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'dashboard3rdParty',
                        loadChildren: () => import('./dashboard3rdParty/dashboard3rdParty.module').then((m) => m.Dashboard3rdPartyModule),
                        data: { permission: 'Pages.Tenant.Dashboard3rdParty' },
                    },
                    {
                        path: 'registration',
                        loadChildren: () => import('./registration/registration.module').then(m => m.RegistrationModule)
                    },
                    {
                        path: 'ocr',
                        loadChildren: () => import('./ocr/ocr.module').then(m => m.OCRModule)
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}

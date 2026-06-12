import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CasePoliceReportSummaryRoutingModule} from './casePoliceReportSummary-routing.module';
import {CasePoliceReportSummariesComponent} from './casePoliceReportSummaries.component';
import {CreateOrEditCasePoliceReportSummaryComponent} from './create-or-edit-casePoliceReportSummary.component';




@NgModule({
    declarations: [
        CasePoliceReportSummariesComponent,
        CreateOrEditCasePoliceReportSummaryComponent,
        
        
    ],
    imports: [AppSharedModule, CasePoliceReportSummaryRoutingModule , AdminSharedModule ],
    
})
export class CasePoliceReportSummaryModule {
}

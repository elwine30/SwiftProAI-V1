import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseReportRoutingModule} from './caseReport-routing.module';
import {CaseReportsComponent} from './caseReports.component';



@NgModule({
    declarations: [
        CaseReportsComponent,
        
    ],
    imports: [AppSharedModule, CaseReportRoutingModule , AdminSharedModule ],
    
})
export class CaseReportModule {
}

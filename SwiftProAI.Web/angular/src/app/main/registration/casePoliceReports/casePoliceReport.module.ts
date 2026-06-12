import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CasePoliceReportRoutingModule} from './casePoliceReport-routing.module';
import {CreateOrEditCasePoliceReportComponent} from './create-or-edit-casePoliceReport.component';
import { CreateOrEditCaseInvestigationOfficerComponent } from '../caseInvestigationOfficers/create-or-edit-caseInvestigationOfficer.component';
import { CasePoliceReportFileUploadModalComponent } from './casePoliceReport-file-upload-modal.component';
import { ViewCasePoliceReportComponent } from './view-casePoliceReport.component';
import { ViewCasePoliceReportModalComponent } from './view-casePoliceReport-modal.component';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';


@NgModule({
    declarations: [
        CreateOrEditCasePoliceReportComponent,
        CreateOrEditCaseInvestigationOfficerComponent,
        CasePoliceReportFileUploadModalComponent,
        ViewCasePoliceReportComponent,
        ViewCasePoliceReportModalComponent,
    ],
    imports: [AppSharedModule, CasePoliceReportRoutingModule , AdminSharedModule ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDateTimePickerConfig },
    ]
})
export class CasePoliceReportModule {
}

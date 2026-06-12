import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseIncidentDetailRoutingModule} from './caseIncidentDetail-routing.module';
import {CreateOrEditCaseIncidentDetailComponent} from './create-or-edit-caseIncidentDetail.component';
import {ViewCaseIncidentDetailComponent} from './view-caseIncidentDetail.component';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';



@NgModule({
    declarations: [
        CreateOrEditCaseIncidentDetailComponent,
        ViewCaseIncidentDetailComponent,
        
    ],
    imports: [AppSharedModule, CaseIncidentDetailRoutingModule , AdminSharedModule ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDateTimePickerConfig },
    ]
})
export class CaseIncidentDetailModule {
}

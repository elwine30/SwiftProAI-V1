import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {WIPReportRoutingModule} from './wipReport-routing.module';
import {WIPReportsComponent} from './wipReports.component';



@NgModule({
    declarations: [
        WIPReportsComponent,
    ],
    imports: [AppSharedModule, WIPReportRoutingModule , AdminSharedModule ],
    
})
export class WIPReportModule {
}

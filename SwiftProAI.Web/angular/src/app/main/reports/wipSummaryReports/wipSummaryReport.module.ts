import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { WIPSummaryReportRoutingModule } from './wipSummaryReport-routing.module';
import { WipSummaryReportComponent } from './wipSummaryReport.component';

@NgModule({
    declarations: [WipSummaryReportComponent],
    imports: [AppSharedModule, WIPSummaryReportRoutingModule, AdminSharedModule],
})
export class WIPSummaryReportModule {}

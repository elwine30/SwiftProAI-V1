import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AdjusterReportRoutingModule } from './adjusterReport-routing.module';
import { AdjusterReportsComponent } from './adjusterReports.component';

@NgModule({
    declarations: [AdjusterReportsComponent],
    imports: [AppSharedModule, AdjusterReportRoutingModule, AdminSharedModule],
})
export class AdjusterReportModule {}

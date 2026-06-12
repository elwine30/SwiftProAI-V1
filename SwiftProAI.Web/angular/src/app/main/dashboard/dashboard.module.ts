import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { CustomizableDashboardModule } from '@app/shared/common/customizable-dashboard/customizable-dashboard.module';
import {CreateRegistrationModalComponent} from './create-registration-modal.component';
import { ReasignCompanyModalComponent } from './reasign-company-modal.component';
import { ReasignCaseAdjusterModalComponent } from './reasign-adjuster-modal.component';
import { PaymentUpdateModalComponent } from './payment-update-modal.component';

@NgModule({
    declarations: [DashboardComponent, CreateRegistrationModalComponent, ReasignCompanyModalComponent, ReasignCaseAdjusterModalComponent, PaymentUpdateModalComponent],
    imports: [AppSharedModule, AdminSharedModule, DashboardRoutingModule, CustomizableDashboardModule],
})
export class DashboardModule {}

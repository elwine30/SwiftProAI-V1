import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CaseAdjusterRoutingModule } from './caseAdjuster-routing.module';
import { CaseAdjustersComponent } from './caseAdjusters.component';
import { CreateOrEditCaseAdjusterComponent } from './create-or-edit-caseAdjuster.component';
import { AppModule } from '@app/app.module';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { ViewCaseAdjusterComponent } from './view-caseAdjuster.component';

@NgModule({
    declarations: [CaseAdjustersComponent, CreateOrEditCaseAdjusterComponent, ViewCaseAdjusterComponent],
    imports: [AppSharedModule, CaseAdjusterRoutingModule, AdminSharedModule, AppModule, AppCommonModule],
})
export class CaseAdjusterModule {}

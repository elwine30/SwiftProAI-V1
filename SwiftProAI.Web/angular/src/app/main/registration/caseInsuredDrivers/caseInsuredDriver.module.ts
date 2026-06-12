import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CaseInsuredDriverRoutingModule } from './caseInsuredDriver-routing.module';
import { CreateOrEditCaseInsuredDriverComponent } from './create-or-edit-caseInsuredDriver.component';
import { ViewCaseInsuredDriverComponent } from './view-caseInsuredDriver.component';

@NgModule({
    declarations: [CreateOrEditCaseInsuredDriverComponent, ViewCaseInsuredDriverComponent],
    imports: [AppSharedModule, CaseInsuredDriverRoutingModule, AdminSharedModule],
})
export class CaseInsuredDriverModule {}

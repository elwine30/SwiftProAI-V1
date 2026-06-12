import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CaseThirdPartyInfoRoutingModule } from './caseThirdPartyInfo-routing.module';
import { CaseThirdPartyInfosComponent } from './caseThirdPartyInfos.component';
import { CreateOrEditCaseThirdPartyInfoComponent } from './create-or-edit-caseThirdPartyInfo.component';
import { CaseThirdPartyUploadComponent } from './caseThirdPartyUpload.component';
import { DatePipe } from '@angular/common';
import { ViewCaseThirdPartyInfoComponent } from './view-caseThirdPartyInfo.component';

@NgModule({
    declarations: [
        CaseThirdPartyInfosComponent,
        CreateOrEditCaseThirdPartyInfoComponent,
        CaseThirdPartyUploadComponent,
        ViewCaseThirdPartyInfoComponent,
    ],
    imports: [AppSharedModule, CaseThirdPartyInfoRoutingModule, AdminSharedModule],
    providers: [DatePipe],
})
export class CaseThirdPartyInfoModule {}

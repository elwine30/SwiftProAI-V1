import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CaseClaimRoutingModule } from './caseClaim-routing.module';
import { CreateOrEditCaseClaimComponent } from './create-or-edit-caseClaim.component';
import { CreateOrEditCaseSearchFeeModalComponent } from '../caseSearchFees/create-or-edit-caseSearchFee-modal.component';
import { CaseSearchFeesComponent } from '../caseSearchFees/caseSearchFees.component';
import { ViewAndEditCaseClaimComponent } from './view-and-edit-caseClaim.component';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        CreateOrEditCaseClaimComponent,
        CreateOrEditCaseSearchFeeModalComponent,
        CaseSearchFeesComponent,
        ViewAndEditCaseClaimComponent,
    ],
    imports: [AppSharedModule, CaseClaimRoutingModule, AdminSharedModule, FormsModule],
})
export class CaseClaimModule {}

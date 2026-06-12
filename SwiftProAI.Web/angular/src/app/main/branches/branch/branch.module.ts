import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BranchRoutingModule } from './branch-routing.module';
import { BranchComponent } from './branch.component';
import { CreateOrEditBranchComponent } from './create-or-edit-branch.component';
import { ViewBranchComponent } from './view-branch.component';
import { CreateOrEditBranchModalComponent } from './create-or-edit-branch-modal.component';
import { ViewBranchModalComponent } from './view-branch-modal.component';

@NgModule({
    declarations: [
        BranchComponent,
        CreateOrEditBranchComponent,
        ViewBranchComponent,
        CreateOrEditBranchModalComponent,
        ViewBranchModalComponent,
    ],
    imports: [AppSharedModule, BranchRoutingModule, AdminSharedModule],
})
export class BranchModule {}

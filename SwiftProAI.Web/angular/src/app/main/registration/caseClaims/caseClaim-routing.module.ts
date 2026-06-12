import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseClaimComponent } from './create-or-edit-caseClaim.component';
import { ViewAndEditCaseClaimComponent } from './view-and-edit-caseClaim.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';

const routes: Routes = [
    {
        path: 'createOrEdit',
        component: CreateOrEditCaseClaimComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },
    {
        path: 'viewAndEdit',
        component: ViewAndEditCaseClaimComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseClaimRoutingModule {}

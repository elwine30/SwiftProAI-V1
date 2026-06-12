import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseInsuredDriverComponent } from './create-or-edit-caseInsuredDriver.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCaseInsuredDriverComponent } from './view-caseInsuredDriver.component';

const routes: Routes = [
    {
        path: 'createOrEdit',
        component: CreateOrEditCaseInsuredDriverComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },
    {
        path: 'view',
        component: ViewCaseInsuredDriverComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseInsuredDriverRoutingModule {}

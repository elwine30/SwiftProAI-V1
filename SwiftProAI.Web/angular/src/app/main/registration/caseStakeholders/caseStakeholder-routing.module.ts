import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseStakeholderComponent } from './create-or-edit-caseStakeholder.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCaseStakeholderComponent } from './view-caseStakeHolder.component';

const routes: Routes = [

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseStakeholderComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },
    {
        path: 'view',
        component: ViewCaseStakeholderComponent,
        pathMatch: 'full'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseStakeholderRoutingModule {
}

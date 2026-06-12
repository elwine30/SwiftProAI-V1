import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CaseAdjustersComponent } from './caseAdjusters.component';
import { CreateOrEditCaseAdjusterComponent } from './create-or-edit-caseAdjuster.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCaseAdjusterComponent } from './view-caseAdjuster.component';

const routes: Routes = [
    {
        path: '',
        component: CaseAdjustersComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseAdjusterComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },

    {
        path: 'view',
        component: ViewCaseAdjusterComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseAdjusterRoutingModule {}

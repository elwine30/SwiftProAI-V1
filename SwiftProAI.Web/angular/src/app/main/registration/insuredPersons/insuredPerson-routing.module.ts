import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditInsuredPersonComponent } from './create-or-edit-insuredPerson.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewInsuredPersonComponent } from './view-insuredPerson.component';

const routes: Routes = [
    {
        path: 'createOrEdit',
        component: CreateOrEditInsuredPersonComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },
    {
        path: 'view',
        component: ViewInsuredPersonComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InsuredPersonRoutingModule {}

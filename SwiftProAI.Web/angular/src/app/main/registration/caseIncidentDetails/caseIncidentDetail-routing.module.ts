import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseIncidentDetailComponent } from './create-or-edit-caseIncidentDetail.component';
import { ViewCaseIncidentDetailComponent } from './view-caseIncidentDetail.component';

const routes: Routes = [
    {
        path: 'createOrEdit',
        component: CreateOrEditCaseIncidentDetailComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewCaseIncidentDetailComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseIncidentDetailRoutingModule {}

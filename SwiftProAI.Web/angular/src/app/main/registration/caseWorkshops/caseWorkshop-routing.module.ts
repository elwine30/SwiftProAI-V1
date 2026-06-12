import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CaseWorkshopsComponent } from './caseWorkshops.component';
import { CreateOrEditCaseWorkshopComponent } from './create-or-edit-caseWorkshop.component';
import { ViewCaseWorkshopComponent } from './view-caseWorkshop.component';

const routes: Routes = [
    {
        path: '',
        component: CaseWorkshopsComponent,
        pathMatch: 'full'
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseWorkshopComponent,
        pathMatch: 'full'
    },
    {
        path: 'view',
        component: ViewCaseWorkshopComponent,
        pathMatch: 'full'
    }


];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseWorkshopRoutingModule {
}

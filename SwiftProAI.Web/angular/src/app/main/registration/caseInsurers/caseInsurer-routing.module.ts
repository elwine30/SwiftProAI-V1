import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CaseInsurersComponent } from './caseInsurers.component';
import { CreateOrEditCaseInsurerComponent } from './create-or-edit-caseInsurer.component';
import { ViewCaseInsurerComponent } from './view-caseInsurer.component';

const routes: Routes = [
    {
        path: '',
        component: CaseInsurersComponent,
        pathMatch: 'full'
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseInsurerComponent,
        pathMatch: 'full'
    },
    {
        path: 'view',
        component: ViewCaseInsurerComponent,
        pathMatch: 'full'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseInsurerRoutingModule {
}

import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateOrEditCaseInvestigationOfficerComponent} from './create-or-edit-caseInvestigationOfficer.component';

const routes: Routes = [
    
    {
        path: 'createOrEdit',
        component: CreateOrEditCaseInvestigationOfficerComponent,
        pathMatch: 'full'
    }
			
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseInvestigationOfficerRoutingModule {
}

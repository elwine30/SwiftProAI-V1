import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateOrEditCasePoliceReportComponent} from './create-or-edit-casePoliceReport.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCasePoliceReportComponent } from './view-casePoliceReport.component';

const routes: Routes = [
    
    {
        path: 'createOrEdit',
        component: CreateOrEditCasePoliceReportComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },
    {
        path: 'view',
        component: ViewCasePoliceReportComponent,
        pathMatch: 'full',
    },
			
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CasePoliceReportRoutingModule {
}

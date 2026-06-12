import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CasePoliceReportSummariesComponent} from './casePoliceReportSummaries.component';
import {CreateOrEditCasePoliceReportSummaryComponent} from './create-or-edit-casePoliceReportSummary.component';


const routes: Routes = [
    {
        path: '',
        component: CasePoliceReportSummariesComponent,
        pathMatch: 'full'
    },
    
			    {
                    path: 'createOrEdit',
                    component: CreateOrEditCasePoliceReportSummaryComponent,
                    pathMatch: 'full'
                },
			
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CasePoliceReportSummaryRoutingModule {
}

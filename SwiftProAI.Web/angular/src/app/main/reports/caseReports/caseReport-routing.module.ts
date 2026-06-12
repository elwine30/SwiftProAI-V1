import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CaseReportsComponent} from './caseReports.component';



const routes: Routes = [
    {
        path: '',
        component: CaseReportsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseReportRoutingModule {
}

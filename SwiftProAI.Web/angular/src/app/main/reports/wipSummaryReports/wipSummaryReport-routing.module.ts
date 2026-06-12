import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WipSummaryReportComponent } from './wipSummaryReport.component';
// import {WIPSummaryReportsComponent} from './wipSummaryReports.component';

const routes: Routes = [
    {
        path: '',
        component: WipSummaryReportComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WIPSummaryReportRoutingModule {}

import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {WIPReportsComponent} from './wipReports.component';

const routes: Routes = [
    {
        path: '',
        component: WIPReportsComponent,
        pathMatch: 'full'
    },	
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WIPReportRoutingModule {
}

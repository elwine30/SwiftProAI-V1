import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AdjusterReportsComponent} from './adjusterReports.component';



const routes: Routes = [
    {
        path: '',
        component: AdjusterReportsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AdjusterReportRoutingModule {
}

import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceReportsComponent} from './invoiceReports.component';



const routes: Routes = [
    {
        path: '',
        component: InvoiceReportsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceReportRoutingModule {
}

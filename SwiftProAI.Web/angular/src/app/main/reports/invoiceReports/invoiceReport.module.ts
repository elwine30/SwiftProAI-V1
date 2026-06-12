import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {InvoiceReportRoutingModule} from './invoiceReport-routing.module';
import {InvoiceReportsComponent} from './invoiceReports.component';
import {ViewInvoiceReportModalComponent} from './view-invoiceReport-modal.component';



@NgModule({
    declarations: [
        InvoiceReportsComponent,
        ViewInvoiceReportModalComponent,
        
    ],
    imports: [AppSharedModule, InvoiceReportRoutingModule , AdminSharedModule ],
    
})
export class InvoiceReportModule {
}

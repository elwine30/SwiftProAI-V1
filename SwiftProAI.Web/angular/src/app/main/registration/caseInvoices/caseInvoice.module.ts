import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseInvoiceRoutingModule} from './caseInvoice-routing.module';
import {CreateOrEditCaseInvoiceComponent} from './create-or-edit-caseInvoice.component';
import {PreviewCaseInvoiceComponent} from './preview-caseInvoice.component';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { InvoiceItemsComponent } from '../invoiceItems/invoiceItems.component';
import { CreateOrEditInvoiceItemModalComponent } from '../invoiceItems/create-or-edit-invoiceItem-modal.component';



@NgModule({
    declarations: [
        CreateOrEditCaseInvoiceComponent,
        PreviewCaseInvoiceComponent,
        CreateOrEditInvoiceItemModalComponent, 
        InvoiceItemsComponent
        
    ],
    imports: [AppSharedModule, CaseInvoiceRoutingModule , AdminSharedModule ],
    providers: [NumberToWordsPipe]
    
})
export class CaseInvoiceModule {
}

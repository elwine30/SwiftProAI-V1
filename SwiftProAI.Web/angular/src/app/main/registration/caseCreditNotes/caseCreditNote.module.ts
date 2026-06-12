import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseCreditNoteRoutingModule} from './caseCreditNote-routing.module';
import {CreateOrEditCaseCreditNoteComponent} from './create-or-edit-caseCreditNote.component';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { CreateOrEditCreditNoteItemModalComponent } from '../creditNoteItems/create-or-edit-creditNoteItem-modal.component';
import { CreditNoteItemsComponent } from '../creditNoteItems/creditNoteItems.component';
import { PreviewCaseCreditNoteComponent } from './preview-caseCreditNote.component';



@NgModule({
    declarations: [
        CreateOrEditCaseCreditNoteComponent,
        CreateOrEditCreditNoteItemModalComponent,
        PreviewCaseCreditNoteComponent,
        CreditNoteItemsComponent
        
    ],
    imports: [AppSharedModule, CaseCreditNoteRoutingModule , AdminSharedModule ],
    providers: [NumberToWordsPipe]
    
})
export class CaseCreditNoteModule {
}

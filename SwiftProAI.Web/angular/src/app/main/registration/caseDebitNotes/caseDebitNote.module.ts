import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseDebitNoteRoutingModule} from './caseDebitNote-routing.module';
import {CreateOrEditCaseDebitNoteComponent} from './create-or-edit-caseDebitNote.component';
import { PreviewCaseDebitNoteComponent } from './preview-caseDebitNote.component';
import { DebitNoteItemsComponent } from '../debitNoteItems/debitNoteItems.component';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { CreateOrEditDebitNoteItemModalComponent } from '../debitNoteItems/create-or-edit-debitNoteItem-modal.component';



@NgModule({
    declarations: [
        CreateOrEditCaseDebitNoteComponent,
        PreviewCaseDebitNoteComponent,
        CreateOrEditDebitNoteItemModalComponent,
        DebitNoteItemsComponent
        
    ],
    imports: [AppSharedModule, CaseDebitNoteRoutingModule , AdminSharedModule ],
    providers: [NumberToWordsPipe]
    
})
export class CaseDebitNoteModule {
}
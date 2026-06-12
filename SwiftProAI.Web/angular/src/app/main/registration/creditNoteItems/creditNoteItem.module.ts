import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CreditNoteItemRoutingModule} from './creditNoteItem-routing.module';



@NgModule({
    declarations: [
        
    ],
    imports: [AppSharedModule, CreditNoteItemRoutingModule , AdminSharedModule ],
    
})
export class CreditNoteItemModule {
}

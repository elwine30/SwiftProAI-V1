import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {LawFirmRoutingModule} from './lawFirm-routing.module';
import {LawFirmsComponent} from './lawFirms.component';
import {CreateOrEditLawFirmModalComponent} from './create-or-edit-lawFirm-modal.component';
import {ViewLawFirmModalComponent} from './view-lawFirm-modal.component';



@NgModule({
    declarations: [
        LawFirmsComponent,
        CreateOrEditLawFirmModalComponent,
        ViewLawFirmModalComponent,
        
    ],
    imports: [AppSharedModule, LawFirmRoutingModule , AdminSharedModule ],
    
})
export class LawFirmModule {
}

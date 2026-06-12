import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {WorkshopRoutingModule} from './workshop-routing.module';
import {WorkshopsComponent} from './workshops.component';
import {CreateOrEditWorkshopModalComponent} from './create-or-edit-workshop-modal.component';
import {ViewWorkshopModalComponent} from './view-workshop-modal.component';



@NgModule({
    declarations: [
        WorkshopsComponent,
        CreateOrEditWorkshopModalComponent,
        ViewWorkshopModalComponent,
        
    ],
    imports: [AppSharedModule, WorkshopRoutingModule , AdminSharedModule ],
    
})
export class WorkshopModule {
}

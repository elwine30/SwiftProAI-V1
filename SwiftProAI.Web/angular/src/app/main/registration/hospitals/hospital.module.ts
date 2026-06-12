import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {HospitalRoutingModule} from './hospital-routing.module';
import {HospitalsComponent} from './hospitals.component';
import {CreateOrEditHospitalModalComponent} from './create-or-edit-hospital-modal.component';
import {ViewHospitalModalComponent} from './view-hospital-modal.component';



@NgModule({
    declarations: [
        HospitalsComponent,
        CreateOrEditHospitalModalComponent,
        ViewHospitalModalComponent,
        
    ],
    imports: [AppSharedModule, HospitalRoutingModule , AdminSharedModule ],
    
})
export class HospitalModule {
}

import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {VehicleRoutingModule} from './vehicle-routing.module';
import {VehiclesComponent} from './vehicles.component';
import {CreateOrEditVehicleModalComponent} from './create-or-edit-vehicle-modal.component';
import {ViewVehicleModalComponent} from './view-vehicle-modal.component';



@NgModule({
    declarations: [
        VehiclesComponent,
        CreateOrEditVehicleModalComponent,
        ViewVehicleModalComponent,
        
    ],
    imports: [AppSharedModule, VehicleRoutingModule , AdminSharedModule ],
    
})
export class VehicleModule {
}

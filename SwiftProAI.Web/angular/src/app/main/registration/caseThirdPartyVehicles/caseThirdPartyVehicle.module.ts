import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseThirdPartyVehicleRoutingModule} from './caseThirdPartyVehicle-routing.module';
import {CreateOrEditCaseThirdPartyVehicleComponent} from './create-or-edit-caseThirdPartyVehicle.component';
import { ViewCaseThirdPartyVehicleComponent } from './view-caseThirdPartyVehicle.component';
import { ViewcaseThirdPartyVehicleModalComponent } from './view-caseThirdPartyVehicle.modal.component';


@NgModule({
    declarations: [

        CreateOrEditCaseThirdPartyVehicleComponent,ViewCaseThirdPartyVehicleComponent,ViewcaseThirdPartyVehicleModalComponent        
    ],
    imports: [AppSharedModule, CaseThirdPartyVehicleRoutingModule , AdminSharedModule ],
    
})
export class CaseThirdPartyVehicleModule {
}

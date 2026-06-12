import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import { CasetypeRoutingModule } from './casetype-routing.module';
import { CasetypeComponent } from './casetype.component';
import { CreateOrEditCasetypeModalComponent } from './create-or-edit-casetype-modal.component';
import { ViewCasetypeModalComponent } from './view-casetype-modal.component';

@NgModule({
    declarations: [
        CasetypeComponent,
        CreateOrEditCasetypeModalComponent,
        ViewCasetypeModalComponent,
        
    ],
    imports: [AppSharedModule, CasetypeRoutingModule , AdminSharedModule ],
    
})
export class CasetypeModule {
}

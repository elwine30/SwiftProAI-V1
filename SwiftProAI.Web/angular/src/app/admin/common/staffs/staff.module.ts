import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {StaffRoutingModule} from './staff-routing.module';



@NgModule({
    declarations: [
        
    ],
    imports: [AppSharedModule, StaffRoutingModule , AdminSharedModule ],
    
})
export class StaffModule {
}

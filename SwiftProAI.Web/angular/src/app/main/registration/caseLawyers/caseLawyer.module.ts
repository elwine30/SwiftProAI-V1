import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseLawyerRoutingModule} from './caseLawyer-routing.module';
import {CaseLawyersComponent} from './caseLawyers.component'; 					

@NgModule({
    declarations: [
        CaseLawyersComponent,
    ],
    imports: [AppSharedModule, CaseLawyerRoutingModule , AdminSharedModule ],
    
})
export class CaseLawyerModule {
}

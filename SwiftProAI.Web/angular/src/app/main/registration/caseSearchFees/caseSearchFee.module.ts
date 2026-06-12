import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseSearchFeeRoutingModule} from './caseSearchFee-routing.module';




@NgModule({
    declarations: [

    ],
    imports: [AppSharedModule, CaseSearchFeeRoutingModule , AdminSharedModule ],
    
})
export class CaseSearchFeeModule {
}

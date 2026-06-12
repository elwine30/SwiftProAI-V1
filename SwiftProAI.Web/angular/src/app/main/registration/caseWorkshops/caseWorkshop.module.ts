import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseWorkshopRoutingModule} from './caseWorkshop-routing.module';
import {CaseWorkshopsComponent} from './caseWorkshops.component';


@NgModule({
    declarations: [
        CaseWorkshopsComponent,
        ],
    imports: [AppSharedModule, CaseWorkshopRoutingModule , AdminSharedModule ],
    
})
export class CaseWorkshopModule {
}

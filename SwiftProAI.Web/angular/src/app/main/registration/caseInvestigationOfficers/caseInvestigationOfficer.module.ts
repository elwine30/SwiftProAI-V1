import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseInvestigationOfficerRoutingModule} from './caseInvestigationOfficer-routing.module';

@NgModule({
    declarations: [
    ],
    imports: [AppSharedModule, CaseInvestigationOfficerRoutingModule , AdminSharedModule ],
    
})
export class CaseInvestigationOfficerModule {
}

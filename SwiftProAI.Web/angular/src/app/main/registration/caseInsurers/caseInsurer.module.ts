import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseInsurerRoutingModule} from './caseInsurer-routing.module';
import {CaseInsurersComponent} from './caseInsurers.component';
import {CreateOrEditCaseInsurerComponent} from './create-or-edit-caseInsurer.component';



@NgModule({
    declarations: [
        CaseInsurersComponent,
    ],
    imports: [AppSharedModule, CaseInsurerRoutingModule , AdminSharedModule ],
    
})
export class CaseInsurerModule {
}

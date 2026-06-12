import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseStakeholderRoutingModule} from './caseStakeholder-routing.module';
import { CreateOrEditCaseInsurerComponent } from '../caseInsurers/create-or-edit-caseInsurer.component';
import { CreateOrEditCaseStakeholderComponent } from './create-or-edit-caseStakeholder.component';
import { CreateOrEditCaseLawyerComponent } from '../caseLawyers/create-or-edit-caseLawyer.component';
import { CreateOrEditCaseWorkshopComponent } from '../caseWorkshops/create-or-edit-caseWorkshop.component';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { ViewCaseLawyerComponent } from '../caseLawyers/view-caseLawyer.component';
import { ViewCaseStakeholderComponent } from './view-caseStakeHolder.component';
import { ViewCaseInsurerComponent } from '../caseInsurers/view-caseInsurer.component';
import { ViewCaseWorkshopComponent } from '../caseWorkshops/view-caseWorkshop.component';
import { ViewcaseLawyerModalComponent } from '../caseLawyers/view-caseLawyer.modal.component';

@NgModule({
    declarations: [
        CreateOrEditCaseStakeholderComponent,
        CreateOrEditCaseLawyerComponent,
        CreateOrEditCaseInsurerComponent,
        CreateOrEditCaseWorkshopComponent,
        ViewCaseStakeholderComponent,
        ViewCaseLawyerComponent,
        ViewCaseInsurerComponent,
        ViewCaseWorkshopComponent,
        ViewcaseLawyerModalComponent,

        

    ],
    imports: [AccordionModule.forRoot(), AppSharedModule, CaseStakeholderRoutingModule , AdminSharedModule],
    
})
export class CaseStakeholderModule {
}

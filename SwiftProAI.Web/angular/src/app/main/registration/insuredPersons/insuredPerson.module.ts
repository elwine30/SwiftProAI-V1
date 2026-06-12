import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { InsuredPersonRoutingModule } from './insuredPerson-routing.module';
import { CreateOrEditInsuredPersonComponent } from './create-or-edit-insuredPerson.component';
import { ViewInsuredPersonComponent } from './view-insuredPerson.component';

@NgModule({
    declarations: [CreateOrEditInsuredPersonComponent, ViewInsuredPersonComponent],
    imports: [AppSharedModule, InsuredPersonRoutingModule, AdminSharedModule],
})
export class InsuredPersonModule {}

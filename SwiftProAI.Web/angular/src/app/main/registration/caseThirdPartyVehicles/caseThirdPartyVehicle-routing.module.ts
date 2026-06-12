import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseThirdPartyVehicleComponent } from './create-or-edit-caseThirdPartyVehicle.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCaseThirdPartyVehicleComponent } from './view-caseThirdPartyVehicle.component';

const routes: Routes = [

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseThirdPartyVehicleComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },
    {
        path: 'view',
        component: ViewCaseThirdPartyVehicleComponent,
        pathMatch: 'full'
    }


];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseThirdPartyVehicleRoutingModule {
}

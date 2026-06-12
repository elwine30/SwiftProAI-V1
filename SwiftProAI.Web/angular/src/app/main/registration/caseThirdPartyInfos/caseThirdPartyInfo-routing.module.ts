import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CaseThirdPartyInfosComponent } from './caseThirdPartyInfos.component';
import { CreateOrEditCaseThirdPartyInfoComponent } from './create-or-edit-caseThirdPartyInfo.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCaseThirdPartyInfoComponent } from './view-caseThirdPartyInfo.component';

const routes: Routes = [
    {
        path: '',
        component: CaseThirdPartyInfosComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseThirdPartyInfoComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard],
    },
    {
        path: 'view',
        component: ViewCaseThirdPartyInfoComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseThirdPartyInfoRoutingModule {}

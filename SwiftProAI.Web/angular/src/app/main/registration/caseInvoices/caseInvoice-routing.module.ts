import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseInvoiceComponent } from './create-or-edit-caseInvoice.component';
import { PreviewCaseInvoiceComponent } from './preview-caseInvoice.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';

const routes: Routes = [

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseInvoiceComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },


    {
        path: 'preview',
        component: PreviewCaseInvoiceComponent,
        pathMatch: 'full'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseInvoiceRoutingModule {
}

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseDebitNoteComponent } from './create-or-edit-caseDebitNote.component';
import { PreviewCaseDebitNoteComponent } from './preview-caseDebitNote.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';

const routes: Routes = [

    {
        path: 'createOrEdit',
        component: CreateOrEditCaseDebitNoteComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },


    {
        path: 'preview',
        component: PreviewCaseDebitNoteComponent,
        pathMatch: 'full'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseDebitNoteRoutingModule {
}

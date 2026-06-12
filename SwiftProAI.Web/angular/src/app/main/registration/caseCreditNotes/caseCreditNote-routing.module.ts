import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditCaseCreditNoteComponent } from './create-or-edit-caseCreditNote.component';
import { PreviewCaseCreditNoteComponent } from './preview-caseCreditNote.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';

const routes: Routes = [


    {
        path: 'createOrEdit',
        component: CreateOrEditCaseCreditNoteComponent,
        pathMatch: 'full',
        canDeactivate: [DirtyFormGuard]
    },

    {
        path: 'preview',
        component: PreviewCaseCreditNoteComponent,
        pathMatch: 'full'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseCreditNoteRoutingModule {
}

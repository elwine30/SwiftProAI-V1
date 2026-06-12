import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreditNoteItemsComponent} from './creditNoteItems.component';



const routes: Routes = [
    {
        path: '',
        component: CreditNoteItemsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CreditNoteItemRoutingModule {
}

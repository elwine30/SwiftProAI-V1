import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DebitNoteItemsComponent} from './debitNoteItems.component';



const routes: Routes = [
    {
        path: '',
        component: DebitNoteItemsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DebitNoteItemRoutingModule {
}

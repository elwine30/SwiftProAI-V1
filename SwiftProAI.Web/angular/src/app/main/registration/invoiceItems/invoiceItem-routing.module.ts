import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceItemsComponent} from './invoiceItems.component';



const routes: Routes = [
    {
        path: '',
        component: InvoiceItemsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceItemRoutingModule {
}

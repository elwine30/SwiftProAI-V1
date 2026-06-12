import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LawFirmsComponent} from './lawFirms.component';



const routes: Routes = [
    {
        path: '',
        component: LawFirmsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LawFirmRoutingModule {
}

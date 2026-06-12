import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {WorkshopsComponent} from './workshops.component';



const routes: Routes = [
    {
        path: '',
        component: WorkshopsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WorkshopRoutingModule {
}

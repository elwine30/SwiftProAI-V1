import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CaseLawyersComponent} from './caseLawyers.component';
import {CreateOrEditCaseLawyerComponent} from './create-or-edit-caseLawyer.component';
import { ViewCaseLawyerComponent } from './view-caseLawyer.component';

const routes: Routes = [
    {
        path: '',
        component: CaseLawyersComponent,
        pathMatch: 'full'
    },
    
    {
        path: 'createOrEdit',
        component: CreateOrEditCaseLawyerComponent,
        pathMatch: 'full'
    },
    {
        path: 'view',
        component: ViewCaseLawyerComponent,
        pathMatch: 'full'
    },
			
			
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseLawyerRoutingModule {
}

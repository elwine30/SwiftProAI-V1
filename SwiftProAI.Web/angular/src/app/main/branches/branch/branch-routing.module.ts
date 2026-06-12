import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BranchComponent} from './branch.component';
import {CreateOrEditBranchComponent} from './create-or-edit-branch.component';
import {ViewBranchComponent} from './view-branch.component';

const routes: Routes = [
    {
        path: '',
        component: BranchComponent,
        pathMatch: 'full'
    },
    
			    {
                    path: 'createOrEdit',
                    component: CreateOrEditBranchComponent,
                    pathMatch: 'full'
                },
			
    
			    {
                    path: 'view',
                    component: ViewBranchComponent,
                    pathMatch: 'full'
                }
			
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BranchRoutingModule {
}

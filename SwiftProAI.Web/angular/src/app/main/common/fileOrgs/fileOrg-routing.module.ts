import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {FileOrgsComponent} from './fileOrgs.component';



const routes: Routes = [
    {
        path: '',
        component: FileOrgsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FileOrgRoutingModule {
}

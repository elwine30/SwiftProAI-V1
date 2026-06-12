import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CasetypeComponent} from './casetype.component';

const routes: Routes = [
    {
        path: '',
        component: CasetypeComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CasetypeRoutingModule {
}

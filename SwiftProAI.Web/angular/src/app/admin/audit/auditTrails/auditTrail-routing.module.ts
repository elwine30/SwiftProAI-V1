import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuditTrailsComponent} from './auditTrails.component';



const routes: Routes = [
    {
        path: '',
        component: AuditTrailsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AuditTrailRoutingModule {
}

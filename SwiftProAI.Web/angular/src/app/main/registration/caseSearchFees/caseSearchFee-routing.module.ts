import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CaseSearchFeesComponent} from './caseSearchFees.component';



const routes: Routes = [
    {
        path: '',
        component: CaseSearchFeesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseSearchFeeRoutingModule {
}

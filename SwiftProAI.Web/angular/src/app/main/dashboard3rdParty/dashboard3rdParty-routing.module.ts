import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard3rdPartyComponent } from './dashboard3rdParty.component';

const routes: Routes = [
    {
        path: '',
        component: Dashboard3rdPartyComponent,
        pathMatch: 'full',
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Dashboard3rdPartyRoutingModule {}

import { RouterModule, Routes } from "@angular/router";
import { ThirdPartyViewComponent } from "./thirdPartyViewApproval.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: ThirdPartyViewComponent,
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ThirdPartyViewRoutingModule {}
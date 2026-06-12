import { RouterModule, Routes } from "@angular/router";
import { ExpensesClaimApprovalComponent } from "./expenses-claim-approval.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: ExpensesClaimApprovalComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ExpensesClaimApprovalRoutingModule {}
import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { AdminSharedModule } from "../shared/admin-shared.module";
import { ExpensesClaimApprovalRoutingModule } from "./expenses-claim-approval.routing.module";
import { ExpensesClaimApprovalComponent } from "./expenses-claim-approval.component";
import { AppCommonModule } from "@app/shared/common/app-common.module";

@NgModule({
    declarations:[ExpensesClaimApprovalComponent],
    imports: [AppCommonModule, AppSharedModule, AdminSharedModule, ExpensesClaimApprovalRoutingModule]
})
export class ExpensesClaimApprovalModule {}
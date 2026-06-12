import { NgModule } from "@angular/core";
import { ThirdPartyViewComponent } from "./thirdPartyViewApproval.component";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { AdminSharedModule } from "@app/admin/shared/admin-shared.module";
import { ThirdPartyViewRoutingModule } from "./thirdPartyViewApproval-routing.module";
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { NotOnboardDataTableComponent } from "./not-onboard-data-table/not-onboard-data-table.component";
import { OnBoardDataTableComponent } from "./onboard-data-table/onboard-data-table.component";

@NgModule({
    declarations: [ThirdPartyViewComponent, NotOnboardDataTableComponent, OnBoardDataTableComponent],
    imports: [AppCommonModule, AppSharedModule, AdminSharedModule, ThirdPartyViewRoutingModule]
})
export class ThirdPartyViewApprovalModule {}
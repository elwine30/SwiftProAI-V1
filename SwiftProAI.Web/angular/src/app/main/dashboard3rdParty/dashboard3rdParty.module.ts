import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { Dashboard3rdPartyComponent } from './dashboard3rdParty.component';
import { Dashboard3rdPartyRoutingModule } from './dashboard3rdParty-routing.module';

@NgModule({
    declarations: [Dashboard3rdPartyComponent],
    imports: [AppSharedModule, AdminSharedModule, Dashboard3rdPartyRoutingModule],
})
export class Dashboard3rdPartyModule {}

import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {AuditTrailRoutingModule} from './auditTrail-routing.module';
import {AuditTrailsComponent} from './auditTrails.component';
import {CreateOrEditAuditTrailModalComponent} from './create-or-edit-auditTrail-modal.component';
import {ViewAuditTrailModalComponent} from './view-auditTrail-modal.component';



@NgModule({
    declarations: [
        AuditTrailsComponent,
        CreateOrEditAuditTrailModalComponent,
        ViewAuditTrailModalComponent,
        
    ],
    imports: [AppSharedModule, AuditTrailRoutingModule , AdminSharedModule ],
    
})
export class AuditTrailModule {
}

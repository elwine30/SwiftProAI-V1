import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ScopeAssignmentRoutingModule} from './scopeAssignment-routing.module';
import {ScopeAssignmentsComponent} from './scopeAssignments.component';
import {CreateOrEditScopeAssignmentModalComponent} from './create-or-edit-scopeAssignment-modal.component';
import {ViewScopeAssignmentModalComponent} from './view-scopeAssignment-modal.component';



@NgModule({
    declarations: [
        ScopeAssignmentsComponent,
        CreateOrEditScopeAssignmentModalComponent,
        ViewScopeAssignmentModalComponent,
        
    ],
    imports: [AppSharedModule, ScopeAssignmentRoutingModule , AdminSharedModule ],
    
})
export class ScopeAssignmentModule {
}

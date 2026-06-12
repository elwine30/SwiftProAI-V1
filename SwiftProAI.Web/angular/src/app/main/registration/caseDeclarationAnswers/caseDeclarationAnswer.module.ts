import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CaseDeclarationAnswerRoutingModule} from './caseDeclarationAnswer-routing.module';



@NgModule({
    declarations: [
        
    ],
    imports: [AppSharedModule, CaseDeclarationAnswerRoutingModule , AdminSharedModule ],
    
})
export class CaseDeclarationAnswerModule {
}

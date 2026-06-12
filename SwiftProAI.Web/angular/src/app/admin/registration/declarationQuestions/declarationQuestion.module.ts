import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {DeclarationQuestionRoutingModule} from './declarationQuestion-routing.module';
import {DeclarationQuestionsComponent} from './declarationQuestions.component';
import {CreateOrEditDeclarationQuestionModalComponent} from './create-or-edit-declarationQuestion-modal.component';
import {ViewDeclarationQuestionModalComponent} from './view-declarationQuestion-modal.component';



@NgModule({
    declarations: [
        DeclarationQuestionsComponent,
        CreateOrEditDeclarationQuestionModalComponent,
        ViewDeclarationQuestionModalComponent,
        
    ],
    imports: [AppSharedModule, DeclarationQuestionRoutingModule , AdminSharedModule ],
    
})
export class DeclarationQuestionModule {
}

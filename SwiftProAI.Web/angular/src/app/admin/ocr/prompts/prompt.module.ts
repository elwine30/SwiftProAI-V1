import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {PromptRoutingModule} from './prompt-routing.module';
import {PromptsComponent} from './prompts.component';
import {CreateOrEditPromptModalComponent} from './create-or-edit-prompt-modal.component';
import {ViewPromptModalComponent} from './view-prompt-modal.component';



@NgModule({
    declarations: [
        PromptsComponent,
        CreateOrEditPromptModalComponent,
        ViewPromptModalComponent,
        
    ],
    imports: [AppSharedModule, PromptRoutingModule , AdminSharedModule ],
    
})
export class PromptModule {
}

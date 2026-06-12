import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {OpenAIIntegrationLogRoutingModule} from './openAIIntegrationLog-routing.module';
import {OpenAIIntegrationLogsComponent} from './openAIIntegrationLogs.component';
import {ViewOpenAIIntegrationLogModalComponent} from './view-openAIIntegrationLog-modal.component';



@NgModule({
    declarations: [
        OpenAIIntegrationLogsComponent,
        ViewOpenAIIntegrationLogModalComponent,
        
    ],
    imports: [AppSharedModule, OpenAIIntegrationLogRoutingModule , AdminSharedModule ],
    
})
export class OpenAIIntegrationLogModule {
}

import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {OpenAIIntegrationLogsComponent} from './openAIIntegrationLogs.component';



const routes: Routes = [
    {
        path: '',
        component: OpenAIIntegrationLogsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OpenAIIntegrationLogRoutingModule {
}

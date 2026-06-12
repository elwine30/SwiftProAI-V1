import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DocumentSettingsComponent} from './documentSettings.component';
import {CreateOrEditDocumentSettingComponent} from './create-or-edit-documentSetting.component';
import {ViewDocumentSettingComponent} from './view-documentSetting.component';

const routes: Routes = [
    {
        path: '',
        component: CreateOrEditDocumentSettingComponent,
        pathMatch: 'full'
    },
    
			    {
                    path: 'view',
                    component: ViewDocumentSettingComponent,
                    pathMatch: 'full'
                }
			
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DocumentSettingRoutingModule {
}

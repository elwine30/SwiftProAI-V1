import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {DocumentSettingRoutingModule} from './documentSetting-routing.module';
import {DocumentSettingsComponent} from './documentSettings.component';
import {CreateOrEditDocumentSettingComponent} from './create-or-edit-documentSetting.component';
import {ViewDocumentSettingComponent} from './view-documentSetting.component';



@NgModule({
    declarations: [
        DocumentSettingsComponent,
        CreateOrEditDocumentSettingComponent,
        ViewDocumentSettingComponent,
        
    ],
    imports: [AppSharedModule, DocumentSettingRoutingModule , AdminSharedModule ],
    
})
export class DocumentSettingModule {
}

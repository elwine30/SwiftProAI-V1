import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { FileOrgRoutingModule } from './fileOrg-routing.module';
import { FileOrgsComponent } from './fileOrgs.component';
import { FileViewerModalComponent } from './file-viewer-modal.component';
import { FileUploadModalComponent } from './file-upload-modal.component';
// import {CreateOrEditFileOrgModalComponent} from './create-or-edit-fileOrg-modal.component';
// import {ViewFileOrgModalComponent} from './view-fileOrg-modal.component';
// import {FileOrgMainRegistrationLookupTableModalComponent} from './fileOrg-mainRegistration-lookup-table-modal.component';
//     					import {FileOrgFolderLookupTableModalComponent} from './fileOrg-folder-lookup-table-modal.component';

@NgModule({
    declarations: [FileOrgsComponent, FileViewerModalComponent, FileUploadModalComponent],
    imports: [AppSharedModule, FileOrgRoutingModule, AdminSharedModule],
})
export class FileOrgModule {}

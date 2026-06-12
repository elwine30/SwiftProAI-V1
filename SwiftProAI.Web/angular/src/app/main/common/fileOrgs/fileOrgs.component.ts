import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
    FileOrgsServiceProxy,
    FoldersServiceProxy,
    FileViewInputByFolderAndCase,
    FileViewDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileViewerModalComponent } from './file-viewer-modal.component';
import { FileUploadModalComponent } from './file-upload-modal.component';

@Component({
    templateUrl: './fileOrgs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styleUrls: ['./fileOrgs.component.css'],
})
export class FileOrgsComponent extends AppComponentBase implements OnInit {
    @ViewChild('fileViewerModal', { static: false }) fileViewerModal: FileViewerModalComponent;
    @ViewChild('fileUploadModal', { static: false }) fileUploadModal: FileUploadModalComponent;

    registerId: number;
    mainRegistrations = [];
    filesList = [];
    fileSelected: FileViewDto = new FileViewDto();

    selectedField: any;

    editedName: any;

    fileViewInput: FileViewInputByFolderAndCase = undefined;

    isFileViewOnly: boolean = false;

    constructor(
        injector: Injector,
        private _fileOrgsServiceProxy: FileOrgsServiceProxy,
        private _folderServiceProxy: FoldersServiceProxy,
        private _activatedRoute: ActivatedRoute,
    ) {
        super(injector);
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
    }

    ngOnInit(): void {
        this.loadFolder();
    }

    //#region FileOrganzer Side Tree Component
    // Expand Folder View
    toggle(entity: any) {
        console.log('Toggling entity:', entity);
        if (!entity) {
            console.error('Entity is undefined');
            return;
        }
        entity.expanded = !entity.expanded;
        console.log('Entity after toggle:', entity);
    }

    loadFolder() {
        this._folderServiceProxy.getAllInDictionary(this.registerId).subscribe((result) => {
            var data = [
                {
                    name: this.registerId,
                    entities: Object.keys(result).map((key) => {
                        return {
                            name: key,
                            fields: Object.keys(result[key]).map((field) => ({
                                name: field,
                                id: result[key][field],
                            })),
                            expanded: false,
                        };
                    }),
                    expanded: false,
                },
            ];

            console.log(data);
            this.mainRegistrations = data;
        });
    }

    selectField(field: { name: string; id: number }) {
        this._fileOrgsServiceProxy
            .getMetadataByFolderAndCase(field.id, this.registerId.toString(), undefined)
            .subscribe((result) => {
                this.filesList = result;
            });
        this.selectedField = field;
    }
    //#endregion

    deleteFile(file: any) {
        console.log('Deleting file:', file);
        this._fileOrgsServiceProxy.deleteFileByReference(file.referenceNo).subscribe(() => {});
    }

    uploadFile(field: any) {
        console.log('Uploading file:', field.fileName);
        this.fileUploadModal.open(field);
    }
    viewFile(file: any) {
        console.log('Viewing file:', file);
        this.fileViewerModal.show(file.referenceNo);
    }

    //Trigger edit state
    editFile(file: any) {
        file.isEditing = true;
        file.originalName = file.fileName; //Original name to revert when cancel edit is triggered
        this.editedName = file.fileName;
    }
    //Trigger update api
    saveFileName(file: any) {
        file.isEditing = false;
        console.log('editedName', this.editedName);
        console.log('filename', file.fileName);
        this._fileOrgsServiceProxy.renameFileByReference(file.referenceNo, file.fileName).subscribe((result) => {
            console.log('result', result);
            file.fileName = result;
        });
    }
    //Cancel edit state
    cancelEdit(file: any) {
        file.isEditing = false;
        file.fileName = file.originalName;
    }

    //#region File Styling Functions (Icon and color)

    getFileTypeIcon(fileType: string): string {
        switch (fileType.toLowerCase()) {
            case 'image':
                return 'fa-file-image ';
            case 'pdf':
                return 'fa-file-pdf ';
            case 'word':
                return 'fa-file-word ';
            case 'excel':
                return 'fa-file-excel';
            default:
                return 'fa-file ';
        }
    }

    getFileTypeColor(fileType: string): any {
        switch (fileType.toLowerCase()) {
            case 'image':
                return { color: '#f39c12' };
            case 'pdf':
                return { color: '#e74c3c' };
            case 'word':
                return { color: '#3498db' };
            case 'excel':
                return { color: '#2ecc71' };
            default:
                return { color: '#95a5a6' };
        }
    }
    //#endregion
}

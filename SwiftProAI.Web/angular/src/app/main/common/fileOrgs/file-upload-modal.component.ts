import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    FileMetadataDto,
    FileOrgDto,
    FileOrgsServiceProxy,
    FileUploadInput,
} from '@shared/service-proxies/service-proxies';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'file-upload-modal',
    templateUrl: './file-upload-modal.component.html',
    styleUrls: ['./file-upload-modal.component.css'],
})
export class FileUploadModalComponent extends AppComponentBase {
    @Output() fileUploaded = new EventEmitter<FileMetadataDto>();

    @ViewChild('fileUploadModal') public fileUploadModal: ModalDirective;

    uploader: FileUploader = new FileUploader({
        url: 'your-upload-url',
        itemAlias: 'file',
    });

    previewUrl: SafeResourceUrl;
    fileType: string;
    selectedFile: File;
    selectedField: any;

    constructor(
        injector: Injector,
        private _fileOrgsServiceProxy: FileOrgsServiceProxy,
        private sanitizer: DomSanitizer,
        private _activatedRoute: ActivatedRoute,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.uploader.onAfterAddingFile = (fileItem: FileItem) => {
            fileItem.withCredentials = false;
            this.handleFile(fileItem._file);
        };

        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
            console.log('File uploaded:', item.file.name);
            this.fileUploaded.emit(item.file);
            this.reset();
        };
    }

    handleFileInput(event: any): void {
        const file = event.target.files[0];
        this.handleFile(file);
    }

    handleFile(file: File): void {
        this.previewUrl = null;
        this.fileType = null;

        console.log('File Type', file);
        this.selectedFile = file;
        if (file.type.startsWith('image')) {
            this.fileType = 'image';
            this.previewFile(file);
        } else if (file.type === 'application/pdf') {
            this.fileType = 'application/pdf';
            this.previewFile(file);
        } else {
            console.log('File type not supported for preview:', file.type);
        }
    }

    previewFile(file: File): void {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            console.log('preview url', reader.result);
            this.previewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(reader.result as string);
        };
    }

    async uploadFile(): Promise<void> {
        if (!this.selectedFile) {
            console.error('No file selected for upload');
            return;
        }

        const fileReader = new FileReader();
        fileReader.readAsDataURL(this.selectedFile);
        fileReader.onload = () => {
            const base64Content = (fileReader.result as string).split(',')[1];

            const fileInput: FileUploadInput = new FileUploadInput();
            fileInput.fileOrgOb = new FileOrgDto();
            fileInput.fileOrgOb.fileName = this.selectedFile.name;
            fileInput.fileOrgOb.folderId = this.selectedField.id;
            fileInput.fileOrgOb.mainRegistrationId = this._activatedRoute.snapshot.queryParams['id'];

            fileInput.contentType = this.selectedFile.type;
            fileInput.fileContent = base64Content;

            this._fileOrgsServiceProxy.uploadFile(fileInput).subscribe(
                (result) => {
                    console.log('File successfully uploaded');
                    this.fileUploaded.emit(result);
                    this.reset();
                },
                (error) => {
                    console.error('Error uploading file:', error);
                },
            );
        };
    }

    open(selectedField: any): void {
        this.fileUploadModal.show();
        this.selectedField = selectedField;
    }

    close(): void {
        this.fileUploadModal.hide();
        this.reset();
    }

    reset(): void {
        this.previewUrl = null;
        this.fileType = null;
        this.selectedField = null;
        this.uploader.clearQueue();
    }

    onDrop(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        const files = event.dataTransfer?.files;
        if (files && files.length > 0) {
            this.handleFile(files[0]);
        }
    }

    onDragOver(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
    }

    onDragLeave(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
    }
}

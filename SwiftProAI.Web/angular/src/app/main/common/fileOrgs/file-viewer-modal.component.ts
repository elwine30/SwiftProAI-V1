import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileOrgsServiceProxy, FileViewDto, FileViewInputByReference } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'file-viewer-modal',
    templateUrl: './file-viewer-modal.component.html',
    styleUrls: ['./file-viewer-modal.component.css'],
})
export class FileViewerModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('fileViewerModal', { static: false }) public fileViewerModal: ModalDirective;

    fileInput: FileViewInputByReference = new FileViewInputByReference();
    fileView: FileViewDto = new FileViewDto();
    fileUrl: any;

    constructor(
        injector: Injector,
        private _fileOrgsServiceProxy: FileOrgsServiceProxy,
        private sanitizer: DomSanitizer,
    ) {
        super(injector);
    }

    ngOnInit() {}

    downloadFile(): void {
        if (this.fileUrl) {
            const anchor = document.createElement('a');
            anchor.href = this.fileUrl.changingThisBreaksApplicationSecurity; // Access the actual URL
            anchor.download = this.fileView.fileName;
            anchor.click();
        }
    }

    show(referenceNo: string): void {
        this.fileViewerModal.show();
        this.fileInput.referenceNo = referenceNo;
        this._fileOrgsServiceProxy.viewFileByReference(this.fileInput).subscribe(
            (result) => {
                this.fileView = result;

                const byteCharacters = atob(this.fileView.fileContent);
                const byteNumbers = new Array(byteCharacters.length);
                for (let i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                const byteArray = new Uint8Array(byteNumbers);

                let blob = new Blob([byteArray], {
                    type: this.fileView.contentType == 'PDF' ? 'application/pdf' : this.fileView.contentType,
                });

                this.fileUrl = this.sanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(blob));
            },
            (error) => {
                console.error('Error viewing file:', error);
            },
        );
    }

    close(): void {
        this.fileViewerModal.hide();
        if (this.fileUrl) {
            URL.revokeObjectURL(this.fileUrl.changingThisBreaksApplicationSecurity);
        }
    }
}

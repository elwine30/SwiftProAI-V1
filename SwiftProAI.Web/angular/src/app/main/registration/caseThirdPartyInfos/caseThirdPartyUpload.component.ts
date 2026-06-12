import {
    ChangeDetectorRef,
    Component,
    ElementRef,
    EventEmitter,
    Injector,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import Swal from 'sweetalert2';
import { T } from '@fullcalendar/core/internal-common';
import { CaseThirdPartyInfosServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'caseThirdPartyUpload',
    templateUrl: './caseThirdPartyUpload.component.html',
    styleUrls: ['./caseThirdPartyUpload.component.css'],
})
export class CaseThirdPartyUploadComponent extends AppComponentBase implements OnInit {
    @Input() fileUploadType: string;
    @Input() registerId: number;
    @Input() TpiFileUploadFileToken: string;
    @Input() TpiFileUploadFileName: string;
    @Input() isTPIFileSuccessfullyUploaded: boolean;
    @Input() ThirdPartyInfoId?: number;
    @Output() fileTokenEmitter = new EventEmitter<T>();
    TpiUploader: FileUploader;
    @ViewChild('tpiFileInput', { static: true }) TpiFileInput: ElementRef;

    TpiFileUploadFileAcceptedTypes: string = '';

    constructor(
        injector: Injector,
        private _tokenService: TokenService,
        private _http: HttpClient,
        private _thirdPartyInfo: CaseThirdPartyInfosServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.TpiFileUploadFileAcceptedTypes += '.' + list[i] + ',';
            }
        });

        this.TpiUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl +
                `/fileOcr/UploadFile?caseId=${this.registerId}&docTypeValue=${this.fileUploadType}`,
            (content) => {
                this.TpiFileUploadFileToken = content.fileToken;
                this.fileTokenEmitter.emit(content);
            },
        );
    }

    initializeUploader(url: string, onSuccess: (fileToken: any) => void): FileUploader {
        let uploader = new FileUploader({ url: url });

        let _uploaderOptions: FileUploaderOptions = { url: url };
        _uploaderOptions.autoUpload = false;
        _uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        _uploaderOptions.removeAfterUpload = true;

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);
            var newResult;
            if (resp.success && resp.result.fileToken) {
                if (resp.result.output) {
                    var resData = JSON.parse(resp.result.output.replaceAll('**', ''));
                    var extractedString = resData.choices[0].message.content.match(/\{[\s\S]*\}/);
                    var content = JSON.parse(extractedString); // could be null as some documents dont need ocr

                    newResult = {
                        fileToken: resp.result.fileToken,
                        content: content,
                    };
                } else {
                    newResult = {
                        fileToken: resp.result.fileToken,
                    };
                }

                onSuccess(newResult);
            } else {
                this.message.error(resp.result.message);
            }
        };

        uploader.onErrorItem = (item, response, status, headers) => {
            let errorMessage = this.l('AnErrorOccurredDuringFileUpload');
            try {
                const resp = JSON.parse(response);
                if (resp && resp.error && resp.error.message) {
                    errorMessage = resp.error.message;
                }
            } catch (e) {
                errorMessage = this.l('Extraction Failed');
            }

            Swal.fire({
                icon: 'error',
                title: this.l('Extraction Failed'),
                text: errorMessage,
            });
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    clearQueue() {
        console.log('Queue before:', this.TpiUploader.queue);

        this.TpiUploader.clearQueue();

        this.TpiFileUploadFileName = null;
        this.isTPIFileSuccessfullyUploaded = false;

        console.log('Queue cleared:', this.TpiUploader.queue);

        this.TpiUploader.queue.length = 0;
        this.TpiFileInput.nativeElement.value = null;
        console.log('Queue cleared and UI updated');
    }
    onSelectTPIFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.TpiUploader.clearQueue();
        this.TpiUploader.addToQueue([selectedFile]);
        this.TpiUploader.uploadAll();

        if (this.fileUploadType != null) {
            this.swalAlert.fire({
                title: this.l('UploadingAndExtractingContent'),
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    }

    removeFileUploadFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                // Currently hardcoded
                this._thirdPartyInfo
                    .removeTpiFile(
                        this.TpiFileUploadFileToken,
                        // , this.ThirdPartyInfoId, this.fileUploadType
                    )
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.TpiFileUploadFileName = null;
                    });
            }
        });
    }
}

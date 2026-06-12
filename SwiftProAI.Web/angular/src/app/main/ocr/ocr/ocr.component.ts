import { Component, Injector, OnInit } from '@angular/core';
import { chatGPTOCROutput } from '@app/shared/model/model.item';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import Swal from 'sweetalert2';

@Component({
    selector: 'app-ocr',
    templateUrl: './ocr.component.html',
    styleUrl: './ocr.component.css',
    animations: [appModuleAnimation()],
})
export class OcrComponent extends AppComponentBase implements OnInit {
    uploadedFile: FileItem = null;
    ocrUploader: FileUploader;
    invalidFileSize = false;
    uploading = false;
    result: any;
    ocr: chatGPTOCROutput = new chatGPTOCROutput();
    swalAlert = Swal;

    constructor(
        injector: Injector,
        private _tokenService: TokenService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.initUploader();
    }

    initUploader() {
        this.ocrUploader = this.createUploader('/api/services/OCR/UploadPoliceReport', (result) => {
            this.result = JSON.parse(result.result.replaceAll('**', ''));

            if (result == null) {
                this.message.error(this.l('EmptyReturnedResultMessage'));

                this.uploadedFile = null;
                this.uploading = false;
                return;
            }

            // remove any ```json string before the content if chatgpt happen to return such format
            // most of the times it will return without any string before the object
            // this is just a precaution for the scenario
            // example "content": "```json\n{\n  \"policeStation\": \"Trafik Johor Bahru\"}```"
            // this extra string will cause error when parsing into JSON

            var extractedString = this.result.choices[0].message.content.match(/\{[\s\S]*\}/);

            var content = JSON.parse(extractedString);

            if (content == null) {
                this.message.error(this.l('EmptyReturnedContentMessage'));

                this.uploadedFile = null;
                this.uploading = false;
                return;
            }

            this.ocr.policeStation = content.policeStation;
            this.ocr.officerName = content.officerName;
            this.ocr.officerServiceNo = content.officerServiceNo;
            this.ocr.reportNo = content.reportNo;
            this.ocr.incidentDateTime = content.incidentDatetime;
            this.ocr.reporterName = content.reporterName;
            this.ocr.organizationName = content.organizationName;
            this.ocr.incidentLocation = content.incidentLocation;
            this.ocr.summaryOfIncident = content.summaryOfIncident;
            this.ocr.natureOfIncident = content.natureOfIncident;
            this.ocr.vehicleRegistrationNumbers =
                content.vehicleRegistrationNumbers != null ? content.vehicleRegistrationNumbers.join(', ') : null;
            this.ocr.vehicleModels = content.vehicleModels != null ? content.vehicleModels.join(', ') : null;
            this.ocr.vehicleColours = content.vehicleColours != null ? content.vehicleColours.join(', ') : null;
            this.ocr.summary = content.summaryOfIncidentReported;

            this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
        });
    }

    createUploader(url: string, success?: (result: any) => void): FileUploader {
        var token = 'Bearer ' + this._tokenService.getToken();
        const uploaderOptions: FileUploaderOptions = {
            url: AppConsts.remoteServiceBaseUrl + url,
            removeAfterUpload: true,
            authToken: token,
        };
        const uploader = new FileUploader(uploaderOptions);

        uploader.onAfterAddingFile = (file) => {
            this.checkFiles(file);
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const ajaxResponse = <IAjaxResponse>JSON.parse(response);
            if (ajaxResponse.success) {
                if (success) {
                    success(ajaxResponse.result);
                }
            } else {
                this.message.error(ajaxResponse.error.message);
            }
            document.getElementById('ocrUploader')['value'] = ''; //clear value to upload again
            this.uploadedFile = null;
            this.uploading = false;
        };

        return uploader;
    }

    checkFiles(file: FileItem) {
        const img = new Image();

        this.uploadedFile = file;

        //! Validations handled in BE

        // if (file._file.size > 10 * 1024 * 1024) {
        //     uploader.removeFromQueue(file);
        //     return;
        // }

        img.src = URL.createObjectURL(this.ocrUploader.queue[this.ocrUploader.queue.length - 1]._file);
    }

    uploadLogo() {
        this.uploading = true;
        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
        this.ocrUploader.uploadAll();
    }

    clearLogo() {
        this.invalidFileSize = false;
        this.uploadedFile = null;
        document.getElementById('ocrUploader')['value'] = '';
    }
}

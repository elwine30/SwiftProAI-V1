import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CasePoliceReportsServiceProxy,
    CreateOrEditCasePoliceReportDto,
    CasePoliceReportMainRegistrationLookupTableDto,
    CasePoliceReportDto,
    ChartDateInterval,
    GetLookupByGroupDto,
    LookupsServiceProxy,
    GetCasePoliceReportForEditOutput,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { HttpClient } from '@angular/common/http';
import { DateTime } from 'luxon';
import Swal from 'sweetalert2';

@Component({
    selector: 'casePoliceReportFileUploadModal',
    templateUrl: './casePoliceReport-file-upload-modal.component.html',
})
export class CasePoliceReportFileUploadModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    registerId: number;

    casePoliceReport: CreateOrEditCasePoliceReportDto = new CreateOrEditCasePoliceReportDto();
    reportFileUploadFileUploader: FileUploader;
    reportFileUploadFileToken: string;
    reportFileUploadFileName: string;
    reportFileUploadFileAcceptedTypes: string = '';
    reportFileUploadFileUploaded: boolean;

    uploadedFile: FileItem = null;
    invalidFileSize = false;
    uploading = false;
    result: any;
    types: GetLookupByGroupDto[];

    mainRegistrationVehicleNo = '';
    swalAlert = Swal;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _casePoliceReportsServiceProxy: CasePoliceReportsServiceProxy,
        private _tokenService: TokenService,
        private _http: HttpClient,
        private _lookupService: LookupsServiceProxy,
    ) {
        super(injector);
    }

    show(result?: GetCasePoliceReportForEditOutput, registerId?: number): void {
        console.log('this.reportFileUploadFileName', this.reportFileUploadFileName);
        console.log('this.reportFileUploadFileUploadeds before', this.reportFileUploadFileUploaded);
        this.casePoliceReport = result.casePoliceReport;
        this.reportFileUploadFileName = result.reportFileUploadFileName;
        if (!this.casePoliceReport) {
            this.casePoliceReport = new CreateOrEditCasePoliceReportDto();
            this.reportFileUploadFileName = null;
        }
        this.casePoliceReport.registerId = registerId;

        this.reportFileUploadFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=PoliceReport`,
            ({ content, fileToken }) => {
                if (content) {
                    this.casePoliceReport.reportFileUploadToken = fileToken;
                    this.casePoliceReport.policeStation = content.policeStation;
                    this.casePoliceReport.ipd = content.policeStation;
                    this.casePoliceReport.vehicleNo =
                        content.vehicleRegistrationNumbers != null
                            ? content.vehicleRegistrationNumbers.join(', ')
                            : null;
                    this.casePoliceReport.reportNo = content.reportNo;
                    this.casePoliceReport.officerName = content.officerName;
                    this.casePoliceReport.serviceNo = content.officerServiceNo;
                    this.casePoliceReport.complainantIdentityNo = content.identityNo;
                    this.casePoliceReport.statement = content.summaryOfIncidentReported;

                    //Format dateTime string to remove AM/PM as it conflicts with converting to Datetime object. Sometimes returns 24 hour time with AM/PM (14:02 PM)
                    content.reportDatetime = content.reportDatetime.replace(/\b(?:am|pm)\b/i, '').trim();
                    content.incidentDatetime = content.incidentDatetime.replace(/\b(?:am|pm)\b/i, '').trim();
                    this.casePoliceReport.reportTime = DateTime.fromFormat(content.reportDatetime, 'dd/MM/yyyy HH:mm');
                    this.casePoliceReport.incidentTime = DateTime.fromFormat(
                        content.incidentDatetime,
                        'dd/MM/yyyy HH:mm',
                    );
                    this.reportFileUploadFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } else {
                    this.message.error(
                        'Failed to extract from report. Please upload a new police report and try again.',
                    );
                }
            },
        );

        this.active = true;
        this.modal.show();
        console.log('this.reportFileUploadFileName', this.reportFileUploadFileName);
        console.log('this.uploadedFile', this.reportFileUploadFileUploaded);
    }

    initializeUploader(url: string, onSuccess: (result: any) => void): FileUploader {
        let uploader = new FileUploader({ url: url });
        var token = 'Bearer ' + this._tokenService.getToken();

        let _uploaderOptions: FileUploaderOptions = { url: url };
        _uploaderOptions.autoUpload = false;
        _uploaderOptions.authToken = token;
        _uploaderOptions.removeAfterUpload = false;

        uploader.onAfterAddingFile = (file) => {
            //! Validations handled in BE

            // if (file._file.size > 10 * 1024 * 1024) {
            //     uploader.removeFromQueue(file);
            //     return;
            // }

            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);

            if (!resp.success) this.message.error(resp.error.message);

            const fileToken = resp.result.fileToken;

            if (resp.result.output) {
                var resData = JSON.parse(resp.result.output.replaceAll('**', ''));
                var extractedString = resData.choices[0].message.content.match(/\{[\s\S]*\}/);
                var content = JSON.parse(extractedString); // could be null as some documents dont need ocr
                onSuccess({ fileToken, content });
            } else {
                onSuccess(fileToken);
            }
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    onSelectReportFileUploadFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.reportFileUploadFileUploader.clearQueue();
        this.reportFileUploadFileUploader.addToQueue([selectedFile]);
        this.reportFileUploadFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    resetUploader() {
        this.reportFileUploadFileUploader.clearQueue();

        this.reportFileUploadFileUploaded = false;
        this.reportFileUploadFileName = null;
        this.casePoliceReport.reportFileUploadToken = null;
        console.log('this.uploadedFile', this.reportFileUploadFileUploaded);
    }

    removeReportFileUploadFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._casePoliceReportsServiceProxy
                    .removeReportFileUploadFile(this.casePoliceReport.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.reportFileUploadFileName = null;
                    });
            }
        });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this._lookupService.getByGroup('InvolvedPartyType').subscribe((result) => {
            this.types = result;
        });
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.reportFileUploadFileAcceptedTypes += '.' + list[i] + ',';
            }
        });
    }

    close(): void {
        this.active = false;
        this.modalSave.emit(this.casePoliceReport);
        this.modal.hide();
    }
}

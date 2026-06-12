import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef, Input } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseThirdPartyVehiclesServiceProxy,
    CreateOrEditCaseThirdPartyVehicleDto,
    CaseThirdPartyVehicleMainRegistrationLookupTableDto,
    CaseThirdPartyVehicleDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    FileMetadataDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { AppConsts } from '@shared/AppConsts';
import { result } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';

@Component({
    templateUrl: './create-or-edit-caseThirdPartyVehicle.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseThirdPartyVehicleComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;

    caseThirdPartyVehicle: CreateOrEditCaseThirdPartyVehicleDto = new CreateOrEditCaseThirdPartyVehicleDto();
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('caseThirdPartyVehicleForm') caseThirdPartyVehicleForm: NgForm;
    @ViewChild('fileUploadModal', { static: true }) fileUploadModal: ModalDirective;

    @ViewChild('InsuredPerson_driverCarGrantLabel') insuredPerson_driverCarGrantLabel: ElementRef;

    @ViewChild('tpvDetFileInput', { static: true }) TPVDetFileInput: ElementRef;
    @ViewChild('tpvGrantFileInput', { static: true }) TPVGrantFileInput: ElementRef;

    driverCarGrantFileUploaded: boolean;

    mainRegistrationVehicleNo = '';
    companyName = '';

    advancedFiltersAreShown = false;
    registerIdFilter = '';
    caseThirdPartyVehicleIdNumber: number;

    allMainRegistrations: CaseThirdPartyVehicleMainRegistrationLookupTableDto[];
    allCompanys: CommonDropdownDto[];

    filesSaved: FileMetadataDto;
    allMakers: CommonDropdownDto[];

    types = [
        { value: 'Comprehensive' },
        { value: 'Third Party Fire and Theft' },
        { value: 'Third Party' },
        { value: 'Act Cover' },
        { value: 'NONE' },
    ];

    makes = [
        { value: 'PROTON' },
        { value: 'PERODUA' },
        { value: 'TOYOTA' },
        { value: 'HONDA' },
        { value: 'BMW' },
        { value: 'MERCEDES-BENZ' },
        { value: 'AUDI' },
        { value: 'VOLKSWAGEN' },
        { value: 'FORD' },
        { value: 'CHEVROLET' },
        { value: 'NISSAN' },
        { value: 'MAZDA' },
        { value: 'MITSUBISHI' },
        { value: 'CHERY' },
        { value: 'SUBARU' },
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseThirdPartyVehiclesServiceProxy: CaseThirdPartyVehiclesServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        public navigationService: NavigationService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _tokenService: TokenService,
        private _http: HttpClient,
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.navigationService.registerId = this.registerIdFilter;

        this.show();

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe((result) => {
            this.allCompanys = result;
        });
    }
    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseThirdPartyVehicleForm && this.caseThirdPartyVehicleForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseThirdPartyVehicleForm && this.caseThirdPartyVehicleForm.controls) {
            Object.keys(this.caseThirdPartyVehicleForm.controls).forEach((controlName) => {
                const control = this.caseThirdPartyVehicleForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    show(caseThirdPartyVehicleId?: number): void {
        this._caseThirdPartyVehiclesServiceProxy
            .getCaseThirdPartyVehicleForEdit(caseThirdPartyVehicleId)
            .subscribe((result) => {
                this.caseThirdPartyVehicle = result.caseThirdPartyVehicle;
                if (!this.caseThirdPartyVehicle) {
                    this.caseThirdPartyVehicle = new CreateOrEditCaseThirdPartyVehicleDto();
                    this.companyName = '';
                }
                this.companyName = result.companyName;
                this.caseThirdPartyVehicle.registerId = this._activatedRoute.snapshot.queryParams['id'];
                this.active = true;

                this.driverCarGrantFileName =
                    this.caseThirdPartyVehicle.tpvCarGrant == null
                        ? null
                        : this.caseThirdPartyVehicle.tpvCarGrant.fileName;
                this.TpvFileUploadFileName =
                    this.caseThirdPartyVehicle.tpvDetails == null
                        ? null
                        : this.caseThirdPartyVehicle.tpvDetails.fileName;
                this.markFormAsPristine(this.caseThirdPartyVehicleForm);
            });

        this._CommonDropdownService.getAllMakerVehicle().subscribe((result) => {
            this.allMakers = result;
        });
    }

    saveAndNew(): void {
        this.saving = true;

        this.caseThirdPartyVehicle.coverStartDate =
            this.caseThirdPartyVehicle.coverStartDate != null
                ? DateTime.fromJSDate(new Date(this.caseThirdPartyVehicle.coverStartDate.toLocaleString()))
                : undefined;
        this.caseThirdPartyVehicle.coverEndDate =
            this.caseThirdPartyVehicle.coverEndDate != null
                ? DateTime.fromJSDate(new Date(this.caseThirdPartyVehicle.coverEndDate.toLocaleString()))
                : undefined;

        this._caseThirdPartyVehiclesServiceProxy
            .createOrEdit(this.caseThirdPartyVehicle)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));

                this.show();
                this.getCaseThirdPartyVehicles();
                this.clearFileInput();

                this.markFormAsPristine(this.caseThirdPartyVehicleForm);
                this.caseThirdPartyVehicle = new CreateOrEditCaseThirdPartyVehicleDto();
                this.caseThirdPartyVehicleForm.resetForm();
            });
    }

    getCaseThirdPartyVehicles(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseThirdPartyVehiclesServiceProxy
            .getAll(
                this.registerIdFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                50,
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    deleteCaseThirdPartyVehicle(caseThirdPartyVehicle: CaseThirdPartyVehicleDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseThirdPartyVehiclesServiceProxy.delete(caseThirdPartyVehicle.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    editCaseThirdPartyVehicle(record: CaseThirdPartyVehicleDto): void {
        this.active = true;
        this.show(record.id);
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseThirdPartyVehicleForm.dirty) {
            var returnVal = this.swalAlert.fire({
                title: 'Are you sure you want to leave?',
                text: 'Your changes will be lost.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, proceed!',
            });

            return returnVal.then((result) => result.isConfirmed);
        } else {
            return true;
        }
    }
    //TPV Detail
    TpvUploader: FileUploader;
    TpvFileUploadFileToken: string;
    TpvFileUploadFileName: string;
    TpvFileUploadFileAcceptedTypes: string = '';

    uploadedFiles: { name: string; token: string }[] = [];
    selectedFiles: File[] = []; // Store files to upload
    fileTokens: { file: File; token?: string }[] = [];
    //Car Grant
    driverCarGrantFileUploader: FileUploader;
    driverCarGrantFileToken: string;
    driverCarGrantFileName: string;
    driverCarGrantFileAcceptedTypes: string = '';
    initializeUploader(url: string, onSuccess: (result: any) => void): FileUploader {
        let uploader = new FileUploader({ url: url });
        var token = 'Bearer ' + this._tokenService.getToken();

        let _uploaderOptions: FileUploaderOptions = { url: url };
        _uploaderOptions.autoUpload = false;
        _uploaderOptions.authToken = token;
        _uploaderOptions.removeAfterUpload = true;

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
            console.log(this.caseThirdPartyVehicle.tpvCarGrantToken);
            this.caseThirdPartyVehicleForm.form.markAsDirty();
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

    showModal() {
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            this.TpvFileUploadFileAcceptedTypes = list.map((ext) => '.' + ext).join(',');
        });

        this.TpvUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${this.registerIdFilter}&docTypeValue=None`,
            (fileToken) => {
                this.caseThirdPartyVehicle.tpvDetailsToken = fileToken;
                console.log('fileToken', fileToken);
                console.log('this.caseThirdPartyVehicle.tpvDetailsToken ', this.caseThirdPartyVehicle.tpvDetailsToken);
            },
        );

        this.driverCarGrantFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl +
                `/fileOcr/UploadFile?caseId=${this.registerIdFilter}&docTypeValue=CarGrant`,
            ({ content, fileToken }) => {
                try {
                    this.caseThirdPartyVehicle.vehicleNo =
                        content.JPJRegisterNo == null ? content.jpjRegisterNo : content.JPJRegisterNo;
                    this.caseThirdPartyVehicle.vehicleMake = content.make;
                    this.caseThirdPartyVehicle.vehicleYear = content.year;
                    this.caseThirdPartyVehicle.tpvCarGrantToken = fileToken;
                    console.log(fileToken);
                    this.driverCarGrantFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                    // this.clearFileInput();
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );
        this.fileUploadModal.show();
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    clearFileInput(): void {
        this.TPVDetFileInput.nativeElement.value = '';
        this.TPVGrantFileInput.nativeElement.value = '';
        this.driverCarGrantFileName = '';
        this.driverCarGrantFileUploaded = false;
    }

    onSelectTPVFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.TpvUploader.clearQueue();
        this.TpvUploader.addToQueue([selectedFile]);
        this.TpvUploader.uploadAll();
    }

    removeFileUploadFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                // Implement file remove
            }
        });
    }

    onSelectDriverCarGrantFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverCarGrantFileUploader.clearQueue();
        this.driverCarGrantFileUploader.addToQueue([selectedFile]);
        this.driverCarGrantFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverCarGrantFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseThirdPartyVehiclesServiceProxy
                    .removeDriverCarGrantFile(this.caseThirdPartyVehicle.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverCarGrantFileName = null;
                        this.driverCarGrantFileUploaded = false;
                    });
            }
        });
    }
    // removeDriverDetFile(): void {
    //     this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
    //         if (isConfirmed) {
    //             this._caseThirdPartyVehiclesServiceProxy
    //                 .removeDriverCarGrantFile(this.caseThirdPartyVehicle.id)
    //                 .subscribe(() => {
    //                     abp.notify.success(this.l('SuccessfullyDeleted'));
    //                     this.driverCarGrantFileName = null;
    //                     this.driverCarGrantFileUploaded = false;
    //                 });
    //         }
    //     });
    // }

    close() {
        this.fileUploadModal.hide();
    }
}

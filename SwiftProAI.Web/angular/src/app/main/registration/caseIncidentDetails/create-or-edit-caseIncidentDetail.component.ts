import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef, viewChild } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseIncidentDetailsServiceProxy,
    CreateOrEditCaseIncidentDetailDto,
    CaseIncidentDetailMainRegistrationLookupTableDto,
    LookupsServiceProxy,
    CommonDropdownDto,
    GetLookupByGroupDto,
    CommonDropdownServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileUploader, FileUploaderOptions } from '@node_modules/ng2-file-upload';
import { IAjaxResponse, TokenService } from '@node_modules/abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ModalDirective } from 'ngx-bootstrap/modal';
import Swal from 'sweetalert2';

@Component({
    templateUrl: './create-or-edit-caseIncidentDetail.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseIncidentDetailComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    today = new Date();
    registerId = '';
    timeFrom: any;
    timeTo: any;

    caseIncidentDetail: CreateOrEditCaseIncidentDetailDto = new CreateOrEditCaseIncidentDetailDto();
    circumstancesFileUploadFileUploader: FileUploader;
    circumstancesFileUploadFileToken: string;
    circumstancesFileUploadFileName: string;
    circumstancesFileUploadFileAcceptedTypes: string = '';
    circumstancesFileUploadFileUploaded: boolean;
    @ViewChild('CaseIncidentDetail_circumstancesFileUploadLabel')
    caseIncidentDetail_circumstancesFileUploadLabel: ElementRef;
    @ViewChild('caseIncidentDetailForm') caseIncidentDetailForm: NgForm;
    @ViewChild('fileUploadModal', { static: true }) fileUploadModal: ModalDirective;

    countries: CommonDropdownDto[];
    states: CommonDropdownDto[];
    drivingWith: GetLookupByGroupDto[];
    typesOfRoad: GetLookupByGroupDto[];
    centerDemarcations: GetLookupByGroupDto[];
    viewsFromEitherDirection: GetLookupByGroupDto[];
    visibilities: GetLookupByGroupDto[];
    visibilityReasons: GetLookupByGroupDto[];
    surroundingAreas: GetLookupByGroupDto[];
    roadConditions: GetLookupByGroupDto[];
    weatherConditions: GetLookupByGroupDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseIncidentDetailsServiceProxy: CaseIncidentDetailsServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _http: HttpClient,
        public navigationService: NavigationService,
        private _lookupService: LookupsServiceProxy,
        private _commonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
    }
    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseIncidentDetailForm && this.caseIncidentDetailForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseIncidentDetailForm && this.caseIncidentDetailForm.controls) {
            Object.keys(this.caseIncidentDetailForm.controls).forEach((controlName) => {
                const control = this.caseIncidentDetailForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    getLookupData(): void {
        //Enter the list of lookupGroups needed in here to retrieve from backend.
        const lookupGroups = [
            'DriverStatus',
            'RoadView',
            'RoadType',
            'Visibility',
            'CenterDemarcation',
            'VisibilityReason',
            'SurroundingArea',
            'RoadCondition',
            'WeatherCondition',
        ];
        //Map the results back to each dropdown
        this._lookupService.getAllLookupsByGroups(lookupGroups).subscribe(
            (allLookups: { [key: string]: GetLookupByGroupDto[] }) => {
                this.drivingWith = allLookups['DriverStatus'];
                this.viewsFromEitherDirection = allLookups['RoadView'];
                this.typesOfRoad = allLookups['RoadType'];
                this.visibilities = allLookups['Visibility'];
                this.centerDemarcations = allLookups['CenterDemarcation'];
                this.visibilityReasons = allLookups['VisibilityReason'];
                this.surroundingAreas = allLookups['SurroundingArea'];
                this.roadConditions = allLookups['RoadCondition'];
                this.weatherConditions = allLookups['WeatherCondition'];
            },
            (error) => {
                console.error('Error fetching lookup data:', error);
                // Handle error as needed
            },
        );
        this._commonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe((result) => {
            this.countries = result;
        });
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerId;
        this.getLookupData();

        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.circumstancesFileUploadFileAcceptedTypes += '.' + list[i] + ',';
            }
        });
    }

    show(registerId?: number): void {
        this._caseIncidentDetailsServiceProxy.getCaseIncidentDetailForEdit(registerId).subscribe((result) => {
            this.caseIncidentDetail = result.caseIncidentDetail;
            if (!this.caseIncidentDetail) {
                this.caseIncidentDetail = new CreateOrEditCaseIncidentDetailDto();
                this.timeFrom = this._dateTimeService.getStartOfDay();
                this.timeTo = this._dateTimeService.getStartOfDay();
                this.circumstancesFileUploadFileName = null;
            }
            this.timeFrom = this.caseIncidentDetail.timeFrom;
            this.timeTo = this.caseIncidentDetail.timeTo;

            this.caseIncidentDetail.registerId = registerId;
            this.circumstancesFileUploadFileName = result.circumstancesFileUploadFileName;
            this.active = true;

            this.markFormAsPristine(this.caseIncidentDetailForm);
        });

        this.circumstancesFileUploadFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            (fileToken) => (this.caseIncidentDetail.circumstancesFileUploadToken = fileToken),
        );
    }

    showModal() {
        this.fileUploadModal.show();
    }
    close() {
        this.fileUploadModal.hide();
    }

    save(): void {
        this.caseIncidentDetail.timeFrom = DateTime.fromJSDate(new Date(this.timeFrom));
        this.caseIncidentDetail.timeTo = DateTime.fromJSDate(new Date(this.timeTo));

        this.saving = true;
        this._caseIncidentDetailsServiceProxy
            .createOrEdit(this.caseIncidentDetail)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseIncidentDetailForm.control.markAsPristine();
                //  this._router.navigate( ['/app/main/registration/caseIncidentDetails']);
            });
    }

    // saveAndNew(): void {
    //     this.saving = true;

    //     this._caseIncidentDetailsServiceProxy.createOrEdit(this.caseIncidentDetail)
    //         .pipe(finalize(() => {
    //             this.saving = false;
    //         }))
    //         .subscribe(x => {
    //             this.saving = false;
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.caseIncidentDetail = new CreateOrEditCaseIncidentDetailDto();
    //         });
    // }

    onCountrySelect(parentLocationId): void {
        this._commonDropdownService.getAllLocationByStateForTableDropdown(parentLocationId).subscribe((result) => {
            this.states = result;
        });
    }

    onSelectCircumstancesFileUploadFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.circumstancesFileUploadFileUploader.clearQueue();
        this.circumstancesFileUploadFileUploader.addToQueue([selectedFile]);
        this.circumstancesFileUploadFileUploader.uploadAll();
        this.circumstancesFileUploadFileUploaded = true;
    }

    removeCircumstancesFileUploadFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseIncidentDetailsServiceProxy
                    .removeCircumstancesFileUploadFile(this.caseIncidentDetail.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.circumstancesFileUploadFileName = null;
                        this.circumstancesFileUploadFileUploaded = false;
                    });
            }
        });
    }

    initializeUploader(url: string, onSuccess: (fileToken: string) => void): FileUploader {
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
            if (resp.success && resp.result.fileToken) {
                onSuccess(resp.result.fileToken);
            } else {
                this.message.error(resp.result.message);
            }
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseIncidentDetailForm.dirty) {
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
}

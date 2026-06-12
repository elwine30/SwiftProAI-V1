import { Component, ViewChild, Injector, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    InsuredPersonsServiceProxy,
    CreateOrEditInsuredPersonDto,
    InsuredPersonMainRegistrationLookupTableDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    GetLookupByGroupDto,
    LookupsServiceProxy,
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
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    templateUrl: './create-or-edit-caseInsuredDriver.component.html',
    styleUrls: ['./create-or-edit-caseInsuredDriver.component.css'],
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseInsuredDriverComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    swalAlert = Swal;

    caseInsuredDriver: CreateOrEditInsuredPersonDto = new CreateOrEditInsuredPersonDto();

    currentId: number | undefined;
    registerId = '';
    hospitalName = '';
    hospitalAddress = '';
    locationName = '';
    locationName2 = '';

    allMainRegistrations: InsuredPersonMainRegistrationLookupTableDto[];
    allHospitals: CommonDropdownDto[];
    allCountryLocations: CommonDropdownDto[];
    allStateLocations: CommonDropdownDto[];
    Relationship: GetLookupByGroupDto[];
    IDType: GetLookupByGroupDto[];

    driverICFrontFileUploader: FileUploader;
    driverICFrontFileToken: string;
    driverICFrontFileName: string;
    driverICFrontFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverICFrontLabel') caseInsuredDriver_driverICFrontLabel: ElementRef;
    driverICFrontFileUploaded: boolean;

    driverICBackFileUploader: FileUploader;
    driverICBackFileToken: string;
    driverICBackFileName: string;
    driverICBackFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverICBackLabel') caseInsuredDriver_driverICBackLabel: ElementRef;
    driverICBackFileUploaded: boolean;

    driverLicenseFrontFileUploader: FileUploader;
    driverLicenseFrontFileToken: string;
    driverLicenseFrontFileName: string;
    driverLicenseFrontFileAcceptedTypes: string = '';
    driverLicenseFrontUploaded: boolean;

    @ViewChild('CaseInsuredDriver_driverLicenseFrontLabel') caseInsuredDriver_driverLicenseFrontLabel: ElementRef;

    driverLicenseBackFileUploader: FileUploader;
    driverLicenseBackFileToken: string;
    driverLicenseBackFileName: string;
    driverLicenseBackFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverLicenseBackLabel') caseInsuredDriver_driverLicenseBackLabel: ElementRef;
    driverLicenseBackFileUploaded: boolean;

    driverEmploymentDetailFileUploader: FileUploader;
    driverEmploymentDetailFileToken: string;
    driverEmploymentDetailFileName: string;
    driverEmploymentDetailFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverEmploymentDetailLabel')
    caseInsuredDriver_driverEmploymentDetailLabel: ElementRef;
    driverEmploymentDetailFileuploaded: boolean;

    driverHospitalDetailFileUploader: FileUploader;
    driverHospitalDetailFileToken: string;
    driverHospitalDetailFileName: string;
    driverHospitalDetailFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverHospitalDetailLabel') caseInsuredDriver_driverHospitalDetailLabel: ElementRef;
    driverHospitalDetailFileUploaded: boolean;

    driverCarGrantFileUploader: FileUploader;
    driverCarGrantFileToken: string;
    driverCarGrantFileName: string;
    driverCarGrantFileAcceptedTypes: string = '';
    @ViewChild('CaseInsuredDriver_driverCarGrantLabel') CaseInsuredDriver_driverCarGrantLabel: ElementRef;
    driverCarGrantFileUploaded: boolean;

    @ViewChild('caseDriverPersonForm') caseDriverPersonForm: NgForm;

    SameOwnerRelationship = [{ value: 'Self' }];

    isSameAsOwner = false;

    @ViewChild('fileUploadModal', { static: true }) fileUploadModal: ModalDirective;

    jpjRegisterDateString: any;
    licenseDateFromString: any;
    licenseDateToString: any;
    licenseNRIC: string;
    mainRegistrationVehicleNo = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseInsuredDriversServiceProxy: InsuredPersonsServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _http: HttpClient,
        public navigationService: NavigationService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _lookupService: LookupsServiceProxy,
        private fb: FormBuilder,
    ) {
        super(injector);
    }
    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseDriverPersonForm && this.caseDriverPersonForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseDriverPersonForm && this.caseDriverPersonForm.controls) {
            Object.keys(this.caseDriverPersonForm.controls).forEach((controlName) => {
                const control = this.caseDriverPersonForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }
    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerId;
        this.show(false, this._activatedRoute.snapshot.queryParams['id']);
        // this.relationship = this.difOwnerRelationship;
        this._lookupService.getByGroup('Relationship').subscribe((result) => {
            this.Relationship = result;
        });
        this._lookupService.getByGroup('IDType').subscribe((result) => {
            this.IDType = result;
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
                this.driverICFrontFileAcceptedTypes += '.' + list[i] + ',';
                this.driverICBackFileAcceptedTypes += '.' + list[i] + ',';
                this.driverLicenseFrontFileAcceptedTypes += '.' + list[i] + ',';
                this.driverLicenseBackFileAcceptedTypes += '.' + list[i] + ',';
                this.driverCarGrantFileAcceptedTypes += '.' + list[i] + ',';
                this.driverEmploymentDetailFileAcceptedTypes += '.' + list[i] + ',';
                this.driverHospitalDetailFileAcceptedTypes += '.' + list[i] + ',';
            }
        });
    }

    loadGetCaseInsuredDriverDataOnly(showOwnerData: boolean, registerId?: number) {
        this._caseInsuredDriversServiceProxy.getInsuredPersonForEdit(registerId, showOwnerData).subscribe((result) => {
            if (!this.caseInsuredDriver.name && result.insuredPerson) {
                if (result.insuredPerson.relationship == 'Self') {
                    this.isSameAsOwner = true;
                }
            }
            this.caseInsuredDriver = result.insuredPerson;

            if (!this.caseInsuredDriver) {
                this.caseInsuredDriver = new CreateOrEditInsuredPersonDto();
                this.hospitalName = '';
                this.locationName = '';
                this.locationName2 = '';

                this.driverICFrontFileName = null;
                this.driverICBackFileName = null;
                this.driverLicenseFrontFileName = null;
                this.driverLicenseBackFileName = null;
                this.driverEmploymentDetailFileName = null;
                this.driverHospitalDetailFileName = null;

                this.caseInsuredDriver.valuation = null;
                this.caseInsuredDriver.year = null;
                this.caseInsuredDriver.valuationEquiry = null;
            }

            this.caseInsuredDriver.registerId = registerId;
            this.hospitalName = result.hospitalName;
            this.hospitalAddress = result.hospitalAddress;
            this.locationName = result.locationName;
            this.locationName2 = result.locationName2;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;

            this.driverICFrontFileName = result.driverICFrontFileName;
            this.driverICBackFileName = result.driverICBackFileName;
            this.driverLicenseFrontFileName = result.driverLicenseFrontFileName;
            this.driverLicenseBackFileName = result.driverLicenseBackFileName;
            this.driverEmploymentDetailFileName = result.driverEmploymentDetailFileName;
            this.driverHospitalDetailFileName = result.driverHospitalDetailFileName;
            this.driverCarGrantFileName = result.driverCarGrantFileName;

            // format the date to string for input field
            this.jpjRegisterDateString = this.caseInsuredDriver.jpjRegisterDate
                ? this.caseInsuredDriver.jpjRegisterDate.toFormat('dd/MM/yyyy')
                : null;
            // this.licenseDateFromString = this.caseInsuredDriver.licenseDateFrom
            //     ? this.caseInsuredDriver.licenseDateFrom.toFormat('dd/MM/yyyy')
            //     : null;
            // this.licenseDateToString = this.caseInsuredDriver.licenseDateTo
            //     ? this.caseInsuredDriver.licenseDateTo.toFormat('dd/MM/yyyy')
            //     : null;

            this.licenseDateFromString = this.caseInsuredDriver.licenseDateFrom.isValid ? this.caseInsuredDriver.licenseDateFrom.toJSDate() : null;
            this.licenseDateToString = this.caseInsuredDriver.licenseDateTo.isValid ? this.caseInsuredDriver.licenseDateTo.toJSDate() : null;

            this.active = true;
            this.onCountrySelect(this.caseInsuredDriver.countryLocationId);
        });
    }

    show(showOwnerData: boolean, registerId?: number): void {
        this.loadGetCaseInsuredDriverDataOnly(showOwnerData, registerId);
        this._caseInsuredDriversServiceProxy.getAllMainRegistrationForTableDropdown().subscribe((result) => {
            this.allMainRegistrations = result;
        });
        this._CommonDropdownService.getAllHospitalForTableDropdown().subscribe((result) => {
            this.allHospitals = result;
        });
        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe((result) => {
            this.allCountryLocations = result;
        });

        this.driverICFrontFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=ICFront`,
            ({ content, fileToken }) => {
                try {
                    this.caseInsuredDriver.driverICFrontToken = fileToken;
                    this.caseInsuredDriver.identicationNo = content.identityNumber;
                    this.caseInsuredDriver.identicationType = content.identityType;
                    this.caseInsuredDriver.name = content.fullName;
                    this.caseInsuredDriver.address = content.addressLine;
                    this.caseInsuredDriver.postcode = content.postcode;
                    this.caseInsuredDriver.city = content.city;
                    this.caseInsuredDriver.nationality = content.nationality;

                    var selectedCountry = this.allCountryLocations.find(
                        (x) => x.displayName.toLowerCase() == content.country.trim().toLowerCase(),
                    );
                    this.caseInsuredDriver.countryLocationId = selectedCountry ? selectedCountry.id : null;
                    this._CommonDropdownService
                        .getAllLocationByStateForTableDropdown(selectedCountry ? selectedCountry.id : 1)
                        .subscribe((result) => {
                            this.allStateLocations = result;
                            var selectedState = result.find(
                                (x) => x.displayName.toLowerCase() == content.state.trim().toLowerCase(),
                            );
                            this.caseInsuredDriver.stateLocationId = selectedState ? selectedState.id : null;
                        });

                    this.driverICFrontFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverICBackFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            (fileToken) => (this.caseInsuredDriver.driverICBackToken = fileToken),
        );

        this.driverLicenseFrontFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=LicenseFront`,
            ({ content, fileToken }) => {
                try {
                    this.licenseNRIC = content.identityNumber;
                    const licenseFromDateTime = DateTime.fromFormat(content.licenseDateFrom, 'dd/MM/yyyy');
                    const licenseToDateTime = DateTime.fromFormat(content.licenseDateTo, 'dd/MM/yyyy');

                    this.licenseDateFromString = licenseFromDateTime.isValid ? licenseFromDateTime.toJSDate() : null;
                    this.licenseDateToString = licenseToDateTime.isValid ? licenseToDateTime.toJSDate() : null;

                    this.caseInsuredDriver.licenseClasses =
                        content.licenseClasses != null ? content.licenseClasses.join(', ') : null;
                    this.caseInsuredDriver.driverLicenseFrontToken = fileToken;
                    this.driverLicenseFrontUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverLicenseBackFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=LicenseBack`,
            ({ content, fileToken }) => {
                try {
                    this.caseInsuredDriver.licenseNo = content.licenseNo;
                    this.caseInsuredDriver.driverLicenseBackToken = fileToken;
                    this.driverLicenseBackFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverEmploymentDetailFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            ({ content, fileToken }) => {
                console.log('This is the content for employment', content);
                try {
                    this.caseInsuredDriver.employerName = content.employerName;
                    this.caseInsuredDriver.employerContact = content.employerContact;
                    this.caseInsuredDriver.employerAddress = content.employerAddress;
                    //Need to parse values from string to int
                    const parsedIncome = Number(content.monthlyIncome.replace(/[^0-9.-]+/g, ''));
                    this.caseInsuredDriver.monthlyIncome = parsedIncome;
                    this.caseInsuredDriver.driverEmploymentDetailToken = fileToken;
                    this.driverEmploymentDetailFileuploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverHospitalDetailFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            (fileToken) => (this.caseInsuredDriver.driverHospitalDetailToken = fileToken),
        );

        this.driverCarGrantFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=CarGrant`,
            ({ content, fileToken }) => {
                try {
                    this.caseInsuredDriver.make = content.make;
                    this.caseInsuredDriver.model = content.model;
                    this.caseInsuredDriver.specs = content.specs;
                    this.caseInsuredDriver.year = content.year;
                    const jpjRegisterDate = DateTime.fromFormat(content.jpjRegisterDate, 'dd/MM/yyyy');
                    this.jpjRegisterDateString = jpjRegisterDate.isValid ? jpjRegisterDate.toJSDate() : null;
                    this.caseInsuredDriver.jpjRegisterNo = content.jpjRegisterNo;
                    this.caseInsuredDriver.driverCarGrantToken = fileToken;
                    this.driverCarGrantFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );
    }

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
            this.caseDriverPersonForm.form.markAsDirty();
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

    onchange() {
        this.loadGetCaseInsuredDriverDataOnly(this.isSameAsOwner, parseInt(this.registerId));
    }

    updateHospitalAddress(hospitalId: number): void {
        if (hospitalId) {
            this._CommonDropdownService.getHospitalByIdForTableDropdown(hospitalId).subscribe((result) => {
                if (!result || !result.displayName) return;
                this.hospitalAddress = result.address;
            });
        } else {
            this.hospitalAddress = '';
        }
    }

    save(): void {
        //format back the date to DateTime to save.
        this.caseInsuredDriver.jpjRegisterDate =
            this.jpjRegisterDateString != null ? DateTime.fromJSDate(new Date(this.jpjRegisterDateString)) : null;
        this.caseInsuredDriver.licenseDateFrom =
            this.licenseDateFromString != null ? DateTime.fromJSDate(new Date(this.licenseDateFromString)) : null;
        this.caseInsuredDriver.licenseDateTo =
            this.licenseDateToString != null ? DateTime.fromJSDate(new Date(this.licenseDateToString)) : null;

        // NRIC and License checking
        if (this.driverLicenseFrontUploaded) {
            console.log('checking the ic and licensee');
            const NRICIdentityNo = this.caseInsuredDriver.identicationNo.replace(/-/g, '');
            const LicenseIdentityNo = this.licenseNRIC.replace(/-/g, '');
            if (NRICIdentityNo != LicenseIdentityNo) {
                this.message.error(this.l('ICDoesNotMatch'));
                return;
            }
        }

        this.saving = true;
        this.caseInsuredDriver.isDriver = true;

        if (this.caseInsuredDriver.isOwner) {
            this.caseInsuredDriver.id = null;
        }

        this.caseInsuredDriver.isOwner = false;
        this._caseInsuredDriversServiceProxy
            .createOrEditInsuredDriver(this.caseInsuredDriver)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseDriverPersonForm.control.markAsPristine();
            });
    }

    onSelectDriverICFrontFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverICFrontFileUploader.clearQueue();
        this.driverICFrontFileUploader.addToQueue([selectedFile]);
        this.driverICFrontFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverICFrontFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseInsuredDriversServiceProxy
                    .removeDriverICFrontFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverICFrontFileName = null;
                        this.driverICFrontFileUploaded = false;
                    });
            }
        });
    }

    onSelectDriverICBackFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverICBackFileUploader.clearQueue();
        this.driverICBackFileUploader.addToQueue([selectedFile]);
        this.driverICBackFileUploader.uploadAll();
        this.driverICBackFileUploaded = true;
    }

    removeDriverICBackFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseInsuredDriversServiceProxy.removeDriverICBackFile(this.caseInsuredDriver.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverICBackFileName = null;
                    this.driverICBackFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverLicenseFrontFile(fileInput: any): void {
        var selectedFile = <File>fileInput.target.files[0];

        this.driverLicenseFrontFileUploader.clearQueue();
        this.driverLicenseFrontFileUploader.addToQueue([selectedFile]);
        this.driverLicenseFrontFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverLicenseFrontFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                document.getElementById('CaseInsuredDriver_DriverLicenseFront')['value'] = '';
                this._caseInsuredDriversServiceProxy
                    .removeDriverLicenseFrontFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverLicenseFrontFileName = null;
                        this.driverLicenseBackFileUploaded = false;
                    });
            }
        });
    }

    onSelectDriverLicenseBackFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverLicenseBackFileUploader.clearQueue();
        this.driverLicenseBackFileUploader.addToQueue([selectedFile]);
        this.driverLicenseBackFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverLicenseBackFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseInsuredDriversServiceProxy
                    .removeDriverLicenseBackFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverLicenseBackFileName = null;
                        this.driverLicenseBackFileUploaded = false;
                    });
            }
        });
    }

    onSelectDriverEmploymentDetailFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverEmploymentDetailFileUploader.clearQueue();
        this.driverEmploymentDetailFileUploader.addToQueue([selectedFile]);
        this.driverEmploymentDetailFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverEmploymentDetailFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseInsuredDriversServiceProxy
                    .removeDriverEmploymentDetailFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverEmploymentDetailFileName = null;
                        this.driverEmploymentDetailFileuploaded = false;
                    });
            }
        });
    }

    onSelectDriverHospitalDetailFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverHospitalDetailFileUploader.clearQueue();
        this.driverHospitalDetailFileUploader.addToQueue([selectedFile]);
        this.driverHospitalDetailFileUploader.uploadAll();
        this.driverHospitalDetailFileUploaded = true;
    }

    removeDriverHospitalDetailFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseInsuredDriversServiceProxy
                    .removeDriverHospitalDetailFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverHospitalDetailFileName = null;
                        this.driverHospitalDetailFileUploaded = false;
                    });
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
                this._caseInsuredDriversServiceProxy
                    .removeDriverCarGrantFile(this.caseInsuredDriver.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverCarGrantFileName = null;
                        this.driverCarGrantFileUploaded = false;
                    });
            }
        });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    onCountrySelect(parentLocationId): void {
        if (parentLocationId != null) {
            this._CommonDropdownService.getAllLocationByStateForTableDropdown(parentLocationId).subscribe((result) => {
                this.allStateLocations = result;
            });
        }
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseDriverPersonForm.dirty) {
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

    showModal() {
        this.fileUploadModal.show();
    }

    close() {
        this.fileUploadModal.hide();
    }
}
